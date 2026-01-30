using HajjGuide.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;

namespace HajjGuide.Controllers
{
    public class DuaRequest : Controller
    {

        public IActionResult Index()
        {
            List<DuasModel> ls4 = new List<DuasModel>();
            string constr = @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=HajjGuide;Data Source=localhost";


            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string q = "SELECT id, [desc], DescUrdu FROM Step"; 

                using (SqlCommand cmd = new SqlCommand(q, con))
                {
                    using (SqlDataReader sda = cmd.ExecuteReader())
                    {
                        while (sda.Read())
                        {
                            var v = new DuasModel()
                            {
                                stepid = sda.GetInt32(0),
                                Description = sda.GetString(1),
                                UrduDesc = sda.GetString(2),
                            };

                            ls4.Add(v);
                        }
                    }
                }
            }

            return View(ls4);
        }




        [HttpPost]
        public IActionResult Save(DuasModel model)
        {
            //if (ModelState.IsValid)
            //{
            byte[] userIdBytes = HttpContext.Session.Get("LoggedUserId");
            int userId = 0;
            if (userIdBytes != null)
            {
                string userIdString = Encoding.UTF8.GetString(userIdBytes);
                userId = int.Parse(userIdString);

                // Now you have the user's ID (userId) available for use.
            }

            string constr = @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=HajjGuide;Data Source=localhost";

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                Console.WriteLine($"stepid to insert: {model.stepid}");

                string q = "INSERT INTO DuaRequest ([desc], stepId, isChecked, Applicant, userid) " +
                           "VALUES (@Desc, @StepId,'1',@Applicant, @userId )";

                using (SqlCommand cmd = new SqlCommand(q, con))
                {
                    cmd.Parameters.AddWithValue("@Desc", model.Desc ?? model.Description ?? "");
                    cmd.Parameters.AddWithValue("@StepId", model.stepid);
                    cmd.Parameters.AddWithValue("@Applicant", model.Applicant ?? "");
                    cmd.Parameters.AddWithValue("@userId", userId);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Data saved successfully");
                    }
                    else
                    {
                        Console.WriteLine("Data not saved");
                    }
                }
            }
            // }

            return RedirectToAction("Index");
        }




        public IActionResult UpdateChecklist(Dictionary<int, int> Prayers)
        {

            string constr = @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=HajjGuide;Data Source=DESKTOP-QK26P6V\MSSQLSERVER01";

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();

                foreach (var kvp in Prayers)
                {
                    int itemId = kvp.Key;
                    int IsChecked = kvp.Value;

                    string updateQuery = "UPDATE Dua SET IsChecked = @IsChecked WHERE id = @Id";
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(updateQuery, con))
                        {
                            cmd.Parameters.AddWithValue("@IsChecked", IsChecked);
                            cmd.Parameters.AddWithValue("@Id", itemId);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }

            return RedirectToAction("Index");
        }
        public IActionResult DuaRequests(int stepid) // Pass stepid as a parameter
        {
            byte[] userIdBytes = HttpContext.Session.Get("LoggedUserId");
            int userId = 0;
            if (userIdBytes != null)
            {
                string userIdString = Encoding.UTF8.GetString(userIdBytes);
                userId = int.Parse(userIdString);

                // Now you have the user's ID (userId) available for use.
            }
            List<DuasModel> Prayers = GeRequestedPrayerFromDatabase(stepid);
            return View(Prayers);
        }

        private List<DuasModel> GeRequestedPrayerFromDatabase(int stepid)
        {
            byte[] userIdBytes = HttpContext.Session.Get("LoggedUserId");
            int userId = 0;
            if (userIdBytes != null)
            {
                string userIdString = Encoding.UTF8.GetString(userIdBytes);
                userId = int.Parse(userIdString);

                // Now you have the user's ID (userId) available for use.
            }
      
            List<DuasModel> ls14 = new List<DuasModel>();
            string constr = @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=HajjGuide;Data Source=DESKTOP-QK26P6V\MSSQLSERVER01";

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string q = " SELECT * FROM DuaRequest WHERE userid = @userId"; // Filter by stepid
                using (SqlCommand cmd = new SqlCommand(q, con))
                {
                    cmd.Parameters.AddWithValue("@userId", userId); // Set the parameter value
                    using (SqlDataReader sda = cmd.ExecuteReader())
                    {
                        while (sda.Read())
                        {
                            var v = new DuasModel()
                            {
                                
                                Desc = sda.GetString(1),
                                stepid = sda.GetInt32(2),
                                Applicant = sda.GetString(4),
                            };

                            ls14.Add(v);
                        }
                    }
                }
            }

            return ls14;
        }


    }
}





