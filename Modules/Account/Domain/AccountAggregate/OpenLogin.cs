using Haskap.EShopping.Domain;
using Modules.Account.Application.Dtos.Account;
using UAParser;

namespace Modules.Account.Domain.AccountAggregate;

public class OpenLogin : Entity
{
    public Guid AccountId { get; set; }
    public DateTime UtcLoginDateTime { get; private set; }
    public DateTime UtcLastSeenDateTime { get; private set; }
    public string Platform { get; private set; }
    public string Browser { get; private set; }
    public string? RemoteIpAddress { get; private set; }

    private OpenLogin()
    {
        
    }

    public OpenLogin(Guid id, string? remoteIpAddress, string userAgentString)
        : base(id)
    {
        var clientInfo = Parser.GetDefault().Parse(userAgentString);

        UtcLoginDateTime = DateTime.UtcNow;
        SetLastSeenDateTime();
        Platform = clientInfo.Device.Family + " - " + clientInfo.OS.Family;
        Browser = clientInfo.UA.Family;

        RemoteIpAddress = remoteIpAddress;
    }

    public void SetLastSeenDateTime()
    {
        UtcLastSeenDateTime = DateTime.UtcNow;
    }

    public OpenLoginOutputDto ToOpenLoginOutputDto()
    {
        return new()
        {
            AccountId = AccountId,
            Id = Id,
            UtcLoginDateTime = UtcLoginDateTime,
            UtcLastSeenDateTime = UtcLastSeenDateTime,
            Platform = Platform,
            Browser = Browser,
            RemoteIpAddress = RemoteIpAddress
        };
    }
}
