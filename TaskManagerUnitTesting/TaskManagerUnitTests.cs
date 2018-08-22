using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http.Results;
using System.Threading.Tasks;
using TaskManagerDataLayer;
using TaskManagerServicesLayer;
using TaskManagerServicesLayer.Controllers;
using System.Web.Http;
using TaskManagerBusinessLayer;
using System.Net;

namespace TaskManagerUnitTesting
{
    public class TaskManagerUnitTests
    {
        TaskController taskController;
        TaskDao taskDao;
        [SetUp]
        public void Init()
        {
            taskDao = new TaskDao();
            taskController = new TaskController(taskDao);
            DeleteAllTasks();

        }
        private void DeleteAllTasks()
        {
            //Delete All tasks
            TaskDBEntities taskDBEntities = new TaskDBEntities();
            taskDBEntities.Database.ExecuteSqlCommand("Truncate table TaskDetails");
            // foreach (var t in taskDao.GetAll())
            // {
            //    taskDao.DeleteTask(t.TaskID);
            // }
        }

        private void AddNewTask()
        {
            TaskViewModel task = new TaskViewModel();
            task.TaskName = "Task1";
            task.End_Date = "05-05-2018";
            task.Start_Date = "01-01-2018";
            task.Priority = 10;
            taskController.Post(task);
        }

        [Test]
        public void Get()
        {
            //Add Task//
            AddNewTask();

            //GetAllTasks
            IHttpActionResult response = taskController.Get();
            var contentResult = response as OkNegotiatedContentResult<List<TaskViewModel>>;
            var t = contentResult.Content;

            //Count
            Assert.IsTrue(t.Where(tsk => tsk.TaskName == "Task1").Count() == 1);
        }

        [Test]
        public void GetTask()
        {
            //Add Task//
            AddNewTask();
            var t = taskController.Get(1);
            Assert.IsTrue(t.TaskName =="Task1");
        }
        [Test]
        public void AddTask()
        {
            //Add Task//
            AddNewTask();

            //Check if Added//
            IHttpActionResult response = taskController.Get();
            var contentResult = response as OkNegotiatedContentResult<List<TaskViewModel>>;
            var t = contentResult.Content;
            Assert.IsTrue(t.Where(tsk => tsk.TaskName == "Task1").Count() == 1);
        }

        [Test]
        public void DeleteTask()
        {
            //Add Task//
            AddNewTask();

            //Delete task//
            var response = taskController.Delete(1);

            //Check if Deleted//
            response = taskController.Get();
            var contentResult = response as NotFoundResult;
            // var t = contentResult.Content;
            Assert.IsNotNull(contentResult);
        }

        [Test]
        public void EditTask()
        {
            //Add Task//
            AddNewTask();

            var task = taskController.Get(1);

            //Edit Task//
            task.TaskName = "Task1";
            task.End_Date = "10-10-2018";
            task.Start_Date = "05-05-2018";
            task.Priority =20;
            taskController.Put(task);

            //Check if Updated//
            TaskViewModel t= taskController.Get(1);
            Assert.IsTrue(t.Priority == 20);
        }
    }
}
