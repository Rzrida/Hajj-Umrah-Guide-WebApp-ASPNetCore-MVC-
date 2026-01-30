using HajjGuide.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace HajjGuide.Controllers
{
    public class Checklist : Controller
    {



        [HttpPost]
        public IActionResult UpdateChecklist(Dictionary<int, int> Prayers, int checklistId, int userid, int stepid)
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

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();

                foreach (var kvp in Prayers)
                {
                    int itemId = kvp.Key;
                    int isBookmark = kvp.Value;

                    string updateQuery = " UPDATE userchecklist SET isChecked = @IsChecked WHERE checklistid = @checklistid and userid = @userId";

                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(updateQuery, con))
                        {
                            cmd.Parameters.AddWithValue("@IsChecked", isBookmark);
                            cmd.Parameters.AddWithValue("@checklistid", itemId);
                            cmd.Parameters.AddWithValue("@UserId", userid);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }

            return RedirectToAction("Index", new { stepid });
        }

        public IActionResult Index(int stepId)
        {
            //List<ChecklistModel> checklist = GetChecklistFromDatabase();
            //return View(checklist);
        

            MyChecklist chklst = new MyChecklist();
            chklst.chklst = GetChecklistFromDatabase();
            chklst.chkitems = GetChecklistitemsFromDatabase();
            return View(chklst);
        }

        private List<ChecklistModel> GetChecklistFromDatabase()
        {
            byte[] userIdBytes = HttpContext.Session.Get("LoggedUserId");
            int userId = 0;
            if (userIdBytes != null)
            {
                string userIdString = Encoding.UTF8.GetString(userIdBytes);
                userId = int.Parse(userIdString);

                // Now you have the user's ID (userId) available for use.
            }
            List<ChecklistModel> ls4 = new List<ChecklistModel>();
            string constr = @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=HajjGuide;Data Source=localhost";

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string q = "SELECT uc.userid, c.[desc] AS ChecklistDesc,c.DescUrdu AS ChecklistDescUrdu,uc.checklistid,uc.isChecked FROM userchecklist uc JOIN Checklist c ON uc.checklistid = c.id JOIN Signup u ON uc.[userid] = u.SignupId where uc.userid=@userId";
                using (SqlCommand cmd = new SqlCommand(q, con))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    using (SqlDataReader sda = cmd.ExecuteReader())
                    {
                        while (sda.Read())
                        {
                            var v = new ChecklistModel()
                            {
                                UserId = sda.GetInt32(0),
                                Description = sda.GetString(1),
                                DescriptionUrdu = sda.GetString(2),
                                checklistid = sda.GetInt32(3),
                                IsChecked = sda.GetBoolean(4),
                  

                            };

                            ls4.Add(v);
                        }
                    }
                }
            }

            return ls4;
        }
        private List<ChecklistModel> GetChecklistitemsFromDatabase()
        {
            byte[] userIdBytes = HttpContext.Session.Get("LoggedUserId");
            int userId = 0;
            if (userIdBytes != null)
            {
                string userIdString = Encoding.UTF8.GetString(userIdBytes);
                userId = int.Parse(userIdString);

                // Now you have the user's ID (userId) available for use.
            }
            List<ChecklistModel> ls4 = new List<ChecklistModel>();
            string constr = @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=HajjGuide;Data Source=localhost";

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string q1 = "select* from chkitems where userid=@userId";
                using (SqlCommand cmd = new SqlCommand(q1, con))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    using (SqlDataReader sda = cmd.ExecuteReader())
                    {
                        while (sda.Read())
                        {
                            var v1 = new ChecklistModel()
                            {


                                items = sda.GetString(1),
                                //IsChecked = sda.GetBoolean(2),
                                //UserId=sda.GetInt32(3)

                            };

                            ls4.Add(v1);
                        }
                    }
                }
            }

            return ls4;
        }

    }
}


//using HajjGuide.Models;
//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Collections.Generic;
//using System.Data.SqlClient;
//using System.Text;

//namespace HajjGuide.Controllers
//{
//    public class ChecklistController : Controller
//    {
//        [HttpPost]
//        public IActionResult UpdateChecklist(Dictionary<int, int> checklistUpdates, int checklistId, int userId, int stepId)
//        {
//            try
//            {
//                string constr = @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=HajjGuide;Data Source=DESKTOP-QK26P6V\MSSQLSERVER01";

//                using (SqlConnection con = new SqlConnection(constr))
//                {
//                    con.Open();

//                    foreach (var kvp in checklistUpdates)
//                    {
//                        int itemId = kvp.Key;
//                        int isChecked = kvp.Value;

//                        string updateQuery = "UPDATE userchecklist SET isChecked = @IsChecked WHERE checklistid = @ChecklistId AND userid = @UserId";

//                        using (SqlCommand cmd = new SqlCommand(updateQuery, con))
//                        {
//                            cmd.Parameters.AddWithValue("@IsChecked", isChecked);
//                            cmd.Parameters.AddWithValue("@ChecklistId", itemId);
//                            cmd.Parameters.AddWithValue("@UserId", userId);
//                            cmd.ExecuteNonQuery();
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.Message);
//                // Handle the exception as needed
//            }

//            return RedirectToAction("Index", new { stepId });
//        }

//        public IActionResult Index(int stepId)
//        {
//            List<ChecklistModel> checklist = GetChecklistFromDatabase();
//            return View(checklist);
//        }

//        private List<ChecklistModel> GetChecklistFromDatabase()
//        {
//            int userId = GetLoggedUserId();
//            List<ChecklistModel> checklistItems = new List<ChecklistModel>();

//            if (userId > 0)
//            {
//                string constr = @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=HajjGuide;Data Source=DESKTOP-QK26P6V\MSSQLSERVER01";

//                using (SqlConnection con = new SqlConnection(constr))
//                {
//                    con.Open();
//                    string query = "SELECT uc.userid, c.[desc] AS ChecklistDesc, c.DescUrdu AS ChecklistDescUrdu, uc.checklistid, uc.isChecked FROM userchecklist uc JOIN Checklist c ON uc.checklistid = c.id WHERE uc.userid = @UserId";

//                    using (SqlCommand cmd = new SqlCommand(query, con))
//                    {
//                        cmd.Parameters.AddWithValue("@UserId", userId);

//                        using (SqlDataReader reader = cmd.ExecuteReader())
//                        {
//                             cwhile (reader.Read())
//                            {
//                                var checklistItem = new ChecklistModel()
//                                {
//                                    UserId = reader.GetInt32(0),
//                                    Description = reader.GetString(1),
//                                    DescriptionUrdu = reader.GetString(2),
//                                    Cid = reader.GetInt32(3),
//                                    IsChecked = reader.GetBoolean(4)
//                                };

//                                checklistItems.Add(checklistItem);
//                            }
//                        }
//                    }
//                }
//            }

//            return checklistItems;
//        }

//        private int GetLoggedUserId()
//        {
//            byte[] userIdBytes = HttpContext.Session.Get("LoggedUserId");
//            return userIdBytes != null ? int.Parse(Encoding.UTF8.GetString(userIdBytes)) : 0;
//        }
//    }
//}
