using Employee_timesheet.Models;
using Task = Employee_timesheet.Models.Task;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Employee_timesheet.Services
{
    public class TimesheetsService : ITimesheetService
    {
        
        private readonly CompanyTimesheetContext _context;
        


        public TimesheetsService(CompanyTimesheetContext context)
        {
            _context = context;
            
        }


        public async Task<IEnumerable<Timesheet>> GetTimesheetsForUserAsync()
        {
            var companyTimesheetContext = _context.Timesheets
                                                .Include(t => t.Employee)
                                                .Include(t => t.Task)
                                                .Include(t => t.Project);
            return companyTimesheetContext.AsEnumerable();
        }

        public async Task<IEnumerable<Task>> GetTasksByProjectIdAsync(string projectId)
        {
            return _context.Tasks.Where(t => t.ProjectId == projectId).OrderBy(t => t.TaskName).AsEnumerable();
        }
        public async Task<bool> AddCommentToTimesheetAsync(string id, string comment)
        {
            var timesheet = await _context.Timesheets.FindAsync(id);
            if (timesheet != null)
            {
                timesheet.Comments = comment;
                await _context.SaveChangesAsync();
                return true;

            }
            return false;
        }

        //public async Task<int> CreateTimesheetAsync(Timesheet timesheet)
        //{
        //    _context.Add(timesheet);
        //    return await _context.SaveChangesAsync();
        //}
        public int CreateTimesheetAsync(Timesheet timesheet)
        {
            var result = _context.UpdateOrInsertTimesheet(
                timesheet.TimesheetId,
                timesheet.EmployeeId,
                timesheet.TaskId,
                timesheet.Date,
                timesheet.Type,
                timesheet.TotalHoursWorked,
                timesheet.ProjectId
            );

            return result;
        }


        //public async Task<Timesheet> DeleteTimesheetAsync(string id)
        //{
        //    var timesheet = await _context.Timesheets
        //             .Include(t => t.Employee)
        //             .Include(t => t.Task)
        //             .Include(t => t.Project)
        //             .FirstOrDefaultAsync(m => m.TimesheetId == id);

        //    if (timesheet != null)
        //    {
        //        _context.Timesheets.Remove(timesheet);
        //        await _context.SaveChangesAsync();
        //    }

        //    return timesheet;
        //}
        public async Task<int> DeleteTimesheetAsync(string id)
        {
            // Call the method in your database context to delete the timesheet
            int result = _context.DeleteTimesheet(id);

            return result;
        }



        public async Task<Timesheet> GetTimesheetAsync(string id)
        {
            if (id == null)
            {
                return null;
            }
            var timesheet = await _context.Timesheets.FindAsync(id);
            return timesheet;
        }

        public string GetLatestIdFromTimesheetAsync()
        {
            return _context.Timesheets.Max(timesheet => timesheet.TimesheetId);
        }

        public async Task<TimesheetData> GetTimesheetDataAsync()
        {
          var EmployeesData = await _context.Employees.ToListAsync();
          var ProjectData = await _context.Projects.ToListAsync();
          var TaskData = await _context.Tasks.ToListAsync();

            return new TimesheetData
            {
                Employees = EmployeesData,
                Projects = ProjectData,
                Tasks = TaskData
            };
        }


        //public async Task<int> UpdateTimesheetAsync(Timesheet timesheet)
        //{
        //        _context.Update(timesheet);
        //        return await _context.SaveChangesAsync();   
        //}
        public async Task<int> UpdateTimesheetAsync(Timesheet timesheet)
        {
            return _context.UpdateOrInsertTimesheet(
                timesheet.TimesheetId,
                timesheet.EmployeeId,
                timesheet.TaskId,
                timesheet.Date,
                timesheet.Type,
                timesheet.TotalHoursWorked,
                timesheet.ProjectId
                
            );
        }

        public async Task<bool> UpdateTimesheetStatusAsync(string id, bool status)
        {
            var timesheet = await _context.Timesheets.FindAsync(id);

            if (timesheet == null)
            {
                return false;
            }
            timesheet.Status = status;
            int saveChangesResult = await _context.SaveChangesAsync(); // Use async version of SaveChanges

            return saveChangesResult > 0;
        }
        public async Task<Timesheet> GetTimesheetDetailsAsync(string id)
        {
            Timesheet timesheet = await _context.Timesheets
                    .Include(t => t.Employee)
                    .Include(t => t.Task)
                    .Include(t => t.Project)
                    .FirstOrDefaultAsync(m => m.TimesheetId == id);
            return timesheet;
        }
    }
}
