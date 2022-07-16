using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppService.Dtos.Accounts
{
    public class AccountUpdatingDto
    {
        public Guid AccountId { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
