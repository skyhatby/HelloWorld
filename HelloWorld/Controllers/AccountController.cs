﻿using System;
using System.Web.Mvc;
using System.Web.Security;
using HelloWorld.Filters;
using HelloWorld.Service;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using HelloWorld.Models;

namespace HelloWorld.Controllers
{
    [Authorize]
    [Culture]
    public class AccountController : Controller
    {

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
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, model.RememberMe))
            {
                return RedirectToLocal(returnUrl);
            }

            // Появление этого сообщения означает наличие ошибки; повторное отображение формы
            ModelState.AddModelError("", Resources.Resource.LoginError);
            return View(model);
        }

        //
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    var confirmationToken = WebSecurity.CreateUserAndAccount(model.UserName, model.Password, null, true);

                    var uri = new Uri("http://" + System.Web.HttpContext.Current.Request.Url.Host + "/Account/RegisterConfirmation/" + confirmationToken);
                    // int host = System.Web.HttpContext.Current.Request.Url.Port;
                    // String host = uri.Scheme + Uri.SchemeDelimiter + uri.Host + ":" + uri.Port;

                    string body = "<h4>" + Resources.Resource.LoginLetter
                     + model.UserName
                     + "\n" + Resources.Resource.PassLetter
                     + model.Password
                     + ".</div> <div>" + Resources.Resource.ActivLetter
                     + "<a href=\"" + uri
                     + "\">" + Resources.Resource.PressLetter + "</a>"
                     + "<div></h4>";
                    new Email(model.Email, "Registration process", body);

                    return RedirectToAction("RegisterStepTwo");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult RegisterStepTwo()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult RegisterConfirmation(string id)
        {
            if (WebSecurity.ConfirmAccount(id))
            {
                return RedirectToAction("ConfirmationSuccess");
            }
            return RedirectToAction("ConfirmationFailure");
        }

        [AllowAnonymous]
        public ActionResult ConfirmationSuccess()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ConfirmationFailure()
        {
            return View();
        }

       
        //
        // GET: /Account/Manage

        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? Resources.Resource.PassChanged
                : message == ManageMessageId.SetPasswordSuccess ? Resources.Resource.PassSet
                : "";
            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model)
        {
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // В ряде случаев при сбое ChangePassword породит исключение, а не вернет false.
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        ModelState.AddModelError("", Resources.Resource.IncorrectPass);
                    }
                }
            }
            else
            {
                // У пользователя нет локального пароля, уберите все ошибки проверки, вызванные отсутствующим
                // полем OldPassword
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("", String.Format("Не удалось создать локальную учетную запись. Возможно, учетная запись \"{0}\" уже существует.", User.Identity.Name));
                    }
                }
            }

            // Появление этого сообщения означает наличие ошибки; повторное отображение формы
            return View(model);
        }

        #region Вспомогательные методы
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // Полный список кодов состояния см. по адресу http://go.microsoft.com/fwlink/?LinkID=177550
            //.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "Имя пользователя уже существует. Введите другое имя пользователя.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "Имя пользователя для данного адреса электронной почты уже существует. Введите другой адрес электронной почты.";

                case MembershipCreateStatus.InvalidPassword:
                    return "Указан недопустимый пароль. Введите допустимое значение пароля.";

                case MembershipCreateStatus.InvalidEmail:
                    return "Указан недопустимый адрес электронной почты. Проверьте значение и повторите попытку.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "Указан недопустимый ответ на вопрос для восстановления пароля. Проверьте значение и повторите попытку.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "Указан недопустимый вопрос для восстановления пароля. Проверьте значение и повторите попытку.";

                case MembershipCreateStatus.InvalidUserName:
                    return "Указано недопустимое имя пользователя. Проверьте значение и повторите попытку.";

                case MembershipCreateStatus.ProviderError:
                    return "Поставщик проверки подлинности вернул ошибку. Проверьте введенное значение и повторите попытку. Если проблему устранить не удастся, обратитесь к системному администратору.";

                case MembershipCreateStatus.UserRejected:
                    return "Запрос создания пользователя был отменен. Проверьте введенное значение и повторите попытку. Если проблему устранить не удастся, обратитесь к системному администратору.";

                default:
                    return "Произошла неизвестная ошибка. Проверьте введенное значение и повторите попытку. Если проблему устранить не удастся, обратитесь к системному администратору.";
            }
        }
        #endregion


    }
}
