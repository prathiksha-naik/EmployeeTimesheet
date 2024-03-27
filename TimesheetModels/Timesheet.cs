using System;
using System.Collections.Generic;

namespace Employee_timesheet.Models;

public partial class Timesheet
{
    public string TimesheetId { get; set; } = null!;

    public string EmployeeId { get; set; } = null!;

    public string TaskId { get; set; } = null!;

    public string? Type { get; set; }

    public decimal? TotalHoursWorked { get; set; }

    public bool? Status { get; set; }

    public string? ProjectId { get; set; }

    public string? Comments { get; set; }

    public DateTime? Date { get; set; }

    public virtual Employee? Employee { get; set; } = null!;

    public virtual Project? Project { get; set; }

    public virtual Task? Task { get; set; } = null!;
}
