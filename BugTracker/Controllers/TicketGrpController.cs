using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using BugTracker.Data;
using BugTracker.Models;
using Microsoft.AspNetCore.Mvc;

namespace BugTracker.Controllers
{
    public class TicketGrpController : Controller
    {
        private ApplicationDbContext Context;

        public TicketGrpController(ApplicationDbContext context)
        {
            Context = context;
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
        public IActionResult CreateNew(Ticket tick)
        {
            if (ModelState.IsValid)
            {
                Context.Tickets.Add(tick);
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
            //return RedirectToAction("Edit", new { id = ticket.TicketId });
        }

        public IActionResult Detail (int id)
        {
            Ticket ticket = Context.Tickets.Find(id);
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
            Context.SaveChanges();
            return RedirectToAction("Index", new { id = ticket.GroupId });
        }
    }
}