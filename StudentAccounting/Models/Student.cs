using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentAccounting.Models
{
    public class Student
    {
        public int StudentID { get; set; }
        [Required]
        [StringLength(128, ErrorMessage = "Name cannot be longer than 128 characters")]
        [Display(Name = "Student Name")]
        public string Name { get; set; }
        [Required]
        [Range(0, 1000, ErrorMessage = "Please enter a valid age")]
        public int Age { get; set; }
        public bool Active { get; set; }

        public ICollection<CourseSelection> CourseSelections { get; set; }
        public ICollection<CourseSelectionOwing> CourseSelectionOwings { get; set; }
    }
}
