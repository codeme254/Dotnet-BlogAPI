namespace BlogAPI.Services.Implementations;

public class EmailTemplateService(IWebHostEnvironment env) : IEmailTemplateService
{
    private readonly string _templatesPath = Path.Combine(env.ContentRootPath, "EmailTemplates");

    private string LoadTemplate(string templateName)
    {
        var path = Path.Combine(_templatesPath, templateName);
        return File.ReadAllText(path);
    }
    public string GetVerificationEmailBody(string verificationUrl)
    {
        return LoadTemplate("VerificationEmail.html")
        .Replace("{{VERIFICATION_URL}}", verificationUrl);
    }
}