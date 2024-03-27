using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Employee_timesheet.Models;
using Timesheet = Employee_timesheet.Models.Timesheet;
using Task = Employee_timesheet.Models.Task;
using Microsoft.AspNetCore.Authorization;
using NToastNotify;
using Employee_timesheet.Services;
using Microsoft.CodeAnalysis;
using Microsoft.Build.Evaluation;


namespace Employee_timesheet.Controllers
{
    public class TimesheetsController : Controller
    {
        private readonly IToastNotification _toastNotification;
        private readonly ITimesheetService _timesheetService;
        private readonly ILogger<TimesheetsController> _logger;


        public TimesheetsController(IToastNotification toastNotification, ITimesheetService timesheetService, ILogger<TimesheetsController> logger)
        {
            _toastNotification = toastNotification;
            _timesheetService = timesheetService;
            _logger = logger;
        }

        public class TimesheetNotFoundException : Exception
        {
            public string TimesheetId  { get; }
            public TimesheetNotFoundException(string id)
                : base(id)
            {
                TimesheetId = id;
            }
        }


        public async Task<IActionResult> GetTasksByProjectId(string projectId)
        {
            if (string.IsNullOrEmpty(projectId))
            {
                return BadRequest("projectId cannot be null or empty.");
            }

            try
            {
                var tasks = await _timesheetService.GetTasksByProjectIdAsync(projectId);
                return Json(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError( "An unexpected error occurred while fetching details", ex.Message);
                return Problem("An unexpected error occurred while fetching  details.");
            }
        }



        // GET: Timesheets for Employee Login
        public async Task<IActionResult> EmployeeIndex()
        {
            try
            {
                var employee = await _timesheetService.GetTimesheetsForUserAsync();
                return View(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while fetching the Timesheets", ex.Message);

                return Problem("An error occurred while fetching the Timesheets ");
            }
        }



        // GET: Timesheets for Manager Login
        public async Task<IActionResult> ManagerIndex()
        {
            try
            {
                var manager = await _timesheetService.GetTimesheetsForUserAsync();
                return View(manager);
            }
            catch (Exception ex)
            {
                _logger.LogError( "An error occurred while fetching the Timesheets", ex.Message);
                return Problem("An error occurred while fetching the Timesheets ");
            }
        }



        public async Task<IActionResult> Approval()
        {
            try
            {
                var approval = await _timesheetService.GetTimesheetsForUserAsync();
                return View(approval);
            }
            catch (Exception ex)
            {
                _logger.LogError( "An error occurred while fetching the Timesheets", ex.Message);
                return Problem("An error occurred while fetching the Timesheets ");
            }
        }


        // GET: Timesheets/Details/
        public async Task<IActionResult> Details(string id)
        {

            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Id cannot be null or empty.");
            }
            try
            {
                var timesheet = await _timesheetService.GetTimesheetDetailsAsync(id);
                if (timesheet == null)
                {
                    throw new TimesheetNotFoundException(id);
                }

                return View(timesheet);
            }
            catch (TimesheetNotFoundException ex)
            {
                _logger.LogError("Timesheet not found: {message}", ex.Message);
                return NotFound($"Timesheet with ID {ex.TimesheetId} was not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while fetching the details of the timesheet", ex.Message);
                return Problem("An error occurred while fetching the details of the timesheet ");
            }
        }
        [Authorize]

        // GET: Timesheets/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                var timesheetData = await _timesheetService.GetTimesheetDataAsync();
                if (timesheetData == null)
                {
                    return View( "An error occurred while retrieving timesheet data.");
                }
                ViewBag.Projects = timesheetData.Projects.ToList();
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while processing your request", ex.Message);
                return View( "An error occurred while processing your request.");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("TimesheetId,EmployeeId,ProjectId,TaskId,Date,Type,TotalHoursWorked")] Timesheet timesheet)
        {
            try
            {
                if (timesheet == null)
                {
                    return BadRequest();
                }

                string lastTimesheetId = _timesheetService.GetLatestIdFromTimesheetAsync();    //Fetching last timesheet id from timesheet table

                if (lastTimesheetId == null)
                {
                    lastTimesheetId = "TST000";
                }
                int nextTstNumber = int.Parse(lastTimesheetId.Substring(3)) + 1;        //incrementing numeric part of timesheet id to 1
                timesheet.TimesheetId = "TST" + nextTstNumber.ToString("000");
             
                if (ModelState.IsValid)
                {
                    var temp = _timesheetService.CreateTimesheetAsync(timesheet);
                    _toastNotification.AddSuccessToastMessage("Timesheet Submitted Successfully");
                    return Json(Url.Action("EmployeeIndex", "Timesheets"));
                    
                }
                
                return View(timesheet);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError("A database error occurred while creating the timesheet: {message}", ex.Message);
                return Json(new { success = false, error = "A database error occurred while creating the timesheet." });
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while creating the timesheet", ex.Message);
                return Json(new { success = false, error = "An error occurred while creating the timesheet: " });
            }
        }

      


