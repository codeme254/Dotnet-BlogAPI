namespace BlogAPI.Services;

public interface IEmailTemplateService
{
    string GetVerificationEmailBody(string verificationUrl);
}