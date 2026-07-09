namespace Modules.Account.Domain.ExternalServices;
public interface IGoogleReCaptchaService
{
    Task VerifyLoginAsync(string token, CancellationToken cancellationToken);
}
