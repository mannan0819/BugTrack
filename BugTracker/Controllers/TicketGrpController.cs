using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using BugTracker.Data;
using BugTracker.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Web;
using Microsoft.Extensions.Hosting;

namespace BugTracker.Controllers
{
    public class TicketGrpController : Controller
    {
        private ApplicationDbContext Context;
        private IHostingEnvironment Env;

        public TicketGrpController(ApplicationDbContext context, 
                                   IHostingEnvironment env)
        {
            Context = context;
            Env = env;
        }

        public IActionResult Index(int id)
        {
            ViewBag.GrpName = Context.TicketGroup.Find(id).name;
            ViewBag.id = id;          
            return View(Context.Tickets.Where(a => a.GroupId == id).ToList()); 
        }

        [HttpGet]
        public IActionResult CreateNew(int id)
        {
            ViewBag.GrpName = Context.TicketGroup.Find(id).name;
            ViewBag.GrId = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateNew(TicketViewModel tick)
        {
            if (ModelState.IsValid)
            {
                Ticket ticket = new Ticket
                {
                    Title = tick.Title,   
                    Slug = tick.Slug,
                    Author = tick.Author, 
                    DateCreated = tick.DateCreated, 
                    DateEdited = tick.DateEdited, 
                    Status = tick.Status,
                    Description = tick.Description,
                    GroupId = tick.GroupId
                 };
                if (ticket.Description == null) ticket.Description = "";

                if (tick.upFile != null)
                {
                    string filename = tick.upFile.FileName;
                    string folderPath = Path.Combine(Env.ContentRootPath, "wwwroot", "uploads");
                    string uniqFile = Guid.NewGuid().ToString() + "_" + filename;
                    string filePath = Path.Combine(folderPath, uniqFile);
                    tick.upFile.CopyTo(new FileStream(filePath, FileMode.Create));
                    ticket.FileName = uniqFile;


                    //Checking if image file
                    string Filetype = tick.upFile.ContentType;
                    int index = Filetype.IndexOf("/");
                    if (index != -1) Filetype = Filetype.Substring(0, index);
                    ticket.isImg = Filetype.Equals("image");
                  
                }
                Context.Tickets.Add(ticket);
                Context.SaveChanges();
                return RedirectToAction("Index", new { id = tick.GroupId });
            }
            return View();
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Ticket toEdit = Context.Tickets.Find(id);
            ViewBag.GrpName = (Context.TicketGroup.Find(toEdit.GroupId)).name;
            return View(toEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                Context.Tickets.Update(ticket);
                Context.SaveChanges();

                return RedirectToAction("Index", new { id = ticket.GroupId });
            }
            ViewBag.GrpName = (Context.TicketGroup.Find(ticket.GroupId)).name;
            return View();
        }

        public IActionResult img()
        {
            return View();
        }

        public IActionResult Detail (int id)
        {
            Ticket ticket = Context.Tickets.Find(id);
            if (ticket.FileName != null)
            {
                string fileStr = ticket.FileName.ToString();
                //fileStr = fileStr.Substring(fileStr.IndexOf("_") + 1);
                ViewBag.FileNa = "/uploads"+ "/" + fileStr;
                
            }

            ViewBag.GrpName = Context.TicketGroup.Find(ticket.GroupId).name;
            return View(ticket);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Ticket ticket = Context.Tickets.Find(id);
            ViewBag.GrpName = Context.TicketGroup.Find(ticket.GroupId).name;
            return View(ticket);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult Delete_post(int id)
        {
            Ticket ticket = Context.Tickets.Find(id);
            Context.Tickets.Remove(ticket);
            if (ticket.FileName != null)
            {
               // File.Delete();
               // IFileProvider physicalFileProvider = new PhysicalFileProvider(@"D:\DeleteMyContentsPlease\");

            }
            Context.SaveChanges();
            return RedirectToAction("Index", new { id = ticket.GroupId });
        }
    }
}