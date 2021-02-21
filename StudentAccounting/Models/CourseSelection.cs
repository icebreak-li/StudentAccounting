using System;
using System.Collections.Generic;

namespace StudentAccounting.Models
{
    public class CourseSelection
    {
        public int ID { get; set; }
        public int StudentID { get; set; }
        public int CourseID { get; set; }
        public double Paid { get; set; }
        public bool Active { get; set; }

        public Course Course { get; set; }
        public Student Student { get; set; }
    }
}
