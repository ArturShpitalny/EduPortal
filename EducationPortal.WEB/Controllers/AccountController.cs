using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Security.Claims;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using EducationPortal.Core.Models.Auth;
using EducationPortal.WEB.Models.ViewModel;
using EducationPortal.BLL.Services;
using System;
using System.Web.Configuration;
using EducationPortal.WEB.Managers;
using EducationPortal.Core.Models.States;
using EducationPortal.Core.Models.Entities;
using System.Collections.Generic;
using EducationPortal.Core.Models.DTO;

namespace EducationPortal.WEB.Controllers
{
    public class AccountController : Controller
    {
        public readonly AccountService accountService;

        private readonly int pageSize;

        public AccountController(AccountService accountService)
        {
            this.accountService = accountService;

            int.TryParse(WebConfigurationManager.AppSettings["ElementOnPage"], out pageSize);
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public ActionResult WindowAfterRegistration()
        {
            return View();
        }

        //Registration form
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(UserRegistrationFormModel model)
        {
            if (model != null)
            {
                if (ModelState.IsValid)
                {
                    Role role = this.accountService.GetRole("User");
                    User registration = new User();
                    registration.RoleId = role.Id;
                    registration.UserEmail = model.Email;
                    registration.UserPassword = HashManager.HashData(model.Password);

                    if (model.Birthdate > DateTime.Today)
                    {
                        ModelState.AddModelError("Birthdate", "Дата рождения не может быть в будущем");
                        return View(model);
                    }

                    registration.UserBirthdate = model.Birthdate;

                    var state = this.accountService.AddUser(registration);

                    if (state.State == true)
                    {
                        EmailManager emailManager = new EmailManager();
                        emailManager.SendEmail($"Спасибо что зарегистрировались на сайте КурсоВод!<br/>Ваш логин: {model.Email};", "Registration on the kursovod.edu.ua", model.Email);

                        return RedirectToAction("WindowAfterRegistration", "Account");
                    }

                    ModelState.AddModelError("Email", "Пользователь с такими данными уже зарегистрирован.");
                }
                return View(model);
            }
            return View();
        }

        //Login form
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginFormModel model)
        {
            if (model != null)
            {
                if (ModelState.IsValid)
                {
                    UserModel user = this.accountService.GetUser(model.Email, HashManager.HashData(model.Password));

                    if (user == null)
                    {
                        ModelState.AddModelError("Email", "Неверный логин или пароль.");
                    }
                    else
                    {
                        Role role = this.accountService.GetRole(user.RoleId);

                        ClaimsIdentity claim = new ClaimsIdentity("ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                        claim.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(), ClaimValueTypes.String));
                        claim.AddClaim(new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email, ClaimValueTypes.String));
                        claim.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider",
                            "OWIN Provider", ClaimValueTypes.String));
                        if (role != null)
                            claim.AddClaim(new Claim(ClaimsIdentity.DefaultRoleClaimType, role.RoleName, ClaimValueTypes.String)); //<---user.Role.RoleName

                        AuthenticationManager.SignOut();
                        AuthenticationManager.SignIn(new AuthenticationProperties
                        {
                            IsPersistent = true
                        }, claim);
                        return RedirectToAction("MainPageCourses", "Course");
                    }
                }
                return View(model);
            }
            return View();
        }

        //Exit button
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("MainPageCourses", "Course");
        }

        //Partial view of user navigation menu
        public ActionResult Menu()
        {
            return PartialView();
        }

        //An output page for all site users with pagination in the admin panel
        [Authorize(Roles = "Admin")]
        public ActionResult AdminPanelUser(int page = 1, string search = "")
        {
            return PartialView(this.accountService.GetUsers(pageSize, page, search));
        }

        //User Editing
        [Authorize(Roles = "Admin")]
        public ActionResult EditUser(int id)
        {
            var model = this.accountService.GetUser(id);

            if (model == null)
            {
                return View(new UserFormModel() { Id = -1 });
            }

            return View(new UserFormModel() { Id = model.Id, Email = model.Email, Birthdate = model.Birthdate, RoleId = model.RoleId });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult EditUser(UserFormModel model)
        {
            if (model != null)
            {
                if (ModelState.IsValid)
                {
                    User user = new User();
                    user.Id = model.Id;
                    user.UserEmail = model.Email;

                    if (model.Birthdate > DateTime.Today)
                    {
                        ModelState.AddModelError("Birthdate", "Дата рождения не может быть в будущем");
                        return View(model);
                    }

                    user.UserBirthdate = model.Birthdate;
                    user.RoleId = model.RoleId;

                    if (model.Password != null)
                    {
                        user.UserPassword = HashManager.HashData(model.Password);
                    }

                    if (model.Id > 0)
                    {
                        this.accountService.UpdateUser(user);
                    }
                    else
                    {
                        this.accountService.AddUser(user);
                    }

                    return RedirectToAction("AdminPanelUser");
                }
                return View(model);
            }
            return View();
        }

        //Delete user (Ajax)
        [HttpPost]
        public ActionResult DeleteUser(int id)
        {
            if (User.Identity.IsAuthenticated && User.Identity.GetUserRole() == "Admin")
            {
                ResponseState state = this.accountService.DeleteUser(id);

                if (state.State == true && state.Massage == "OK")
                {
                    return Json(new ResultModel { Index = id, Message = state.Massage });
                }
                else if (state.State == false && state.Massage == "UserIsAbsent")
                {
                    return Json(new ResultModel { Index = id, Message = state.Massage });
                }

                return Json(new ResultModel { Index = id, Message = "ERR" }); 
            }
            return Json(new ResultModel { Index = id, Message = "UserNotAuthorize" });
        }

        //Listing Roles When Editing a User
        [Authorize(Roles = "Admin")]
        public ActionResult UserRole(int selectedId)
        {
            return PartialView(new EntityIndexModel<IEnumerable<Role>> { Entity = this.accountService.GetRoles(), Index = selectedId });
        }

        [Authorize(Roles = "Admin, User")]
        public ActionResult UserInfo()
        {
            var info = this.accountService.UserInfo(User.Identity.GetUserId<int>());

            if (info != null)
            {
                return View(info);
            }
            return HttpNotFound();
        }
    }
}