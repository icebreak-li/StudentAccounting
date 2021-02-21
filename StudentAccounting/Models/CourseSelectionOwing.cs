using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAccounting.Models
{
    public class CourseSelectionOwing
    {
        public int ID { get; set; }
        public int StudentID { get; set; }
        public int CourseID { get; set; }
        public double Owing { get; set; }
        public bool Active { get; set; }

        public Course Course { get; set; }
        public Student Student { get; set; }
    }
}
