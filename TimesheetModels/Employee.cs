using System;
using System.Collections.Generic;

namespace Employee_timesheet.Models;

public partial class Employee
{
    public string EmployeeId { get; set; } = null!;

    public string EmployeeName { get; set; } = null!;

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string Position { get; set; } = null!;

    public DateTime? DateOfJoining { get; set; }

    public string? ManagerId { get; set; }

    public string? Password { get; set; }

    public virtual Manager? Manager { get; set; }

    public virtual ICollection<Timesheet> Timesheets { get; set; } = new List<Timesheet>();
}
