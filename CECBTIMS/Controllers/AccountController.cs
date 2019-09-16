﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CECBTIMS.DAL;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using CECBTIMS.Models;
using CECBTIMS.ViewModels;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CECBTIMS.Controllers
{
    [Authorize]
//    [Authorize(Roles = "Admin")]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext context;

        public AccountController()
        {
            context = new ApplicationDbContext();
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get { return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>(); }
            private set { _signInManager = value; }
        }

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        // List current Users of the System
        public async Task<ActionResult> Index()
        {
            // if Administrator show all the accounts
            if (User.IsInRole("Administrator"))
            {
                var users = await context.Users.ToListAsync();

                return View(users);
            }

            var user = from u in context.Users
                select u;
            var currentUserName = User.Identity.GetUserName();

            user = user.Where(p => p.UserName == currentUserName);

            return View(await user.ToListAsync());
        }

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Model State Not Valid Error!");
                return View(model);
            }

            // This doesn't count login failures towards account lockout    
            // To enable password failures to trigger account lockout, change to shouldLockout: true    
            var result = await SignInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe,
                shouldLockout: true);

            if (result == SignInStatus.Success)
            {
                return RedirectToLocal(returnUrl);
            }
            else
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }
        }

        // GET: /Account/Register
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Create()
        {
            ViewBag.Name = await context.Roles.Select(n =>
                new SelectListItem
                {
                    Value = n.Name,
                    Text = n.Name
                }).ToListAsync();


            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = new ApplicationUser {UserName = model.Username, Email = model.Email};
            var result = await UserManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await UserManager.AddToRoleAsync(user.Id, model.UserRoles);
                //Ends Here     
                return RedirectToAction("Index", "Account");
            }

            ViewBag.Name = await context.Roles.Select(n =>
                new SelectListItem
                {
                    Value = n.Name,
                    Text = n.Name
                }).ToListAsync();

            AddErrors(result);

            // If we got this far, something failed, redisplay form    
            return View(model);
        }

        public async Task<ActionResult> Edit(string id)
        {
            if ((await UserManager.FindByIdAsync(id)) == null) return new HttpNotFoundResult();

            ViewBag.UserId = id;

            return View();
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditConfirmed(EditPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            //get the users
            var user = await UserManager.FindByIdAsync(model.Id);
            // compare the current passwords
            var res = UserManager.PasswordHasher.VerifyHashedPassword(user.PasswordHash, model.OldPassword);

            if (res == PasswordVerificationResult.Success)
            {
                var token = await UserManager.GeneratePasswordResetTokenAsync(model.Id);

                var result = await UserManager.ResetPasswordAsync(model.Id, token, model.Password);

                if (result.Succeeded)
                {
                    TempData["msg_success"] = "Password Updated";
                    return RedirectToAction("Index");
                }

                ViewBag.UserId = model.Id;
                ModelState.AddModelError("", "Password change failed please try again.");
                return View(model);
            }

            ViewBag.UserId = model.Id;
            ModelState.AddModelError("", "Current password dose not match with the password in the Database.");
            return View(model);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> ResetPassword(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            if (user == null) return new HttpNotFoundResult();

            if (!(user.Id).Equals(User.Identity.GetUserId()))
                return View(new ResetPasswordViewModel()
                {
                    Id = user.Id
                });


            TempData["msg_fail"] = "You can't reset user account password you currently logged in. Please try Edit the password.";
            return RedirectToAction($"Index");
        }

        [HttpPost, ActionName("ResetPassword")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> ResetPasswordConfirmed(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var user = await UserManager.FindByIdAsync(model.Id);
            if (user == null) return new HttpNotFoundResult();

            var token = await UserManager.GeneratePasswordResetTokenAsync(model.Id);

            var result = await UserManager.ResetPasswordAsync(model.Id, token, model.Password);

            if (result.Succeeded)
            {
                TempData["msg_success"] = "Password Updated";
                return RedirectToAction($"Index");
            }

            ModelState.AddModelError("", "Password Reset failed please try again.");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Lock(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            if (user == null) return new HttpNotFoundResult();

            if ((user.Id).Equals(User.Identity.GetUserId()))
            {
                TempData["msg_fail"] = "You can't Lock the account currently Logged in";
                return RedirectToAction($"Index");
            }

            var result = await UserManager.SetLockoutEnabledAsync(user.Id, true);

            if (result.Succeeded)
            {
                TempData["msg_success"] = user.UserName + "'s user account Locked";
                return RedirectToAction($"Index");
            }

            TempData["msg_fail"] = "User account lock failed. Please try again";
            return RedirectToAction($"Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Unlock(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            if (user == null) return new HttpNotFoundResult();

            var result = await UserManager.SetLockoutEnabledAsync(user.Id, false);

            if (result.Succeeded)
            {
                TempData["msg_success"] = user.UserName + "'s user account Unlocked";
                return RedirectToAction($"Index");
            }

            TempData["msg_fail"] = "User account unlock failed. Please try again";
            return RedirectToAction($"Index");
        }
        


        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Delete(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            if (user == null) return new HttpNotFoundResult();

            return View(new DeleteViewModel()
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.UserName
            });
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> DeleteConfirmd(DeleteViewModel model)
        {
            if (!ModelState.IsValid) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var user = await UserManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                ModelState.AddModelError("", "The user can't be found in the database.'");
                return View(model);
            }

            // this stops user from delete the only admin account.
            if ((user.Id).Equals(User.Identity.GetUserId()))
            {
                TempData["msg_fail"] = "You can't delete the account you are logged in.";
                return RedirectToAction($"Index");
            }

            // check created records
            if (
                user.Programs.Any() || user.Agendas.Any() || user.Brochures.Any() || user.Costs.Any() ||
                user.DefaultColumns.Any() || user.Documents.Any() || user.Organizers.Any() || user.Payments.Any() ||
                user.Templates.Any() || user.ProgramAssignments.Any() || user.ResourcePersons.Any() ||
                user.Requirements.Any() ||
                user.EmploymentCategories.Any() || user.EmploymentNatures.Any() || user.TargetGroups.Any()
            )
            {
                TempData["msg_fail"] =
                    "Delete Failed. The selected user account identified as a active account. try Locking the account.";
                return RedirectToAction($"Index");
            }

            var logins = user.Logins;
            var rolesForUser = await UserManager.GetRolesAsync(user.Id);

            using (var transaction = context.Database.BeginTransaction())
            {
                foreach (var login in logins.ToList())
                {
                    await UserManager.RemoveLoginAsync(login.UserId,
                        new UserLoginInfo(login.LoginProvider, login.ProviderKey));
                }

                if (rolesForUser.Any())
                {
                    foreach (var item in rolesForUser.ToList())
                    {
                        var result = await UserManager.RemoveFromRoleAsync(user.Id, item);
                    }
                }

                await UserManager.DeleteAsync(user);
                transaction.Commit();
            }

            TempData["msg_success"] = "User account deleted successfully.";
            return RedirectToAction($"Index");
        }


        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }
        // GET: /Account/ExternalLoginFailure

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers

        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties {RedirectUri = RedirectUri};
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }

                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

        #endregion
    }
}