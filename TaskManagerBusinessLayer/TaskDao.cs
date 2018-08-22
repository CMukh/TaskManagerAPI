using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerDataLayer;

namespace TaskManagerBusinessLayer
{
    public class TaskDao
    {
        public List<TaskViewModel> GetAll()
        {
            string parentTask = "";
            TaskDBEntities taskDBEntities = new TaskDBEntities();
            //taskDBEntities.TaskDetails.Include(x => x.TaskDetail1);
            List<TaskDetails> task = taskDBEntities.TaskDetails.ToList();
            List<TaskViewModel> taskViewModelList = new List<TaskViewModel>();
            foreach (var t in task)
            {
                parentTask = "";
                if (t.Parent_ID != null)
                    parentTask = taskDBEntities.TaskDetails.SingleOrDefault(p => p.Task_ID == t.Parent_ID).Task_Description;
                taskViewModelList.Add(new TaskViewModel
                    (t.Task_ID, t.Task_Description, parentTask, t.Start_Date.ToShortDateString(), t.End_Date.ToShortDateString(), t.Priority));
            }
            return taskViewModelList;
        }
        public TaskViewModel GetTask(int id)
        {
            string parentTask = "";
            TaskDBEntities taskDBEntities = new TaskDBEntities();
            var task = taskDBEntities.TaskDetails.SingleOrDefault(p => p.Task_ID == id);
            if (task.Parent_ID != null )
                 parentTask = taskDBEntities.TaskDetails.SingleOrDefault(p => p.Task_ID == task.Parent_ID).Task_Description;
            return new TaskViewModel
                (task.Task_ID, task.Task_Description, parentTask, task.Start_Date.ToShortDateString(), task.End_Date.ToShortDateString(), task.Priority);

        }
        public void DeleteTask(int id)
        {
            TaskDBEntities taskDBEntities = new TaskDBEntities();
            TaskDetails task = taskDBEntities.TaskDetails.SingleOrDefault(p => p.Task_ID == id);
            var entry = taskDBEntities.Entry(task);
            if (entry.State == System.Data.Entity.EntityState.Detached)
                taskDBEntities.TaskDetails.Attach(task);
            taskDBEntities.TaskDetails.Remove(task);
            taskDBEntities.SaveChanges();
        }
        public void AddTask(TaskViewModel taskVM)
        {
            TaskDBEntities taskDBEntities = new TaskDBEntities();
            TaskDetails task = new TaskDetails();
            task.Task_Description = taskVM.TaskName;
            if(taskVM.ParentTaskName != null )
                task.Parent_ID = taskDBEntities.TaskDetails.SingleOrDefault(p => p.Task_Description == taskVM.ParentTaskName).Task_ID;
            task.Start_Date = Convert.ToDateTime(taskVM.Start_Date);
            task.End_Date = Convert.ToDateTime(taskVM.End_Date);
            task.Priority = taskVM.Priority;
            taskDBEntities.TaskDetails.Add(task);
            taskDBEntities.SaveChanges();

        }
        public void EditTask(TaskViewModel taskVM)
        {
            TaskDBEntities taskDBEntities = new TaskDBEntities();
            TaskDetails task=taskDBEntities.TaskDetails.SingleOrDefault(p => p.Task_ID == taskVM.TaskID);
            if (taskVM.ParentTaskName != null && taskVM.ParentTaskName!="") 
                task.Parent_ID = taskDBEntities.TaskDetails.SingleOrDefault(p => p.Task_Description == taskVM.ParentTaskName).Task_ID;
            task.Start_Date = Convert.ToDateTime(taskVM.Start_Date);
            task.End_Date = Convert.ToDateTime(taskVM.End_Date);
            task.Priority = taskVM.Priority;

            taskDBEntities.TaskDetails.Attach(task);
            taskDBEntities.Entry(task).State = System.Data.Entity.EntityState.Modified;
            taskDBEntities.SaveChanges();

        }
    }
}
