using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VMM.Models;
using System.Security.Claims;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using VMM.Common;
using VMM.Auth;

namespace VMM.Controllers {
    [AllowAnonymous]
    public class AuthController : ControllerTemplate {
        // GET: Auth
        public ActionResult Index() {
            return View();
        }

        [HttpGet]
        public ActionResult Login(string strReturnUrl) {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignOut("ApplicationCookie");

            LoginModel model = new LoginModel {
                ReturnUrl = strReturnUrl
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Login(LoginModel model) {
            if (!ModelState.IsValid) {
                return View();
            }

            ClaimsIdentity identity = null;
            UserConfig user = new UserConfig(db);

            if (user.VerifyUser(model, out identity)) {
                IOwinContext ctx = Request.GetOwinContext();
                IAuthenticationManager authManager = ctx.Authentication;

                authManager.SignIn(identity);

                return Redirect(GetRedirectUrl(model.ReturnUrl));
            }
            // user authN failed
            ModelState.AddModelError("", "Invalid username or password");
            return View();
        }

        [HttpGet]
        public ActionResult Register() {
            RegisterModel model = new RegisterModel {
                ReturnUrl = "/Auth/Login"
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model) {
            if (!ModelState.IsValid) {
                return View();
            }

            UserConfig user = new UserConfig(db);
            try {
                user.RegisterUser(model);
                return Redirect(GetRedirectUrl(model.ReturnUrl));
            } catch (Exception ex) {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        public ActionResult Logout() {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignOut("ApplicationCookie");
            return RedirectToAction("login", "auth");
        }

        private string GetRedirectUrl(string strReturnUrl) {
            if (string.IsNullOrEmpty(strReturnUrl)) {
                return Url.Action("index", "tank");
            } else {
                return strReturnUrl;
            }
        }
    }
}