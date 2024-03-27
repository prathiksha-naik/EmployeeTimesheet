using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Employee_timesheet.Models;
using Services;
using System.Runtime.Serialization;

namespace Employee_Timesheet.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginService _loginService;


        public LoginController( ILoginService loginService)
        {
            _loginService = loginService;
   
        }


        public IActionResult EmployeeLogin()
        {
            ClaimsPrincipal claimUser = HttpContext.User;

            if (claimUser.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> EmployeeLogin(string username, string password)
        {
            try
            {

                if (IsValidEmployee(username, password))
                {
                    List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, username),
                };
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);


                    AuthenticationProperties properties = new AuthenticationProperties()
                    {
                        AllowRefresh = true,

                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity), properties);

                    // Successful login, redirect to the home page or any other authenticated area
                    return RedirectToAction("Index", "Home");
                }
                if (!IsValidEmployee(username, password))
                {
                    throw new InvalidUsernameOrPasswordException("Invalid username or password.");
                }
            }

            catch (InvalidUsernameOrPasswordException ex)
            {
                ViewData["ValidateMessage"] = ex.Message;
            }
            catch (Exception ex)
            {
                ViewData["ValidateMessage"] = "An unexpected error occurred while fetching project details.";
            }

                return View();
            
        }


        
        private bool IsValidEmployee(string username, string password)
        {
            if (username.StartsWith("EMP"))
            {
              var Employees = _loginService.IsValidEmployeeLogin(username, password);
                return Employees;
            }
            else if (username.StartsWith("MGR"))
            {
                var Managers = _loginService.IsValidManagerLogin(username, password);
                return Managers;
            }
            else
            {
                return false;
            }
        }

    }

   
    public class InvalidUsernameOrPasswordException : Exception
    {
        public InvalidUsernameOrPasswordException(string? message) : base(message)
        {

        }

    }
}
