using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using First.Models;

namespace First.DAL
{
    public class SchoolInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<SchoolContext>
    {
        protected override void Seed(SchoolContext context)
        {
            var students = new List<Student>
            {
                new Student { FirstMidName = "Salam", LastName = "Ullah", EnrollmentDate = DateTime.Parse("2022-02-02") },
                new Student { FirstMidName = "Borkot", LastName = "Miah", EnrollmentDate = DateTime.Parse("2019-02-02") },
                new Student { FirstMidName = "Kuddus", LastName = "Fahim", EnrollmentDate = DateTime.Parse("2022-02-02") },
                new Student { FirstMidName = "Rohim", LastName = "Marfi", EnrollmentDate = DateTime.Parse("2022-02-02") },
                new Student { FirstMidName = "Mofis", LastName = "Arzu", EnrollmentDate = DateTime.Parse("2022-02-02") },
                new Student { FirstMidName = "Roshid", LastName = "Shahid", EnrollmentDate = DateTime.Parse("2022-02-02") },
                new Student { FirstMidName = "Hamid", LastName = "Jubba", EnrollmentDate = DateTime.Parse("2022-02-02") },
                new Student { FirstMidName = "Kamal", LastName = "Khan", EnrollmentDate = DateTime.Parse("2022-02-02") }
            };
            students.ForEach(s => context.Students.Add(s));
            context.SaveChanges();


            var courses = new List<Course>
            {
                new Course{CourseID=1050, Title="Chemistry", Credits=3,},
                new Course{CourseID=1264, Title="Microeconomics",Credits=3},
                new Course{CourseID=3625, Title="Accounting",Credits=4},
                new Course{CourseID=1258, Title="Calculus",Credits=3},
                new Course{CourseID=3691, Title="Composition",Credits=4},
                new Course{CourseID=4862, Title="Literature",Credits=3},
                new Course{CourseID=7519, Title="Accountin",Credits=4}
            };
            courses.ForEach(s => context.Courses.Add(s));
            context.SaveChanges();


            var enrollments = new List<Enrollment>
            {
                new Enrollment{StudentID=1, CourseID=1050, Grade=Grade.A},
                new Enrollment{StudentID=1, CourseID=1050, Grade=Grade.C},
                new Enrollment{StudentID=1, CourseID=1050, Grade=Grade.B},
                new Enrollment{StudentID=2, CourseID=1050, Grade=Grade.B},
                new Enrollment{StudentID=2, CourseID=1050, Grade=Grade.F},
                new Enrollment{StudentID=2, CourseID=1050, Grade=Grade.F},
                new Enrollment{StudentID=3, CourseID=1050},
                new Enrollment{StudentID=4, CourseID=1050},
                new Enrollment{StudentID=4, CourseID=1050, Grade=Grade.F},
                new Enrollment{StudentID=5, CourseID=1050, Grade=Grade.C},
                new Enrollment{StudentID=6, CourseID=1050},
                new Enrollment{StudentID=7, CourseID=1050, Grade=Grade.A}
            };
            enrollments.ForEach(s => context.Enrollments.Add(s));
            context.SaveChanges();

        }
    }
}