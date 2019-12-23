using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using QNews.Admin.Models;
using System.Text.RegularExpressions;

namespace QNews.Admin.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            ViewData.Model = string.Empty;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult>  ChangePassword(FormCollection collection)
        {
            Regex regexPass = new Regex(@"^.*(?=.{7,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9]).*$");
            string username = "Admin";// User.Identity.GetUserName();
            string userID = "a258ea3a-7abf-4a25-8db4-a1ab29f9a785";// User.Identity.GetUserId();
            string password = collection["Password"];
            string passwordNew = collection["PasswordNew"];
            string passwordNew2 = collection["PasswordNew2"];
            string passwordWithSalt = Utils.StaticClass.MD5ByPHP(username) + password;
            string passwordWithSaltNew = Utils.StaticClass.MD5ByPHP(username) + passwordNew;

            string msg = string.Empty;

          if (!regexPass.IsMatch(passwordNew))
            {
                msg += "Mật khẩu phải ít nhất 7 ký tự, bao gồm chữ hoa, chữ thường, số và ký tự đặc biệt<br>";
            }

            if (string.IsNullOrEmpty(password))
            {
                msg += "Nhập mật khẩu cũ<br>";
            }
            if (string.IsNullOrEmpty(passwordNew))
            {
                msg += "Nhập mật khẩu mới<br>";
            }

            if (string.IsNullOrEmpty(passwordNew2))
            {
                msg += "Nhập mật lại mật khẩu mới<br>";
            }

            if (passwordNew2 != passwordNew)
            {
                msg += "Nhập mật mới nhập lại yêu cầu phải giống nhau<br>";
            }

            /*if (string.IsNullOrEmpty(msg))
            {
                var result = await SignInManager.PasswordSignInAsync(username, password, false, shouldLockout: false);
                switch (result)
                {
                    case SignInStatus.Success:*/
                        UserManager.RemovePassword(userID);
                        UserManager.AddPassword(userID, passwordWithSaltNew);
                        msg = "Đổi mật khẩu thành công";
                   /*     break;
                    default:
                        msg = "Mật khẩu cũ không đúng<br>";
                        break;
                }
            }
            */
            ViewData.Model = msg;
            return View();
        }


        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (Session["CaptchaImageText"] != null && ((string)Request["Captcha"]) == ((string)Session["CaptchaImageText"]))
            {
                ViewBag.Message = string.Empty;
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, change to shouldLockout: true

                var passwordWithSalt = Utils.StaticClass.MD5ByPHP(model.UserName) + model.Password;
                //var passwordWithSalt = model.Password;
                //var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
                var result = await SignInManager.PasswordSignInAsync(model.UserName,passwordWithSalt, model.RememberMe, shouldLockout: false);
                switch (result)
                {
                    case SignInStatus.Success:
                        return RedirectToLocal(returnUrl);
                    case SignInStatus.LockedOut:
                        return View("Lockout");
                    case SignInStatus.RequiresVerification:
                        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                    case SignInStatus.Failure:
                    default:
                        ModelState.AddModelError("", "Invalid login attempt.");
                        return View(model);
                }
            }
            else
            {
                ViewBag.Message = "Sai mã bảo mật. Xin vui lòng thử lại";
                Session["CaptchaImageText"] = GenerateRandomCode();
                return View(model);
            }
        }

        private Random random = new Random();
        private string GenerateRandomCode()
        {
            string s = "";
            for (int i = 0; i < 5; i++)
                s = String.Concat(s, this.random.Next(10).ToString());
            return s;
        }
        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            //var ctx = Request.GetOwinContext();
            //var authenticationManager = ctx.Authentication;
            //authenticationManager.SignOut();

            Request.GetOwinContext().Authentication.SignOut(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Default");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

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
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
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
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
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