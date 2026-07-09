using Microsoft.Extensions.Options;
using Modules.Account.Application.Dtos.Account;
using Modules.Account.Domain.ExternalServices;
using Modules.Account.Domain.Shared.Consts;
using System.Net.Http.Json;

namespace Modules.Account.Infra.ExternalServices;
public class GoogleReCaptchaService : IGoogleReCaptchaService
{
    private readonly HttpClient _httpClient;
    private readonly GoogleReCaptchaSettings _googleReCaptchaSettings;

    public GoogleReCaptchaService(
        HttpClient httpClient,
        IOptions<GoogleReCaptchaSettings> googleReCaptchaSettingsOptions)
    {
        _httpClient = httpClient;
        _googleReCaptchaSettings = googleReCaptchaSettingsOptions.Value;
    }

    public async Task VerifyLoginAsync(string token, CancellationToken cancellationToken)
    {
        var verifyResponse = await _httpClient.GetFromJsonAsync<GoogleReCaptchaVerifyResponseDto>($"?secret={_googleReCaptchaSettings.SecretKey}&response={token}", cancellationToken);

        if (verifyResponse.Action != "login" || verifyResponse.Success != true || verifyResponse.Score < _googleReCaptchaSettings.MinScore)
        {
            throw new InvalidOperationException("Recaptcha verification failed.");
        }
    }
}
