using System;
using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Models
{
    public class TaskModel
    {
        public bool IsSelected { get; set; }
        public int TaskId { get; set; }
        public int UserId { get; set; }
        [Required]
        [Display(Name ="Task Name")]
        public string TaskName { get; set; }
        [Display(Name ="Task Description")]
        [Required]
        public string TaskDescription { get; set; }
        [Display(Name = "Task Status")]
        [Required]
        public string TaskStatus { get; set; }
        [Display(Name = "Task Created on ")]
        public DateTime CreationTime { get; set; }
        [Display(Name = "Task Modified on ")]
        public DateTime? ModificationTime { get; set; }

    }
}