using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee_timesheet.Models;

namespace Employee_timesheet.Services
{
   public interface IProjectsService
    {
        Task<IEnumerable<Project>> GetProjectDataAsync();
        Task<Project> GetProjectDetailsAsync(string id);
        Task<int>CreateProjectAsync(Project project);
        Task<int>UpdateProjectAsync(string id, Project project);
        Task<bool>DeleteProjectAsync(string id, Project project);
        string GetLatestIdFromTimesheetAsync();
    }
}
