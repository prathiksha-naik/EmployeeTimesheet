using Employee_timesheet.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee_timesheet.Models;
using Task = Employee_timesheet.Models.Task;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class TasksService : ITasksService
    {
        private readonly CompanyTimesheetContext _context;
        public TasksService(CompanyTimesheetContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<Task>> GetTaskDataAsync()
        {
            return _context.Tasks.Include(t => t.Project).AsEnumerable();

        }
        public IEnumerable<Project> GetProjectDetails()
        {
            return _context.Projects.AsEnumerable();
        }

       
        public async Task<int> CreateTaskAsync(Task task)
        {
            _context.Add(task);
            return await _context.SaveChangesAsync();
        }
        
        public async Task<bool> DeleteTAskAsync(string id, Task task)
        {
            var hasAssociatedTasks = _context.Timesheets.Any(t => t.TaskId == id);
            if (!hasAssociatedTasks)
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
            }
            return hasAssociatedTasks;
        }

        public string GetLatestIdFromTaskAsync()
        {
            return _context.Tasks.Max(e => e.TaskId);
        }

        public async Task<int> UpdateTaskAsync(string id,Task task)
        {
            _context.Update(task);
           return await _context.SaveChangesAsync();
        }
        public async Task<Task> GetTaskByTaskIdAsync(string id)
        {
            return await _context.Tasks.FindAsync(id);

        }

        public async Task<Task> GetTaskByTaskIdAndRelatedProjectAsync(string id)
        {
            if (id == null)
            {
                return null;
            }
            var tasks = await _context.Tasks
               .Include(t => t.Project)
               .FirstOrDefaultAsync(m => m.TaskId == id);

            return tasks == null ? null : tasks; //ternary operator
        }



    }
}
