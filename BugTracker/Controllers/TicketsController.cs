using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BugTracker.Data;
using BugTracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BugTracker.Controllers.Tickets
{
    public class TicketsController : Controller
    {
        private ApplicationDbContext ticketscontext;
        public TicketsController (ApplicationDbContext context)
        {
            ticketscontext = context;
        }

        public IActionResult Index()
        {
            List<TicketGroup> ticketGrp = ticketscontext.TicketGroup.ToList();
            return View(ticketGrp);
        }

        [HttpGet]
        public IActionResult CreateTickets()
        {
            return View();
        }

        [HttpPost]  
        [ValidateAntiForgeryToken]
        public IActionResult CreateTickets(TicketGroup ticketGrp)
        {
            if (ModelState.IsValid)
            {
                ticketscontext.TicketGroup.Add(ticketGrp);
                ticketscontext.SaveChanges();

                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult ticket(int id)
        {
            return RedirectToAction("index", "ticketGrp", new { id = id });
        }

        public IActionResult ticketGrp(int id)
        {
            return View();
        }

       
    }
}