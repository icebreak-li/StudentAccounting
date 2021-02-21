using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentAccounting.Data;
using StudentAccounting.Models;

namespace StudentAccounting.Controllers
{
    public class CourseSelectionsController : Controller
    {
        private readonly AccountingContext _context;

        public CourseSelectionsController(AccountingContext context)
        {
            _context = context;
        }

        // GET: CourseSelections
        public async Task<IActionResult> Index()
        {
            return View(await _context.Students.Where(m => m.Active == true).ToListAsync());
        }

        // GET: CourseSelections/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await GetStudentWithCourseSelections(id.Value);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: CourseSelections/Create
        public async Task<IActionResult> Create(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await GetStudentWithCourseSelections(id.Value);
            if (student == null)
            {
                return NotFound();
            }

            var allCourses = await _context.Courses.Where(m => m.Active == true).ToListAsync();
            var availableCourses = new List<Course>();
            // Get available courses for enrollment
            foreach(var course in allCourses)
            {
                if (!student.CourseSelections.Any(m => (m.CourseID == course.CourseID && m.Active)))
                    availableCourses.Add(course);
            }

            ViewData["CourseID"] = new SelectList(availableCourses, "CourseID", "CourseName");
            
            return View(new CourseSelection { Student = student});
        }

        // POST: CourseSelections/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentID,CourseID,Paid,Active")] CourseSelection courseSelection)
        {
            if (ModelState.IsValid)
            {
                // Define an object of CourseSelectionOwing and set the details
                CourseSelectionOwing courseSelectionOwing = new CourseSelectionOwing();
                courseSelectionOwing.StudentID = courseSelection.StudentID;
                courseSelectionOwing.CourseID = courseSelection.CourseID;
                courseSelectionOwing.Active = true;

                var course = await _context.Courses.FindAsync(courseSelection.CourseID);
                if (course == null)
                    courseSelectionOwing.Owing = 0;
                else
                    courseSelectionOwing.Owing = course.Cost;

                // Add entry for course selection
                _context.Add(courseSelection);
                // Add entry for course selection owing
                _context.Add(courseSelectionOwing);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Create), courseSelection.StudentID);
            }

            if (courseSelection.StudentID == 0)
            {
                return NotFound();
            }
            var student = await GetStudentWithCourseSelections(courseSelection.StudentID);
            if (student == null)
            {
                return NotFound();
            }
            
            var allCourses = await _context.Courses.Where(m => m.Active == true).ToListAsync();
            var availableCourses = new List<Course>();
            // Get available courses for enrollment
            foreach (var course in allCourses)
            {
                if (!student.CourseSelections.Any(m => (m.CourseID == course.CourseID && m.Active)))
                    availableCourses.Add(course);
            }

            ViewData["CourseID"] = new SelectList(availableCourses, "CourseID", "CourseName");

            return View(new CourseSelection { Student = student });
        }

        // GET: CourseSelections/Pay/5
        public async Task<IActionResult> Pay(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await GetStudentWithCourseSelections(id.Value);
            if (student == null)
            {
                return NotFound();
            }

            return View(new CourseSelection { Student = student });
        }

        // POST: CourseSelections/Pay/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Pay(int id, [Bind("ID,StudentID,CourseID,Paid,Active")] CourseSelection courseSelection)
        {
            if (id != courseSelection.ID)
            {
                return NotFound();
            }

            // Get the object of CourseSelection to be updated
            var courseSelectionToBeUpdated = await _context.CourseSelections.FindAsync(id);
            // Get the object of CourseSelectionOwing to be updated
            var courseSelectionOwingToBeUpdated =  _context.CourseSelectionOwings.FirstOrDefault(m => m.CourseID == courseSelection.CourseID && m.StudentID == courseSelection.StudentID && m.Active == true);

            if (ModelState.IsValid)
            {
                courseSelectionToBeUpdated.Paid += courseSelection.Paid;
                courseSelectionOwingToBeUpdated.Owing -= courseSelection.Paid;

                try
                {
                    // Update the entry of course selection
                    _context.Update(courseSelectionToBeUpdated);
                    // Update the entry of course selection owing
                    _context.Update(courseSelectionOwingToBeUpdated);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseSelectionExists(courseSelection.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            var student = await GetStudentWithCourseSelections(courseSelectionToBeUpdated.StudentID);
            if (student == null)
            {
                return NotFound();
            }

            return View(new CourseSelection { Student = student });
        }

        // GET: CourseSelections/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await GetStudentWithCourseSelections(id.Value);
            if (student == null)
            {
                return NotFound();
            }

            return View(new CourseSelection { Student = student });
        }

        // POST: CourseSelections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Get the object of CourseSelection to be deleted
            var courseSelectionToBeDeleted = await _context.CourseSelections.FindAsync(id);
            // Get the object of CourseSelectionOwing to be deleted
            var courseSelectionOwingToBeDeleted = _context.CourseSelectionOwings.FirstOrDefault(m => 
                            m.CourseID == courseSelectionToBeDeleted.CourseID 
                            && m.StudentID == courseSelectionToBeDeleted.StudentID 
                            && m.Active == true);

            courseSelectionToBeDeleted.Active = false;
            courseSelectionOwingToBeDeleted.Active = false;
            // Delete the entry of course selection (disable)
            _context.Update(courseSelectionToBeDeleted);
            // Delete the entry of course selection owing (disable)
            _context.Update(courseSelectionOwingToBeDeleted);
            await _context.SaveChangesAsync();

            var student = await GetStudentWithCourseSelections(courseSelectionToBeDeleted.StudentID);
            if (student == null)
            {
                return NotFound();
            }
            
            return View(new CourseSelection { Student = student });
        }

        private bool CourseSelectionExists(int id)
        {
            return _context.CourseSelections.Any(e => e.ID == id);
        }

        private async Task<Student> GetStudentWithCourseSelections(int id)
        {
            // Include course selection, course selection owing and course info for the student
            var student = await _context.Students
                .Include(s => s.CourseSelections)
                    .ThenInclude(c => c.Course)
                .Include(s => s.CourseSelectionOwings)
                    .ThenInclude(c => c.Course)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.StudentID == id);

            if (student != null)
            {
                // Remove inactive courses (deleted course selections)
                student.CourseSelections = student.CourseSelections.Where(m => m.Active == true).OrderBy(m => m.CourseID).ToList();
                student.CourseSelectionOwings = student.CourseSelectionOwings.Where(m => m.Active == true).OrderBy(m => m.CourseID).ToList();
            }

            return student;
        }
    }
}
