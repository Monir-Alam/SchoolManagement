
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using First.DAL;
using First.Models;
using System.Data.Entity.Infrastructure;
using First.Models.Base;

namespace First.Controllers
{
    public class DepartmentController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: Department
        public async Task<ActionResult> Index()
        {
            var departments = db.Departments.Include(d => d.Administrator);
            return View(await departments.ToListAsync());
        }

        // GET: Department/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            //Department department = await db.Departments.FindAsync(id);

            string query = "SELECT * FROM Department WHERE DepartmentID = @p0";
            Department department = await db.Departments.SqlQuery(query, id).SingleOrDefaultAsync();

            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // GET: Department/Create
        public ActionResult Create()
        {
            ViewBag.InstructorID = new SelectList(db.Instructors, "ID", "FullName");
            return View();
        }

        // POST: Department/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "DepartmentID,Name,Budget,StartDate,InstructorID")] Department department)
        {
            if (ModelState.IsValid)
            {
                db.Departments.Add(department);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.InstructorID = new SelectList(db.Instructors, "ID", "FullName", department.InstructorID);
            return View(department);
        }

        // GET: Department/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = await db.Departments.FindAsync(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            ViewBag.InstructorID = new SelectList(db.Instructors, "ID", "FullName", department.InstructorID);
            return View(department);
        }

        // POST: Department/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "DepartmentID, Name, Budget, StartDate,RowVersion, InstructorID")] Department department)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ValidateOneAdministratorAssignmentPerInstructor(department);
                }


                if (ModelState.IsValid)
                {
                    db.Entry(department).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            catch(DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.Single();
                var clientValues = (Department)entry.Entity;
                var databaseEntry = entry.GetDatabaseValues();

                if (databaseEntry == null)
                {
                    ModelState.AddModelError(string.Empty, "Unable to save changes. The department was deleted by another user.");
                }
                else
                {
                    var databaseValues = (Department)databaseEntry.ToObject();

                    if (databaseValues.Name != clientValues.Name) ModelState.AddModelError("Name", "Current value: " + databaseValues.Name);

                    if (databaseValues.Budget != clientValues.Budget) ModelState.AddModelError("Budget", "Current value: " + String.Format("{0:c}",
                        databaseValues.Budget));

                    if (databaseValues.StartDate != clientValues.StartDate) ModelState.AddModelError("StartDate", "Current value: " + String.Format("{0:c}",
                        databaseValues.StartDate));

                    if (databaseValues.InstructorID != clientValues.InstructorID) ModelState.AddModelError("InstructorID", "Current value: "
                        + db.Instructors.Find(databaseValues.InstructorID).FullName);

                    ModelState.AddModelError(string.Empty, "The record you attempted to edit " + "was modified by another user. The "
                        + "edit operation was canceled and the current values in the database "
                        + "have been displayed, If you still want to edit this recored, click "
                        + "the save button agai. Otherwise click the back to list hyperlink.");
                    department.RowVersion = databaseValues.RowVersion;
                }
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes. Try again.");
            }

            ViewBag.InstructorID = new SelectList(db.Instructors, "ID", "FullName", department.InstructorID);
            

            return View(department);
        }

        // GET: Department/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = await db.Departments.FindAsync(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // POST: Department/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Department department = await db.Departments.FindAsync(id);
            db.Departments.Remove(department);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }



        private void ValidateOneAdministratorAssignmentPerInstructor(Department department)
        {
            if (department.InstructorID != null)
            {
                Department duplicateDepartment = db.Departments
                    .Include("Administrator")
                    .Where(d => d.InstructorID == department.InstructorID)
                    .AsNoTracking()
                    .FirstOrDefault();

                if(duplicateDepartment!=null && duplicateDepartment.DepartmentID != department.DepartmentID)
                {
                    string errorMessage = string.Format("Instructor {0} {1} is already administrator of the {2} department.",
                        duplicateDepartment.Administrator.FirstMidName,
                        duplicateDepartment.Administrator.LastName,
                        duplicateDepartment.Name);
                    ModelState.AddModelError(string.Empty, errorMessage);
                        
                }
            }
        }
    }
}
