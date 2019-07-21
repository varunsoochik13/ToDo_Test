using System.Web.Mvc;
using ToDoApp.DataModel;
using System.Linq;
using ToDoApp.Models;
using System.Net;

namespace ToDoApp.Controllers
{
    public class AccountController : Controller
    {
        ToDoDBEntities context;
        public AccountController()
        {
            context = new ToDoDBEntities();
        }
        // GET: Account
        public ActionResult Index()
        {
            return View("Login");
        }

        [ValidateAntiForgeryToken]
        public ActionResult VerifyLogin(LoginModel login)
        {
            try
            {
                
                var result = (from log in context.Login
                              where log.username == login.Username && log.password == login.Password
                              select log).FirstOrDefault();
                if (result != null)
                {
                    Session["user"] = new LoginModel { UserId = result.Id, Username = result.username, IsAuthenticated = true };
                    return RedirectToAction("Dashboard", "ToDo");
                }
                else {
                    ViewBag.error = "Username or Password is Incorrect";
                    return View("Error");
                }
                    
            }
            catch (System.Exception ex)
            {
                ViewBag.error = ex.Message;
                return View("Error");
            }

        }

        public ActionResult LogOff()
        {
            try
            {
                Session["user"] = null;
                Session.Abandon();
                return RedirectToAction("Index", "Account");
            }
            catch (System.Exception ex)
            {
                ViewBag.error = ex.Message;
                return View("Error");
            }

        }

        public ActionResult Register()
        {
            try
            {
                return View("Register");
            }
            catch (System.Exception ex)
            {
                ViewBag.error = ex.Message;
                return View("Error");
            }
         
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(LoginModel model)
        {
            try
            {
                var input = new Login()
                {
                    username = model.Username,
                    password = model.Password
                };

                context.Login.Add(input);
                var result = context.SaveChanges();
                if (result == 1)
                {
                    return RedirectToAction("Index", "Account");
                }
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            catch (System.Exception ex)
            {
                ViewBag.error = ex.Message;
                return View("Error");
            }

        }
    }
}