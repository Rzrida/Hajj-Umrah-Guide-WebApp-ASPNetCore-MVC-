using Microsoft.AspNetCore.Mvc;
using HajjGuide.Models;
using System;
using System.Data.SqlClient;
using System.Text;
using System.Collections.Generic;

namespace HajjGuide.Controllers
{
    public class ChecklistItems : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

 [HttpPost]
        public IActionResult AddItem(ChecklistModel model)
        {
            byte[] userIdBytes = HttpContext.Session.Get("LoggedUserId");
            int userId = 0;
            if (userIdBytes != null)
            {
                string userIdString = Encoding.UTF8.GetString(userIdBytes);
                userId = int.Parse(userIdString);

                // Now you have the user's ID (userId) available for use.
            }
            string constr = @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=HajjGuide;Data Source=localhost";

            try
            {
                using (SqlConnection con = new SqlConnection(constr))
                {
                    con.Open();



                    string insertQuery = "INSERT INTO chkitems ([desc], ischecked, userid) " +
                        "VALUES (@Description,' 1', @userId) ";
                    using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@Description",model.items);
                        cmd.Parameters.AddWithValue("@userId", userId);
                        cmd.ExecuteNonQuery();
                    } 
                }

                return RedirectToAction("Index", "Checklist"); // Redirect to the Checklist Index action
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return RedirectToAction("Index"); // Redirect to a suitable action in case of error
            }
        }
    }
}
