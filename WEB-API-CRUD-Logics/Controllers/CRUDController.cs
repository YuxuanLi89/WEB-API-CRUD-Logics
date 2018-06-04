using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WEB_API_CRUD_Logics.Models;

namespace WEB_API_CRUD_Logics.Controllers
{
    public class CRUDController : ApiController
    {
        private CSharpCornerEntities db = new CSharpCornerEntities();
        // GET api/GetEmployees  
        [ResponseType(typeof(IEnumerable<Employee>))]
        [Route("api/GetEmployees")]
        public IQueryable<Employee> GetEmployees()
        {
            return db.Employees;
        }
        // GET api/CRUD/5  
        [ResponseType(typeof(Employee))]
        [Route("api/GetEmployee")]
        public IHttpActionResult GetEmployee(long id)
        {
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }
        // PUT api/CRUD/5  
        [Route("api/PutEmployee")]
        public IHttpActionResult PutEmployee(long id, Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != employee.ID)
            {
                return BadRequest();
            }
            db.Entry(employee).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return StatusCode(HttpStatusCode.NoContent);
        }
        // POST api/CRUD  
        [Route("api/PostEmployee")]
        [ResponseType(typeof(Employee))]
        public IHttpActionResult PostEmployee(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Employees.Add(employee);
            db.SaveChanges();
            return CreatedAtRoute("DefaultApi", new
            {
                id = employee.ID
            }, employee);
        }
        // DELETE api/CRUD/5  
        [Route("api/DeleteEmployee")]
        [ResponseType(typeof(Employee))]
        public IHttpActionResult DeleteEmployee(long id)
        {
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }
            db.Employees.Remove(employee);
            db.SaveChanges();
            return Ok(employee);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        [Route("api/EmployeeExists")]
        private bool EmployeeExists(long id)
        {
            return db.Employees.Count(e => e.ID == id) > 0;
        }
    }
}