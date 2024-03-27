using Employee_timesheet.Models;
using Employee_timesheet.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ProjectsService : IProjectsService
    {
        private readonly CompanyTimesheetContext _context;

        public ProjectsService(CompanyTimesheetContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Project>> GetProjectDataAsync()
        {
            return  _context.Projects.AsEnumerable();

        }

       


        public async Task<int> CreateProjectAsync(Project project)
        {
            _context.Add(project);
            return await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteProjectAsync(string id, Project project)
        {
            var hasAssociatedTasks = _context.Tasks.Any(t => t.ProjectId == id);
            if (!hasAssociatedTasks)
            {
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
            }
            return hasAssociatedTasks;
        }




        public async Task<int> UpdateProjectAsync(string id, Project project)
        {
            _context.Update(project);
            return await _context.SaveChangesAsync();
        }

        public string GetLatestIdFromTimesheetAsync()
        {
            return _context.Projects.Max(e => e.ProjectId);
        }

        public async Task<Project> GetProjectDetailsAsync(string id)
        {

            if (id == null)
            {
                return null;
            }
            var project = await _context.Projects
                           .FirstOrDefaultAsync(m => m.ProjectId == id);

            return project == null ? null : project; //ternary operator
        }

    }
}
