using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BugTracker.Data;
using BugTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BugTracker.Controllers
{
    [Authorize]     
    public class TicketController : Controller
    {
        private ApplicationDbContext ticketContext;

        public TicketController (ApplicationDbContext appdb)
        {
            ticketContext = appdb;

        }
        public IActionResult Index()
        {
            List<Ticket> tickets = ticketContext.Tickets.ToList();
            return View(tickets);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticketContext.Tickets.Add(ticket);
                ticketContext.SaveChanges();

                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Ticket tic = ticketContext.Tickets.Find(id);
            return View(tic);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticketContext.Tickets.Update(ticket);
                ticketContext.SaveChanges();

                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Detail(int id)
        {
            Ticket ticket;

            if (ticketContext.Tickets.Any() &&
                ticketContext.Tickets.Any(x => x.TicketId == id))
            {
                ticket = ticketContext.Tickets.Single(x => x.TicketId == id);
            }
            else
            {
                ticket = null;
            }

            return View(ticket);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Ticket ticket = ticketContext.Tickets.Find(id);
            return View(ticket);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult Delete_post(int id)
        {
            Ticket ticket = ticketContext.Tickets.Find(id);
            ticketContext.Tickets.Remove(ticket);
            ticketContext.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}