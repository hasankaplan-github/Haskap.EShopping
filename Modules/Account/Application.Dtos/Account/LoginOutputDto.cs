using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Account.Application.Dtos.Account;
public class LoginOutputDto
{
    public Guid AccountId { get; set; }
    public string UserFirstName { get; set; }
    public string UserLastName { get; set; }
    public Guid LoginId { get; set; }
}
