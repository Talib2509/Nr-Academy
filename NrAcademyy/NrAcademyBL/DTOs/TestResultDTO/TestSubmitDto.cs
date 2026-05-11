using System;
using System.Collections.Generic;

namespace NrAcademyBL.DTOs.TestResultDTO
{
    public class TestSubmitDto
    {
        public int TestId { get; set; }
        public DateTime StartedAt { get; set; }
        public List<UserAnswerDto> UserAnswers { get; set; } = new();
    }

    public class UserAnswerDto
    {
        public int QuestionId { get; set; }
        public int SelectedAnswerId { get; set; }
    }
}