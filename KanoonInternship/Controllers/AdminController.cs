using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using KanoonInternship.Models;
using KanoonInternship.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace KanoonInternship.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> UserManager;

        public AdminController(UserManager<ApplicationUser> UserManager)
        {
            this.UserManager = UserManager;
        }

        public IActionResult Tables(string table)
        {
            if (table == null)
                table = "All";

            return View(table.ToCharArray());
        }

        #region API Calls

        [HttpGet]
        public IActionResult GetTables(string table)
        {
            List<UserInfo> List = new List<UserInfo>();

            // Open connection to the database
            string conString = "Server=(LocalDb)\\MSSQLLocalDB;Database=KanoonInternship";

            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();

                // Set up a command with the given query and associate this with the current connection.
                using SqlCommand cmd = new SqlCommand(
                    "SELECT Id, UserName, FirstName, LastName, ActiveState," +
                    " IsAdmin, IsBanned, BanUntil from dbo.AspNetUsers"
                    , con);
                using IDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (table.StartsWith("Waiting"))
                    {
                        if (dr[4].ToString() != "0")
                            continue;
                    }
                    else if (table.StartsWith("Banned"))
                    {
                        if (dr[6].ToString() != "True")
                            continue;
                    }

                    string BannedUntil;
                    if (dr[6].ToString() == "False")
                        BannedUntil = "Not Banned";
                    else
                        BannedUntil = dr[7].ToString().Split(" ")[0];

                    List.Add(new UserInfo
                    {
                        Id = dr[0].ToString(),
                        UserName = dr[1].ToString(),
                        FirstName = dr[2].ToString(),
                        LastName = dr[3].ToString(),
                        ActiveState = dr[4].ToString(),
                        IsAdmin = dr[5].ToString(),
                        BannedUntil = BannedUntil
                    });
                }
            }

            var r = Json(new { data = List });
            return r;
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var AppUser = await UserManager.FindByIdAsync(id);

            if (AppUser == null)
                return Json(new { success = false, message = "Error while deleting user." });

            var res = await UserManager.DeleteAsync(AppUser);

            if (res.Succeeded)
                return Json(new { success = true, message = "User deleted Successfully." });

            return Json(new { success = false, message = "Error while deleting user." });
        }

        #endregion

        #region Edit User

        // to Active, Reject or Unban a user
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeProperty(string id, string which)
        {
            var AppUser = await UserManager.FindByIdAsync(id);

            if (AppUser == null)
                return Json(new { success = false, message = "An error occured. Please try again." });

            if (which == "Active")
                AppUser.ActiveState = 1;
            else if (which == "Reject")
                AppUser.ActiveState = -1;
            else
                AppUser.IsBanned = false;

            var res = await UserManager.UpdateAsync(AppUser);

            if (res.Succeeded)
                return Json(new { success = true, message = "Operation has done Successfully." });

            return Json(new { success = false, message = "An error occured. Please try again." });
        }

        public async Task<IActionResult> Edit(string id)
        {
            var AppUser = await UserManager.FindByIdAsync(id);

            if (AppUser == null)
                return NotFound();

            return View(new EditViewModel
            {
                Id = AppUser.Id,
                FirstName = AppUser.FirstName,
                LastName = AppUser.LastName,
                UserName = AppUser.UserName,
                BanUntil = AppUser.BanUntil
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditViewModel model)
        {
            var AppUser = await UserManager.FindByIdAsync(model.Id);
            if (ModelState.IsValid && AppUser != null)
            {
                AppUser.FirstName = model.FirstName;
                AppUser.LastName = model.LastName;
                AppUser.UserName = model.UserName;

                if (model.BanUntil > DateTime.Today)
                {
                    AppUser.IsBanned = true;
                    AppUser.BanUntil = model.BanUntil;
                }

                var res = await UserManager.UpdateAsync(AppUser);

                if (res.Succeeded)
                    return RedirectToAction("Tables");

                foreach (var Error in res.Errors)
                    ModelState.AddModelError("", Error.Description);
            }
            return View(model);
        }

        #endregion
    }
}