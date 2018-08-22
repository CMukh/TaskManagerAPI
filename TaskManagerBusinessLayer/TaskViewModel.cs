using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerBusinessLayer
{
    public class TaskViewModel
    {
        public TaskViewModel()
        {
            
        }
        public TaskViewModel(int tid, string tName, string pName, string stdt, string endt, int? p)
        {
            TaskID = tid;
            TaskName = tName;
           // ParentTaskID = pid;
            ParentTaskName = pName;
            Start_Date = stdt;
            End_Date = endt;
            Priority = p;
        }
        public int TaskID { get; set; }
        public string TaskName { get; set; }
       // public int ParentTaskID { get; set; }
        public string ParentTaskName { get; set; }
        public string Start_Date { get; set; }
        public string End_Date { get; set; }
        public Nullable<int> Priority { get; set; }
    }
}
