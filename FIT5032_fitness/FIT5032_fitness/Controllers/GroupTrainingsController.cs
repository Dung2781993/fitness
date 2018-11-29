using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using FIT5032_fitness.Models;
using Microsoft.AspNet.Identity;

namespace FIT5032_fitness.Controllers
{
    public class GroupTrainingsController : Controller
    {
        private GroupModel db = new GroupModel();

        // GET: GroupTrainings
        public ActionResult Index()
        {
            return View();
        }

        // GET: GroupTrainings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GroupTraining groupTraining = db.GroupTrainings.Find(id);
            if (groupTraining == null)
            {
                return HttpNotFound();
            }
            return View(groupTraining);
        }


        //[Authorize(Roles = "Customer, Admin")]
        public ActionResult BookingClass(String date)
        {
            var userId = User.Identity.GetUserId();
            var userEmail = new AccountController().GetUserEmailFromId(userId);

            TempData["Email"] = userEmail;
            if(userEmail == null)
            {
                if (null == date)
                    date = DateTime.Now.ToString();
                GroupTraining e = new GroupTraining();
                DateTime convertedDate = DateTime.Parse(date);
                e.StartDate = convertedDate;
                return View();
            }
            var trainingClass = db.GroupTrainings.Where(m => m.Email == userEmail & m.StartDate > DateTime.Now).ToList();
            return View(trainingClass);      
        }

        [Authorize(Roles = "Admin")]
        public ActionResult BookingClassList(String date)
        {

            //var trainingClass = db.GroupTrainings.Where(e => e.StartDate > DateTime.Now).ToList();
            var trainingClass = db.GroupTrainings.ToList();
            return View(trainingClass);
        }

        //[Authorize(Roles = "Customer, Admin")]
        // GET: GroupTrainings/Create
        public ActionResult Create(String date)
        {

            if (date == null)
            {
                date = DateTime.Now.ToString();
            }
            GroupTraining e = new GroupTraining();
            DateTime convertedDate = DateTime.Parse(date);
            var userId = User.Identity.GetUserId();
            if(userId != null)
            {
                var userEmail = new AccountController().GetUserEmailFromId(userId);
                e.Email = userEmail;
            }
            e.StartDate = convertedDate;
            return View(e);
        }


        // POST: GroupTrainings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( GroupTraining groupTraining)
        {
            try
            {
                var date = String.Format("{0:dd/MM/yyyy}", groupTraining.StartDate);
                var bookingDate = DateTime.Parse(date).AddHours(7);
                var checkBooking = db.GroupTrainings.Where(m => m.Email == groupTraining.Email & m.StartDate == bookingDate).ToList().Count;
                if(checkBooking == 1)
                {
                    TempData["Error"] = "You already booked for training group";
                    ViewBag.Error = TempData["Error"];
                    return View(groupTraining);
                }
                else
                {
                    if (ModelState.IsValid)
                    {                        
                        groupTraining.StartDate = bookingDate;
                        db.GroupTrainings.Add(groupTraining);
                        db.SaveChanges();
                        TempData["Success"] = "Thank you for your booking, see you in class";
                        ViewBag.Success = TempData["Success"];

                        //Create Email 
                        EmailSender es = new EmailSender();
                        String toEmail = groupTraining.Email;
                        String subject = "Booking Confirmation";
                        String contents = "Thank you for your group training booking. See you on " + bookingDate;
                        es.Send(toEmail, subject, contents);
                        ModelState.Clear();
                        return RedirectToAction("BookingClass");
                    }
                    return View(groupTraining);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                TempData["Error"] = "Oops! Error with form submission";
                return View(groupTraining);
            }
        }

        // GET: GroupTrainings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GroupTraining groupTraining = db.GroupTrainings.Find(id);
            if (groupTraining == null)
            {
                return HttpNotFound();
            }
            return View("BookingClass");
        }

        // POST: GroupTrainings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GroupId,UserId,Title,StartDate,EndDate")] GroupTraining groupTraining)
        {
            if (ModelState.IsValid)
            {
                db.Entry(groupTraining).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(groupTraining);
        }

        // GET: GroupTrainings/Delete/5
        [Authorize(Roles = "Customer, Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GroupTraining groupTraining = db.GroupTrainings.Find(id);
            if (groupTraining == null)
            {
                return HttpNotFound();
            }
            return View(groupTraining);
        }

        // POST: GroupTrainings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GroupTraining groupTraining = db.GroupTrainings.Find(id);
            db.GroupTrainings.Remove(groupTraining);
            db.SaveChanges();
            if(User.IsInRole("Admin"))
            {
                return RedirectToAction("BookingClassList");
            }
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
