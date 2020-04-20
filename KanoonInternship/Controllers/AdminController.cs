using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using KanoonInternship.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace KanoonInternship.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Tables(string table)
        {
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

                    List.Add(new UserInfo
                    {
                        Id = dr[0].ToString(),
                        UserName = dr[1].ToString(),
                        FirstName = dr[2].ToString(),
                        LastName = dr[3].ToString(),
                        ActiveState = dr[4].ToString(),
                        IsAdmin = dr[5].ToString(),
                        IsBanned = dr[6].ToString(),
                        BanUntil = dr[7].ToString()
                    });
                }
            }

            var r = Json(new { data = List });
            return r;
        }

        #endregion
    }
}