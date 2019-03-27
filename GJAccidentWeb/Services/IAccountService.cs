using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GJAccidentWeb.Entity;

namespace GJAccidentWeb.Services
{
    public interface IAccountService
    {
        UserInfo getUserInfoById(string id);
        UserInfo getUserInfoByName(string Name);
    }
}
