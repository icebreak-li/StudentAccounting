using StudentAccounting.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAccounting.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AccountingContext context)
        {
            context.Database.EnsureCreated();
            // Look for any students
            if (context.Students.Any())
            {
                return; // DB has been seeded
            }
            var students = new Student[]
            {
                new Student{Name="Guobin Li",Age=31, Active = true},
                new Student{Name="John",Age=32, Active = true},
                new Student{Name="Alex",Age=18, Active = true},
             };
            foreach (Student s in students)
            {
                context.Students.Add(s);
            }
            context.SaveChanges();
            var courses = new Course[]
            {
                new Course{CourseName="Math",Cost=1000.00, Active = true},
                new Course{CourseName="Internet and Website Design",Cost=860.00, Active = true},
                new Course{CourseName="Web Application Development",Cost=1000.00, Active = true},
                new Course{CourseName="Database Design and Development",Cost=1200.00, Active = true},
                new Course{CourseName="Advanced Program Development",Cost=1400.00, Active = true},
                new Course{CourseName="Network Programming",Cost=1600.00, Active = true},
            };
            foreach (Course c in courses)
            {
                context.Courses.Add(c);
            }
            context.SaveChanges();
            var courseSelections = new CourseSelection[]
            {
                new CourseSelection{StudentID=1,CourseID=2,Paid=860.00, Active = true},
                new CourseSelection{StudentID=1,CourseID=3,Paid=1000.00, Active = true},
                new CourseSelection{StudentID=1,CourseID=4,Paid=1200.00, Active = true},
                new CourseSelection{StudentID=1,CourseID=5,Paid=1400.00, Active = true},
                new CourseSelection{StudentID=1,CourseID=6,Paid=1600.00, Active = true},
                new CourseSelection{StudentID=2,CourseID=1,Paid=800.00, Active = true},
                new CourseSelection{StudentID=2,CourseID=2,Paid=860.00, Active = true},
                new CourseSelection{StudentID=3,CourseID=3,Paid=0.00, Active = true},
                new CourseSelection{StudentID=3,CourseID=5,Paid=0.00, Active = true},
                new CourseSelection{StudentID=3,CourseID=4,Paid=1500.00, Active = true},
            };
            foreach (CourseSelection c in courseSelections)
            {
                context.CourseSelections.Add(c);
            }
            context.SaveChanges();
            var courseSelectionOwings = new CourseSelectionOwing[]
            {
                new CourseSelectionOwing{StudentID=1,CourseID=2,Owing=0, Active = true},
                new CourseSelectionOwing{StudentID=1,CourseID=3,Owing=0, Active = true},
                new CourseSelectionOwing{StudentID=1,CourseID=4,Owing=0, Active = true},
                new CourseSelectionOwing{StudentID=1,CourseID=5,Owing=0, Active = true},
                new CourseSelectionOwing{StudentID=1,CourseID=6,Owing=0, Active = true},
                new CourseSelectionOwing{StudentID=2,CourseID=1,Owing=200.00, Active = true},
                new CourseSelectionOwing{StudentID=2,CourseID=2,Owing=0, Active = true},
                new CourseSelectionOwing{StudentID=3,CourseID=5,Owing=1400.00, Active = true},
                new CourseSelectionOwing{StudentID=3,CourseID=3,Owing=1000.00, Active = true},
                new CourseSelectionOwing{StudentID=3,CourseID=4,Owing=-300.00, Active = true},
            };
            foreach (CourseSelectionOwing c in courseSelectionOwings)
            {
                context.CourseSelectionOwings.Add(c);
            }
            context.SaveChanges();
        }
    }
}
