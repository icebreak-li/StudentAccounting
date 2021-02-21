using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentAccounting.Models
{
    public class Course
    {
        public int CourseID { get; set; }
        [Required]
        [Display(Name = "Course Name")]
        public string CourseName { get; set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter a valid cost")]
        public double Cost { get; set; }
        public bool Active { get; set; }

        public ICollection<CourseSelection> CourseSelections { get; set; }
    }
}
