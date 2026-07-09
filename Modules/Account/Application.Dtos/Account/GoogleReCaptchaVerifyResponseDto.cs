using System.Text.Json.Serialization;

namespace Modules.Account.Application.Dtos.Account;
public class GoogleReCaptchaVerifyResponseDto
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }
    [JsonPropertyName("score")]
    public double Score { get; set; }
    [JsonPropertyName("action")]
    public string Action { get; set; }
    [JsonPropertyName("challenge_ts")]
    public DateTime ChallengeTs { get; set; }
    [JsonPropertyName("hostname")]
    public string Hostname { get; set; }
    [JsonPropertyName("error-codes")]
    public IList<string>? ErrorCodes { get; set; }
}
