namespace Employee_timesheet.Models;
public class TimesheetData
{
    public List<Employee> Employees { get; set; }
    public List<Task> Tasks { get; set; }
    public List<Project> Projects { get; set; }
}