using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using KanoonInternship.Data;
using KanoonInternship.Models;
using KanoonInternship.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KanoonInternship.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;

        public UserController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult AddReport()
        {
            return View(new AddReportViewModel
            {
                EntranceTime = DateTime.Now,
                ExitTime = DateTime.Now,
                Date = DateTime.Today
            });
        }

        [HttpPost]
        public IActionResult AddAsync(AddReportViewModel model)
        {
            var Report = new Report
            {
                Writer = User.Identity.Name.ToString(),
                EntranceTime = model.EntranceTime,
                ExitTime = model.ExitTime,
                Date = model.Date,
                Text = model.Text
            };

            if (ModelState.IsValid)
            {
                _db.Add(Report);
                _db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(Report);
        }
    }
}