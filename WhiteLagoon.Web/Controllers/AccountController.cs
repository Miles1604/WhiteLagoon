using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Web.ViewModels;
using WhiteLagoon.Application.Common.Utility;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Diagnostics.Eventing.Reader;

namespace WhiteLagoon.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _signInManager = signInManager;
        }


        public IActionResult Login(string returnUrl = null)
        {

            returnUrl ??= Url.Content("~/"); //this says every time return url is not empty then it will populate that with the content else it will use returnurl


            LoginVM loginVM = new()
            {
                RedirectUrl = returnUrl
            };

            return View(loginVM);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
        return View();
        }



        public IActionResult Register(string returnUrl = null)

		{
			returnUrl ??= Url.Content("~/");

			if (!_roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult()) ; //get results returns a boolean
            {

                //the two below will create the two roles every time Register method is invoked. To stop this we will add condition in line above this
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).Wait();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).Wait();
            }

            RegisterVM registerVM = new()
            {
                RoleList = _roleManager.Roles.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Name
                }),
                RedirectUrl = returnUrl
            };


            return View(registerVM);
        }
        

        [HttpPost]
			public async Task<IActionResult> Register(RegisterVM registerVM)
        {
                if (ModelState.IsValid)
                {

                    ApplicationUser user = new()
                    {
                        Name = registerVM.Name,
                        Email = registerVM.Email,
                        PhoneNumber = registerVM.PhoneNumber,
                        NormalizedEmail = registerVM.Email.ToUpper(),
                        EmailConfirmed = true,
                        UserName = registerVM.Email,
                        CreatedAt = DateTime.Now,
                    };

                    var result = await _userManager.CreateAsync(user, registerVM.Password);

                    if (result.Succeeded)
                    {
                        if (!string.IsNullOrEmpty(registerVM.Role))
                        {
                            await _userManager.AddToRoleAsync(user, registerVM.Role);
                        }
                        else
                        {
                            await _userManager.AddToRoleAsync(user, SD.Role_Customer);
                        }

                        await _signInManager.SignInAsync(user, isPersistent: false);
                        if (string.IsNullOrEmpty(registerVM.RedirectUrl))
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            return LocalRedirect(registerVM.RedirectUrl); //Localredirect always safer and best practice. redirect to same domain
                        }
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }

                registerVM.RoleList = _roleManager.Roles.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Name
                });


                return View(registerVM);
            }


        
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager
                    .PasswordSignInAsync(loginVM.Email, loginVM.Password, loginVM.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {


                    if (string.IsNullOrEmpty(loginVM.RedirectUrl))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return LocalRedirect(loginVM.RedirectUrl); //Localredirect always safer and best practice. redirect to same domain
                    }

                }
                else
                {
                    ModelState.AddModelError("", "Invalid Login Attempt.");
                }
            }
            return View(loginVM);
        }
    }
}





