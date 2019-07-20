using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ToDoApp.DataModel;
using ToDoApp.Models;

namespace ToDoApp.Controllers
{
    public class ToDoController : Controller
    {
        // GET: ToDo
        ToDoDBEntities context;
        public ToDoController()
        {
            context = new ToDoDBEntities();
        }
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult Dashboard()
        {
            //TempData["user_id"] = userId;
            var UserId = Session["user"] as LoginModel;
            if (Session["user"] != null) {
                List<TaskModel> model = (from tasks in context.Task
                                         where tasks.userId == UserId.UserId
                                         select new TaskModel
                                         {
                                             TaskId = tasks.taskId,
                                             UserId = tasks.userId,
                                             TaskName = tasks.taskName,
                                             TaskDescription = tasks.taskDesc,
                                             TaskStatus = tasks.status,
                                             CreationTime = tasks.creation_dt,
                                             ModificationTime = tasks.modified_dt
                                         }).ToList();

                return View("Dashboard", model);
            }
            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);

        }

        public ActionResult Create()
        {
            return View("Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TaskModel model)
        {
            var UserId = Session["user"] as LoginModel;
            if (Session["user"] != null) {
                var task = new Task()
                {
                    taskName = model.TaskName,
                    taskDesc = model.TaskDescription,
                    userId = UserId.UserId,
                    creation_dt = DateTime.Now,
                    modified_dt = DateTime.Now,
                    status = model.TaskStatus
                };
                context.Task.Add(task);
                context.SaveChanges();
                return RedirectToAction("Dashboard", "Todo");
            }
            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);

        }


        public ActionResult Edit(int? taskId, int? userId)
        {
            var UserId = Session["user"] as LoginModel;
            if (Session["user"] != null)
            {
                if (taskId == null || userId == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var model = (from task in context.Task
                             where task.taskId == taskId && task.userId == userId
                             select new TaskModel
                             {
                                 TaskId = task.taskId,
                                 TaskName = task.taskName,
                                 TaskDescription = task.taskDesc,
                                 TaskStatus = task.status,
                                 UserId = task.userId
                             }).FirstOrDefault();

                if (UserId.UserId == userId) { return View("Edit", model); }
            }
            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TaskModel model)
        {

            var UserId = Session["user"] as LoginModel;

                var updateTask = (from task in context.Task
                                  where task.taskId == model.TaskId && task.userId == model.UserId
                                  select task).FirstOrDefault();


            if (UserId.UserId == updateTask.userId)
            {
                updateTask.taskName = model.TaskName;
                updateTask.taskDesc = model.TaskDescription;
                updateTask.status = model.TaskStatus;
                updateTask.modified_dt = DateTime.Now;
                context.SaveChanges();
                return RedirectToAction("Dashboard", "Todo");
            }

            

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }


        public ActionResult Details(int? taskId, int? userId)
        {
            var UserId = Session["user"] as LoginModel;
            if (Session["user"] != null) {
                if (taskId == null || userId == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var model = (from task in context.Task
                             where task.taskId == taskId && task.userId == userId
                             select new TaskModel
                             {
                                 TaskId = task.taskId,
                                 TaskName = task.taskName,
                                 TaskDescription = task.taskDesc,
                                 TaskStatus = task.status,
                                 CreationTime = task.creation_dt,
                                 ModificationTime = task.modified_dt,
                                 UserId = task.userId
                             }).FirstOrDefault();
                if (UserId.UserId == userId)
                {
                    return View("Details", model);
                }

            }

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }


        public ActionResult Delete(int? taskId , int? userId)
        {
            var UserId = Session["user"] as LoginModel;
            if (Session["user"] != null)
            {
                if (taskId == null || userId == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var model = (from task in context.Task
                             where task.taskId == taskId && task.userId == userId
                             select new TaskModel
                             {
                                 TaskId = task.taskId,
                                 TaskName = task.taskName,
                                 TaskDescription = task.taskDesc,
                                 TaskStatus = task.status,
                                 UserId = task.userId,
                                 CreationTime = task.creation_dt
                             }).FirstOrDefault();
               if(UserId.UserId == userId) { return View("Delete", model); }
            }
            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);

        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(TaskModel model)
        {
            var UserId = Session["user"] as LoginModel;

            var deleteTask = (from task in context.Task
                              where task.taskId == model.TaskId && task.userId == model.UserId
                              select task).FirstOrDefault();
            if(UserId.UserId == deleteTask.userId)
            {
                context.Task.Remove(deleteTask);
                context.SaveChanges();
                return RedirectToAction("Dashboard", "Todo");
            }
            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }

        [HttpPost]
        public ActionResult UpdateStatus(string taskId)
        {
            var TaskId = Int32.Parse(taskId);
            var updateTask = (from task in context.Task
                              where task.taskId == TaskId
                              select task).FirstOrDefault();
            if(updateTask.status.Equals("In Progress"))
            {
                updateTask.status = "Completed";
            }
            else { updateTask.status = "In Progress"; }
            
            updateTask.modified_dt = DateTime.Now;
            context.SaveChanges();
            return RedirectToAction("Dashboard", "Todo");
        }
    }
}