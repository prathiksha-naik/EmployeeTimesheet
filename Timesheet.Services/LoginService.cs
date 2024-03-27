using Employee_timesheet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class LoginService : ILoginService
    {
        private readonly CompanyTimesheetContext _context;
        public LoginService(CompanyTimesheetContext context)
        {
            _context = context;
        }
        bool ILoginService.IsValidEmployeeLogin(string username, string password)
        {
            var employee = _context.Employees
                   .FirstOrDefault(e => e.EmployeeId == username && e.Password == password);
            if (employee != null)
            {
                return true;
            }
            return false;
        }

        bool ILoginService.IsValidManagerLogin(string username, string password)
        {
            var manager = _context.Managers
                     .FirstOrDefault(m => m.ManagerId == username && m.Password == password);
            if (manager != null)
            {
                return true;
            }
            return false;
        }
    }
}
