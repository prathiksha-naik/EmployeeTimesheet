using Employee_timesheet.Models;

namespace Employee_timesheet.Services
{
    public interface ITimesheetService
    {
        Task<IEnumerable<Models.Task>> GetTasksByProjectIdAsync(string projectId); 
        Task<IEnumerable<Timesheet>> GetTimesheetsForUserAsync();
        Task<Timesheet> GetTimesheetDetailsAsync(string id);
        int CreateTimesheetAsync(Timesheet timesheet);
        Task<int> DeleteTimesheetAsync(string id);
        Task<bool> UpdateTimesheetStatusAsync(string id, bool status);
        Task<int> UpdateTimesheetAsync(Timesheet timesheet);
        Task<bool> AddCommentToTimesheetAsync(string id, string comment);
        string GetLatestIdFromTimesheetAsync();
        Task<Timesheet> GetTimesheetAsync(string id);
        Task<TimesheetData> GetTimesheetDataAsync(); 
    }
}
