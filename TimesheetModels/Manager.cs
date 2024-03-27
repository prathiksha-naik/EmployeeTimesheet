using System;
using System.Collections.Generic;

namespace Employee_timesheet.Models;

public partial class Manager
{
    public string ManagerId { get; set; } = null!;

    public string ManagerName { get; set; } = null!;

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Password { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
