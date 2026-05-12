using AutoMapper;
using NrAcademyBL.DTOs.CertificateDTO;
using NrAcademyBL.DTOs.TestResultDTO;
using NrAcademyBL.Exceptions.TestResult;
using NrAcademyBL.Extensions.Caching;
using NrAcademyBL.Services.Abstract;
using NrAcademyCORE.Entities;
using NrAcademyCORE.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NrAcademyBL.Services.Concrete
{
    public class TestResultService : ITestResultService
    {
        private readonly ITestResultRepository _repository;
        private readonly ITestRepository _testRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly ICertificateService _certificateService;
        private readonly IMapper _mapper;
        private readonly ICacheService _cache;

        public TestResultService(
            ITestResultRepository repository,
            ITestRepository testRepository,
            IQuestionRepository questionRepository,
            ICertificateService certificateService,
            IMapper mapper,
            ICacheService cache)
        {
            _repository = repository;
            _testRepository = testRepository;
            _questionRepository = questionRepository;
            _certificateService = certificateService;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<TestResultItemDto> SubmitTestAsync(int userId, TestSubmitDto dto)
        {
            
            var test = await _testRepository.GetByIdAsync(dto.TestId);
            if (test == null) throw new Exception("Test tapılmadı.");

            var now = DateTime.UtcNow;
            var timeTaken = (now - dto.StartedAt.ToUniversalTime()).TotalMinutes;
       

            // Əgər müəllim testə vaxt qoyubsa (> 0) və şagird bu vaxtı keçibsə
            // (+2 dəqiqə əlavə edirik ki, internet zəif olanda şagirdin haqqı getməsin)
            if (test.DurationInMinutes > 0 && timeTaken > (test.DurationInMinutes + 2))
            {
                throw new Exception($"Diqqət: Bu test üçün ayrılmış vaxt ({test.DurationInMinutes} dəqiqə) bitmişdir. Təəssüf ki, nəticəniz qəbul edilmədi.");
            }

           
            var questions = await _questionRepository.GetAllAsync(
                filter: q => q.TestId == dto.TestId,
                includeProperties: "Answers");

            var questionList = questions.ToList();
            if (!questionList.Any()) throw new Exception("Testdə sual tapılmadı.");

            int correctCount = 0;
            foreach (var userAnswer in dto.UserAnswers)
            {
                var question = questionList.FirstOrDefault(q => q.Id == userAnswer.QuestionId);
                if (question != null && question.Answers != null)
                {
                    var correctAnswer = question.Answers.FirstOrDefault(a => a.IsCorrect);
                    if (correctAnswer != null && correctAnswer.Id == userAnswer.SelectedAnswerId)
                        correctCount++;
                }
            }

            int score = (int)Math.Round((double)correctCount / questionList.Count * 100);

            
            var result = new TestResult
            {
                UserId = userId,
                TestId = dto.TestId,
                Score = score,
                StartedAt = dto.StartedAt,
                CompletedAt = DateTime.Now,
                IsWinner = false
            };

            await _repository.AddAsync(result);
            await _cache.RemoveAsync("testresults_all");

            // 5. 80% LİMİTİ VƏ SERTİFİKAT YARADILMASI
            if (score >= 80)
            {
                await _certificateService.CreateAsync(new CertificateCreateDTO
                {
                    UserId = userId,
                    CourseId = test.CourseId,
                    TestTitle = test.Title,
                    Score = score,
                    CertificateType = "Kursu Bitirmə",
                    CertificateUrl = $"https://nracademy.com/certs/view/{result.Id}" 
                });
            }

            return _mapper.Map<TestResultItemDto>(result);
        }

        public async Task<List<TestResultItemDto>> GetResultsByTestIdAsync(int testId)
        {
            var results = await _repository.GetResultsByTestIdWithUserAsync(testId);
            return _mapper.Map<List<TestResultItemDto>>(results);
        }

        public async Task DetermineWinnerForTestAsync(int testId)
        {
            var winner = await _repository.GetWinnerForTestAsync(testId);
            var test = await _testRepository.GetByIdAsync(testId);

            if (winner != null && test != null)
            {
                winner.IsWinner = true;
                await _repository.UpdateAsync(winner);

                await _certificateService.CreateAsync(new CertificateCreateDTO
                {
                    UserId = winner.UserId,
                    CourseId = test.CourseId,
                    TestTitle = test.Title,
                    Score = winner.Score,
                    CertificateType = "Günün Qalibi",
                    CertificateUrl = $"https://nracademy.com/certs/verify/{winner.Id}"
                });

                await _cache.RemoveAsync("testresults_all");
            }
        }

        public async Task<List<TestResultItemDto>> GetUserResultsAsync(int userId)
        {
            var results = await _repository.GetAllAsync(filter: r => r.UserId == userId);
            return _mapper.Map<List<TestResultItemDto>>(results);
        }

        public async Task<List<TestResultItemDto>> GetAllAsync() =>
            _mapper.Map<List<TestResultItemDto>>(await _repository.GetAllAsync());

        public async Task<TestResultItemDto> GetByIdAsync(int id)
        {
            var data = await _repository.GetByIdAsync(id);
            if (data == null) throw TestResultException.NotFound(id);
            return _mapper.Map<TestResultItemDto>(data);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) throw TestResultException.NotFound(id);
            await _repository.DeleteAsync(entity);
            await _cache.RemoveAsync("testresults_all");
        }
    }
}