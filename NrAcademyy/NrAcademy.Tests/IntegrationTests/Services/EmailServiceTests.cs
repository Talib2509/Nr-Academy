using Microsoft.Extensions.Options;
using Moq;
using NrAcademy.Tests.IntegrationTests;
using NrAcademyBL.Configuration;
using NrAcademyBL.DTOs;
using NrAcademyBL.Services.Concrete;
using Xunit;

namespace NrAcademy.Tests.IntegrationTests.Services;

public class EmailServiceTests
{
    private readonly EmailService _emailService;
    private readonly EmailSettings _emailSettings;

    public EmailServiceTests()
    {
        // 1. Ayarları simulyasiya edirik
        _emailSettings = new EmailSettings
        {
            Host = "smtp.gmail.com",
            Port = 587,
            Username = "test@nracademy.az",
            Password = "password",
            FromEmail = "no-reply@nracademy.az",
            FromName = "NR Academy"
        };

        var options = Options.Create(_emailSettings);
        _emailService = new EmailService(options);
    }

    [Fact]
    public async Task SendVerificationEmailAsync_With_Invalid_Config_Should_Throw_Exception()
    {
        
        string testEmail = "testuser@gmail.com";
        string code = "123456";

       
        await Assert.ThrowsAnyAsync<Exception>(() =>
            _emailService.SendVerificationEmailAsync(testEmail, code));
    }

    [Fact]
    public void EmailSettings_Should_Be_Correctly_Injected()
    {
   

        Assert.NotNull(_emailService);
    }
}