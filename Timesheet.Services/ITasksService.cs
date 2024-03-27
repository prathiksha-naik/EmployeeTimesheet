using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee_timesheet.Models;
using Task = Employee_timesheet.Models.Task;
using Timesheet = Employee_timesheet.Models.Project;
namespace Employee_timesheet.Services
{
    public interface ITasksService
    {
        Task<IEnumerable<Task>> GetTaskDataAsync();
        Task<Task> GetTaskByTaskIdAndRelatedProjectAsync(string id);
        IEnumerable<Project> GetProjectDetails();
        Task<int> CreateTaskAsync(Task task);
        Task<int> UpdateTaskAsync(string id, Task task);
        Task<bool> DeleteTAskAsync(string id, Task task);
        string GetLatestIdFromTaskAsync();
        Task<Task> GetTaskByTaskIdAsync(string id);

    }
}
