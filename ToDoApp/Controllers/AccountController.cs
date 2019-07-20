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
          
            var result = (from log in context.Login
                              where log.username == login.Username && log.password == login.Password
                              select log).FirstOrDefault();
            if (result != null)
            {
                Session["user"] = new LoginModel {UserId = result.Id , Username = result.username,IsAuthenticated =true};
                return RedirectToAction("Dashboard", "ToDo");
            }
            else
                return View("Login");
        }

        public ActionResult LogOff()
        {
            Session["user"] = null;
            Session.Abandon();
            return RedirectToAction("Index", "Account");
        }

        public ActionResult Register()
        {
            return View("Register");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(LoginModel model)
        {
            var input = new Login()
            {
                username = model.Username,
                password = model.Password
            };

            context.Login.Add(input);
            var result = context.SaveChanges();
            if(result == 1)
            {
                return RedirectToAction("Index", "Account");
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
    }
}