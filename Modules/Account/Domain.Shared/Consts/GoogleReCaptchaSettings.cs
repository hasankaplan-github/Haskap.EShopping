namespace Modules.Account.Domain.Shared.Consts;

public class GoogleReCaptchaSettings
{
    public const string SectionName = "GoogleReCaptchaSettings";

    public string SiteKey { get; init; }
    public string SecretKey { get; init; }
    public double MinScore { get; init; }
}
