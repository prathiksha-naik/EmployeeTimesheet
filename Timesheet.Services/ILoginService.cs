using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface ILoginService
    {
        bool IsValidEmployeeLogin(string username, string password);
        bool IsValidManagerLogin(string username, string password);
    }
}
