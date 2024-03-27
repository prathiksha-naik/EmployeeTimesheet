using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Employee_timesheet.Models;

public partial class CompanyTimesheetContext : DbContext
{
    public CompanyTimesheetContext()
    {
    }

    public CompanyTimesheetContext(DbContextOptions<CompanyTimesheetContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Manager> Managers { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<Timesheet> Timesheets { get; set; }

    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
    //        => optionsBuilder.UseSqlServer("Server=ELW5196;Database=Company_timesheet;Trusted_Connection=True;TrustServerCertificate=True;");

    //public  int CreateTimesheet(string timesheetId, string employeeId, string taskId, DateTime? date, string? type, decimal? totalHoursWorked,  string? projectId)
    //{
    //    var timesheetIdParam = new SqlParameter("@TimesheetId", timesheetId);
    //    var employeeIdParam = new SqlParameter("@EmployeeId", employeeId);
    //    var taskIdParam = new SqlParameter("@TaskId", taskId);
    //    var dateParam = new SqlParameter("@Date", date);
    //    var typeParam = new SqlParameter("@Type", type);
    //    var totalHoursWorkedParam = new SqlParameter("@TotalHoursWorked", totalHoursWorked);
    //    var projectIdParam = new SqlParameter("@ProjectId", projectId);

    //    return Database.ExecuteSqlRaw("EXEC CreateTimesheet @TimesheetId, @EmployeeId, @TaskId, @Date, @Type, @TotalHoursWorked, @ProjectId",
    //        timesheetIdParam, employeeIdParam, taskIdParam, dateParam, typeParam, totalHoursWorkedParam, projectIdParam);
    //}

    //public  int EditTimesheet(string timesheetId, string employeeId, string taskId, DateTime? date, string? type, decimal? totalHoursWorked,  string? projectId)
    //{
    //    var timesheetIdParam = new SqlParameter("@TimesheetId", timesheetId);
    //    var employeeIdParam = new SqlParameter("@EmployeeId", employeeId);
    //    var taskIdParam = new SqlParameter("@TaskId", taskId);
    //    var dateParam = new SqlParameter("@Date", date);
    //    var typeParam = new SqlParameter("@Type", type);
    //    var totalHoursWorkedParam = new SqlParameter("@TotalHoursWorked", totalHoursWorked);
    //    var projectIdParam = new SqlParameter("@ProjectId", projectId);

    //    return Database.ExecuteSqlRaw("EXEC EditTimesheet @TimesheetId, @EmployeeId, @TaskId, @Date, @Type, @TotalHoursWorked, @ProjectId",
    //        timesheetIdParam, employeeIdParam, taskIdParam, dateParam, typeParam, totalHoursWorkedParam, projectIdParam);
    //}

    public int UpdateOrInsertTimesheet(string timesheetId, string employeeId, string taskId, DateTime? date, string? type, decimal? totalHoursWorked, string? projectId)
    {
        var timesheetIdParam = new SqlParameter("@TimesheetId", timesheetId);
        var employeeIdParam = new SqlParameter("@EmployeeId", employeeId);
        var taskIdParam = new SqlParameter("@TaskId", taskId);
        var dateParam = new SqlParameter("@Date", date);
        var typeParam = new SqlParameter("@Type", type);
        var totalHoursWorkedParam = new SqlParameter("@TotalHoursWorked", totalHoursWorked);
        var projectIdParam = new SqlParameter("@ProjectId", projectId);

        return Database.ExecuteSqlRaw("EXEC UpdateOrInsertTimesheet @TimesheetId, @EmployeeId, @TaskId, @Date, @Type, @TotalHoursWorked, @ProjectId",
            timesheetIdParam, employeeIdParam, taskIdParam, dateParam, typeParam, totalHoursWorkedParam, projectIdParam);
    }

    public int DeleteTimesheet(string timesheetId)
    {
        var timesheetIdParam = new SqlParameter("@TimesheetId", timesheetId);
        return Database.ExecuteSqlRaw("EXEC DeleteTimesheet @TimesheetId", timesheetIdParam);
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__EMPLOYEE__781228D9A75B6D08");

            entity.ToTable("EMPLOYEES");

            entity.Property(e => e.EmployeeId)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasDefaultValueSql("('EMP'+format(NEXT VALUE FOR [seq_Employees],'000'))")
                .HasColumnName("Employee_id");
            entity.Property(e => e.DateOfJoining)
                .HasColumnType("date")
                .HasColumnName("Date_Of_Joining");
            entity.Property(e => e.Email)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("Employee_Name");
            entity.Property(e => e.ManagerId)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("Manager_Id");
            entity.Property(e => e.Password)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("Phone_Number");
            entity.Property(e => e.Position)
                .HasMaxLength(25)
                .IsUnicode(false);

            entity.HasOne(d => d.Manager).WithMany(p => p.Employees)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("FK__EMPLOYEES__Manag__3E52440B");
        });

        modelBuilder.Entity<Manager>(entity =>
        {
            entity.HasKey(e => e.ManagerId).HasName("PK__MANAGERS__AE5FEFAD76C41B47");

            entity.ToTable("MANAGERS");

            entity.Property(e => e.ManagerId)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasDefaultValueSql("('MGR'+format(NEXT VALUE FOR [seq_Managers],'000'))")
                .HasColumnName("Manager_Id");
            entity.Property(e => e.Email)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.ManagerName)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("Manager_Name");
            entity.Property(e => e.Password)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("Phone_Number");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.ProjectId).HasName("PK__PROJECTS__1CB92E03B6C39249");

            entity.ToTable("PROJECTS");

            entity.Property(e => e.ProjectId)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasDefaultValueSql("('PRO'+format(NEXT VALUE FOR [seq_Project],'000'))")
                .HasColumnName("Project_Id");
            entity.Property(e => e.Description)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.ProjectName)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("Project_name");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.TaskId).HasName("PK__TASKS__716846B501707EC0");

            entity.ToTable("TASKS");

            entity.Property(e => e.TaskId)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasDefaultValueSql("('TSK'+format(NEXT VALUE FOR [seq_Task],'000'))")
                .HasColumnName("Task_id");
            entity.Property(e => e.ProjectId)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("Project_id");
            entity.Property(e => e.TaskName)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("Task_name");

            entity.HasOne(d => d.Project).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("FK__TASKS__Project_i__48CFD27E");
        });

        modelBuilder.Entity<Timesheet>(entity =>
        {
            entity.HasKey(e => e.TimesheetId).HasName("PK__TIMESHEE__9E5234109B5F78DD");

            entity.ToTable("TIMESHEETS");

            entity.Property(e => e.TimesheetId)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasDefaultValueSql("('TST'+format(NEXT VALUE FOR [seq_Timesheets],'000'))")
                .HasColumnName("Timesheet_Id");
            entity.Property(e => e.Comments)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.EmployeeId)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("Employee_id");
            entity.Property(e => e.ProjectId)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("Project_id");
            entity.Property(e => e.TaskId)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("Task_Id");
            entity.Property(e => e.TotalHoursWorked)
                .HasColumnType("decimal(6, 3)")
                .HasColumnName("Total_Hours_Worked");
            entity.Property(e => e.Type)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.Employee).WithMany(p => p.Timesheets)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TIMESHEET__Emplo__4E88ABD4");

            entity.HasOne(d => d.Project).WithMany(p => p.Timesheets)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("FK__TIMESHEET__Proje__60A75C0F");

            entity.HasOne(d => d.Task).WithMany(p => p.Timesheets)
                .HasForeignKey(d => d.TaskId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TIMESHEET__Task___4F7CD00D");
        });
        modelBuilder.HasSequence("Seq_Employees").StartsAt(0L);
        modelBuilder.HasSequence("Seq_Managers").StartsAt(0L);
        modelBuilder.HasSequence("Seq_Project").StartsAt(0L);
        modelBuilder.HasSequence("Seq_Task").StartsAt(0L);
        modelBuilder.HasSequence("Seq_Timesheets").StartsAt(0L);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
