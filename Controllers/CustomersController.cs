using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using exa.Models;

namespace exa.Controllers
{
    public class CustomersController : Controller
    {
        private DemoDBEntities db = new DemoDBEntities();

        // GET: Customers
        public ActionResult Index()
        {
            return View(db.Customer.Where(x=>x.IsActive==true).ToList());
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customer.Where(x => x.CustomerID==id && x.IsActive==true).FirstOrDefault();
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }
        public ActionResult InactiveCustomers()
        {
            return View(db.Customer.Where(x => x.IsActive == false).ToList());
        }
        // GET: Customers/Create
        public ActionResult Create()
        {
            Customer model=new Customer();

            return View(model);
        }

        // POST: Customers/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CustomerID,DocumentNumber,DocumentType,Name")] Customer customer)
        {
            if (ModelState.IsValid)
            { customer.IsActive = true;
                db.Customer.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customer);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customer.Where(x => x.CustomerID == id && x.IsActive == true).FirstOrDefault();
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CustomerID,DocumentNumber,DocumentType,Name")] Customer customer)
        {
            if (ModelState.IsValid)
            {

                var customermodify =db.Customer.Where(x => x.CustomerID == customer.CustomerID).FirstOrDefault();
                db.Entry(customermodify).State = EntityState.Modified;
                customermodify.DocumentType=customer.DocumentType;
                customermodify.DocumentNumber=customer.DocumentNumber;
                customermodify.Name=customer.Name;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customer.Where(x=>x.CustomerID ==id && x.IsActive == true).FirstOrDefault();
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customer.Find(id);
            // db.Customer.Remove(customer);
            db.Entry(customer).State = EntityState.Modified;
            customer.IsActive = false;
            db.SaveChanges();
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
    }
}
