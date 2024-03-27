using System;
using System.Collections.Generic;

namespace Employee_timesheet.Models;

public partial class Task
{
    public string TaskId { get; set; } = null!;

    public string TaskName { get; set; } = null!;

    public string? ProjectId { get; set; }

    public virtual Project? Project { get; set; }

    public virtual ICollection<Timesheet> Timesheets { get; set; } = new List<Timesheet>();
}
