using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FIT5032_fitness.Models;
using Microsoft.AspNet.Identity;

namespace FIT5032_fitness.Controllers
{
    public class EditorsController : Controller
    {
        private EditorModels db = new EditorModels();

        // GET: Editors
        public ActionResult Index()
        {
            return View(db.Editors.ToList());
        }


        [Authorize(Roles = "Customer, Admin")]
        public ActionResult MyBlog()
        {


            var currentUserId = User.Identity.GetUserId();
            return View(db.Editors.Where(m => m.UserId == currentUserId).ToList());
        }

        [Authorize(Roles = "Admin")]
        public ActionResult BlogList()
        {
            return View(db.Editors.ToList());
        }

        // GET: Editors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Editor editor = db.Editors.Find(id);
            if (editor == null)
            {
                return HttpNotFound();
            }
            return View(editor);
        }

        // GET: Editors/Create
        [Authorize(Roles = "Customer, Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Editors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title,Description")] Editor editor, HttpPostedFileBase postedFile)
        {
            ModelState.Clear();
            var myUniqueFileName = string.Format(@"{0}", Guid.NewGuid());
            editor.path = myUniqueFileName;
            TryValidateModel(editor);

            if (ModelState.IsValid)
            {
                if(postedFile!= null)
                {
                    string serverPath = Server.MapPath("~/Uploads/");
                    string fileExtension = Path.GetExtension(postedFile.FileName);
                    string filePath = editor.path + fileExtension;
                    editor.path = filePath;
                    postedFile.SaveAs(serverPath + editor.path);
                }
               

                //Get Author Name

                var userid = User.Identity.GetUserId();
                var authorName = new AccountController().GetFirstNameAndLastName(userid);

                editor.authorName = authorName;
                editor.UserId = userid;


                db.Editors.Add(editor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(editor);
        }

        [Authorize(Roles = "Admin,Customer")]
        // GET: Editors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Editor editor = db.Editors.Find(id);
            if (editor == null)
            {
                return HttpNotFound();
            }
            Session["ForumId"] = editor.ForumId;
            return View(editor);
        }

        // POST: Editors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Title,Description")] Editor editor, HttpPostedFileBase postedFile)
        {
            if (ModelState.IsValid)
            {

                var currentEditor = db.Editors.Find(Session["ForumId"]);

                if(postedFile !=null)
                {
                    //Update path
                    string serverPath = Server.MapPath("~/Uploads/");
                    string fileExtension = Path.GetExtension(postedFile.FileName);
                    string filePath = editor.path + fileExtension;
                    editor.path = filePath;
                    postedFile.SaveAs(serverPath + editor.path);
                }      

                //Set change for author name and user id
                editor.UserId = currentEditor.UserId;
                editor.authorName  = currentEditor.authorName;
                editor.ForumId = currentEditor.ForumId;

                db.Editors.AddOrUpdate(editor);
                //db.Entry(editor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(editor);
        }


        [Authorize(Roles = "Admin,Customer")]
        // GET: Editors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Editor editor = db.Editors.Find(id);
            if (editor == null)
            {
                return HttpNotFound();
            }
            return View(editor);
        }

        // POST: Editors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Editor editor = db.Editors.Find(id);
            db.Editors.Remove(editor);
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
