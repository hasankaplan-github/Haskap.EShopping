using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Account.Application.Dtos.Account;

public class OpenLoginOutputDto
{
    public Guid AccountId { get; set; }
    public Guid Id { get; set; }
    public DateTime UtcLoginDateTime { get; set; }
    public DateTime UtcLastSeenDateTime { get; set; }
    public string Platform { get; set; }
    public string Browser { get; set; }
    public string? RemoteIpAddress { get; set; }
}
