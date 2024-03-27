using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Employee_timesheet.Models;
using System.Reflection.Metadata;
using NToastNotify;
using Employee_timesheet.Services;
using Services;
using System.Runtime.Caching;

namespace Employee_timesheet.Controllers;

public class ProjectsController : Controller
{

    private readonly IToastNotification _toastNotification;
    private readonly IProjectsService _projectsService;

    private const string PROJECT_DATA_CACHE = "project_data_cache";

    public ProjectsController(IToastNotification toastNotification, IProjectsService projectsService)
    {
        _toastNotification = toastNotification;
        _projectsService = projectsService;
    }

    private async Task<IEnumerable<Project>> getProjectDetails()
    {
        var projectDataCache = MemoryCache.Default[PROJECT_DATA_CACHE] as IEnumerable<Project>;
        if (projectDataCache == null)
        {
            Console.WriteLine("projectDataCache is null. Fetching data from database...");
            projectDataCache = await _projectsService.GetProjectDataAsync();
            MemoryCache.Default[PROJECT_DATA_CACHE] = projectDataCache.ToList();
        }
        return projectDataCache;
    }

    // GET: Projects
    public async Task<IActionResult> Index()
    {
        try
        {
            var projects = await getProjectDetails();
            if (projects != null)
            {
                return View(projects);
            }
            else
            {
                return Problem("Entity set 'CompanyTimesheetContext.Projects'  is null.");
            }

        }
        catch (Exception ex)
        {
            // Handle the exception and display an error message to the user
            return Problem("An error occurred while fetching index of the project: ");
        }



    }

    // GET: Projects/Details/5
    public async Task<IActionResult> Details(string id)
    {

        try
        {
            IEnumerable<Project> allProjectDetails = await getProjectDetails();
            var project = allProjectDetails.FirstOrDefault(project => project.ProjectId == id);

            return View(project);
        }
        catch (Exception ex)
        {
            // Handle the exception and display an error message to the user
            return Problem("An error occurred while fetching details of the project: ");
        }


    }

    // GET: Projects/Create
    public IActionResult Create()
    {
        return View();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ProjectId,ProjectName,Description")] Project project)
    {
        try
        {
            MemoryCache.Default.Remove(PROJECT_DATA_CACHE);
            string lastProjectId = _projectsService.GetLatestIdFromTimesheetAsync();//Fetching last project id from project table
            int nextProNumber = int.Parse(lastProjectId.Substring(3)) + 1; //incrementing numeric part of project id to 1
            project.ProjectId = "PRO" + nextProNumber.ToString("000"); //concatinating incremented number with PRO and assigning to next project id

            if (ModelState.IsValid)
            {
                await _projectsService.CreateProjectAsync(project);
                return Json(Url.Action("Index", "Projects"));

            }
            return View(project);
        }
        catch (Exception ex)
        {
            // Handle the exception and display an error message to the user
            return Problem("An error occurred while creating the project: ");
        }

    }

    // GET: Projects/Edit/5
    public async Task<IActionResult> Edit(string id)
    {
        try
        {
            var allProjectDetails = await getProjectDetails();
            var project = allProjectDetails.FirstOrDefault(project => project.ProjectId == id);
            //var project = await _projectsService.GetProjectDetailsAsync(id);
            if (project == null)
            {

                return Problem("No record found");
            }
            return View(project);
        }
        catch (Exception ex)
        {
            // Handle the exception and display an error message to the user
            return Problem("An error occurred while editing the project: ");
        }

    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, [Bind("ProjectId,ProjectName,Description")] Project project)
    {
        if (id != project.ProjectId)
        {
            return Problem("No record found");
        }


        if (ModelState.IsValid)
        {
            try
            {
                await _projectsService.UpdateProjectAsync(id, project);
            }
            catch (DbUpdateException ex)
            {
                // Handle databaserelated issues example unique constraint violation
                return Problem("A database error occurred while creating the timesheet.");
            }
            catch (Exception ex)
            {
                // Handle the exception and display an error message to the user
                return Problem("An error occurred while updating the timesheet ");
            }
            MemoryCache.Default.Remove(PROJECT_DATA_CACHE);
            return RedirectToAction(nameof(Index));
        }
        return View(project);
    }


    // GET: Projects/Delete/5
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            var allProjectDetails = await getProjectDetails();
            var project = allProjectDetails.FirstOrDefault(project => project.ProjectId == id);
            if (project == null)
            {
                return Problem("No record found");
            }


            return View(project);
        }
        catch (Exception ex)
        {
            // Handle the exception and display an error message to the user
            return Problem("An error occurred while fetching details of the project: " + ex.Message);
        }
    }

    // POST: Projects/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        Project project = null;
        try
        {
            var allProjectDetails = await getProjectDetails();
            project = allProjectDetails.FirstOrDefault(project => project.ProjectId == id);

            if (project == null)
            {
                return BadRequest();
            }
            // Check if the project has associated tasks
            var hasAssociatedTasks = await _projectsService.DeleteProjectAsync(id, project);

            if (hasAssociatedTasks)
            {
                TempData["ErrorMessage"] = "Cannot delete the project because it contains associated tasks.";
                return RedirectToAction(nameof(Index));
            }

            _toastNotification.AddSuccessToastMessage("Project deleted successfully");
            MemoryCache.Default.Remove(PROJECT_DATA_CACHE);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            // Handle the exception and display an error message to the user
            string errorMessage = "An error occurred while deleting the project: " + ex.Message;
            ViewBag.Script = $"<script>alert('{errorMessage}');</script>";
            return View(project);
        }
    }
}
