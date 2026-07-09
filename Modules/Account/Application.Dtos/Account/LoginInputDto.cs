namespace Modules.Account.Application.Dtos.Account;
public class LoginInputDto
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string ReCaptchaToken { get; set; }

    public string? RemoteIpAddress { get; set; } = null;
    public string UserAgentString { get; set; } = string.Empty;
}