        // GET: Timesheets/Edit/
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Id cannot be null or empty.");
            }
            try
            {
                var timesheet = await _timesheetService.GetTimesheetAsync(id);
                if (timesheet == null)
                {
                    throw new TimesheetNotFoundException(id);
                }
                var data = await _timesheetService.GetTimesheetDataAsync();
                ViewBag.Projects = data.Projects.ToList();
                return View(timesheet);
            }
            catch (TimesheetNotFoundException ex)
            {
                _logger.LogError("Timesheet not found: {message}", ex.Message);
                return NotFound($"Timesheet with ID {ex.TimesheetId} was not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while updating the timesheet", ex.Message);
                return Problem("An error occurred while updating the timesheet ");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("TimesheetId,EmployeeId,ProjectId,TaskId,Date,Type,TotalHoursWorked")] Timesheet timesheet)
        {
            timesheet.Status = null;
            if (timesheet == null && id == null)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _timesheetService.UpdateTimesheetAsync(timesheet);
                    _toastNotification.AddWarningToastMessage("Timesheet Updated Successfully");

                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex ,"A database error occurred while updating the timesheet: {message}", ex.Message);
                    return Problem("A database error occurred while creating the timesheet." );
                }
                catch (Exception ex)
                {
                    _logger.LogError("An error occurred while updating the timesheet", ex.Message);
                    return Problem("An error occurred while updating the timesheet ");
                }
                return RedirectToAction(nameof(EmployeeIndex));
            }
            return View(timesheet);
        }



        // GET: Timesheets/Delete/
        public async Task<IActionResult> Delete(string id)
        {

            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Id cannot be null or empty.");
            }
            try
            {
                var timesheet = await _timesheetService.GetTimesheetDetailsAsync(id);
                if (timesheet == null)
                {
                    // Throw your custom exception here
                    throw new TimesheetNotFoundException(id);
                }

                return View(timesheet);
            }
            catch (TimesheetNotFoundException ex)
            {
                _logger.LogError( "Timesheet not found : {message}", ex.Message);
                return NotFound($"Timesheet with ID {ex.TimesheetId} was not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError( "An error occurred while deleting the timesheet", ex.Message);
                return Problem("An error occurred while deleting the timesheet ");
            }

        }



        // POST: Timesheets/Delete/
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Id cannot be null or empty.");
            }

            try
            {
                var timesheet =  _timesheetService.DeleteTimesheetAsync(id);
                if (timesheet != null)
                {
                    _toastNotification.AddErrorToastMessage("Deleted Successfully");
                    return RedirectToAction(nameof(EmployeeIndex));

                }
                else
                {
                    throw new TimesheetNotFoundException(id);
                }


            }
            catch (TimesheetNotFoundException ex)
            {
                _logger.LogError("Timesheet not found: {message}", ex.Message);
                return NotFound($"Timesheet with ID {ex.TimesheetId} was not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while deleting the timesheet", ex.Message);
                return Problem("An error occurred while deleting the timesheet ");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(string id, bool status)
        {
            if (id == null )
            {
                return NotFound();
            }
            try
            {
                var timesheet = await _timesheetService.UpdateTimesheetStatusAsync(id, status);
                if (timesheet == true)
                {
                    return RedirectToAction(nameof(Approval));
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError( "An error occurred while Updating status the timesheet");
                return Problem("An error occurred while Approving timesheet ");
            }
        }



        [HttpPost]
        public async Task<IActionResult> AddComment(string id, string comment)
        {
            try
            {
                var timesheet = await _timesheetService.AddCommentToTimesheetAsync(id, comment);

                if (timesheet == false)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(ManagerIndex));
            }
            catch (Exception ex)
            {
                _logger.LogError( "An error occurred while Commenting the timesheet");
                return Problem("An error occurred while Commenting the timesheet");
            }
        }

    }
}