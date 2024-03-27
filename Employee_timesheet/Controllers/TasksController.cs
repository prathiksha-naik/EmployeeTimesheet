using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Employee_timesheet.Models;
using Task = Employee_timesheet.Models.Task;
using Employee_timesheet.Services;
using Services;
using Microsoft.Extensions.Caching.Memory;

namespace Employee_timesheet.Controllers
{
    public class TasksController : Controller
    {
        private readonly CompanyTimesheetContext _context;
        private readonly ITasksService _tasksService;
        private readonly IMemoryCache _memoryCache;

        private const string TASK_DATA_CACHE = "task_data_cache";

        public TasksController(CompanyTimesheetContext context, ITasksService tasksService, IMemoryCache memoryCache)
        {
            _context = context;
            _tasksService = tasksService;
            _memoryCache = memoryCache;
        }

    

    private async Task<IEnumerable<Task>> GetTaskDetails()
        {
            if (!_memoryCache.TryGetValue(TASK_DATA_CACHE, out IEnumerable<Task> taskDataCache))
            {
                taskDataCache = await _tasksService.GetTaskDataAsync();
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30) // Set your desired cache expiration time
                };
                taskDataCache = taskDataCache.ToList();
                _memoryCache.Set(TASK_DATA_CACHE, taskDataCache, cacheEntryOptions);
            }
            return taskDataCache;
        }

        // GET: Tasks
        public async Task<IActionResult> Index()
        {
            var tasks = await GetTaskDetails();
            return View(tasks);
        }

        // GET: Tasks/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                throw new Exception("ID is null"); 
            }
            var taskdetail = await GetTaskDetails();
            var task = taskdetail.FirstOrDefault(task => task.TaskId == id);

            if (task == null)
            {
                throw new Exception(id);
            }

            return View(task);
        }

        // Other actions without try-catch

        // GET: Tasks/Create
        public IActionResult Create()
        {
            var projectData = _tasksService.GetProjectDetails();
            ViewData["ProjectId"] = new SelectList(projectData, "ProjectId", "ProjectId");
            ViewBag.Project = projectData.ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TaskId,TaskName,ProjectId")] Task task)
        {
            string lastTaskId = _tasksService.GetLatestIdFromTaskAsync(); // Fetching last employee id from employee table
            int nextTaskNumber = int.Parse(lastTaskId.Substring(3)) + 1; // Incrementing the numeric part of the employee id by 1
            task.TaskId = "TSK" + nextTaskNumber.ToString("000"); // Concatenating incremented number with TSK and assigning to the next task id

            if (ModelState.IsValid)
            {
                await _tasksService.CreateTaskAsync(task);
                return RedirectToAction(nameof(Index));
            }
            var projects = _tasksService.GetProjectDetails();
            ViewData["ProjectId"] = new SelectList(projects, "ProjectId", "ProjectId", task.ProjectId);
            ViewBag.Project = projects.ToList();

            return View(task);
        }

        // GET: Tasks/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Tasks == null)
            {
                throw new Exception("No record found");
            }

            var task = _tasksService.GetTaskByTaskIdAsync(id).Result;

            if (task == null)
            {
                throw new Exception("No record found");
            }
            var projects = _tasksService.GetProjectDetails();
            ViewData["ProjectId"] = new SelectList(projects, "ProjectId", "ProjectId", task.ProjectId);
            ViewBag.Project = projects.ToList();
            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("TaskId,TaskName,ProjectId")] Task task)
        {
            if (id != task.TaskId)
            {
                throw new Exception("No record found");
            }

            if (ModelState.IsValid)
            {
                await _tasksService.UpdateTaskAsync(id, task);
                return RedirectToAction(nameof(Index));
            }
            var projects = _tasksService.GetProjectDetails();
            ViewData["ProjectId"] = new SelectList(projects, "ProjectId", "ProjectId", task.ProjectId);
            return View(task);
        }

        // GET: Tasks/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            var task = await _tasksService.GetTaskByTaskIdAndRelatedProjectAsync(id);

            if (task == null)
            {
               throw new Exception("No record found");
            }

            return View(task);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (id == null)
            {
                throw new Exception("Id is null");
            }
            Task task = await _tasksService.GetTaskByTaskIdAsync(id);

            if (task == null)
            {
                throw new Exception("No record found");
            }
            var hasAssociatedTasks = await _tasksService.DeleteTAskAsync(id, task);

            if (hasAssociatedTasks)
            {
                TempData["ErrorMessage"] = "Cannot delete the task because it contains associated timesheet.";
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
