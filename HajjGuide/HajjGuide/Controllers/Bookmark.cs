using HajjGuide.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.VisualBasic;
using System.Text;
namespace HajjGuide.Controllers
{
    public class Bookmark : Controller
    {
        public IActionResult Index()
        {
            byte[] userIdBytes = HttpContext.Session.Get("LoggedUserId");
            int userId = 0;
            if (userIdBytes != null)
            {
                string userIdString = Encoding.UTF8.GetString(userIdBytes);
                userId = int.Parse(userIdString);
            }


            List<BookmarkModel> Bookmark = GetBookmarkFromDatabase(userId) ;
            return View(Bookmark);
        }

        private List<BookmarkModel> GetBookmarkFromDatabase(int userId)
        {

            Console.WriteLine(userId);
            List<BookmarkModel> ls4 = new List<BookmarkModel>();
            string constr = @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=HajjGuide;Data Source=localhost";

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string q = " SELECT u.Username, d.[desc] AS Duadesc, d.translation AS Duatranslation, d.translatration AS roman, ub.isbookmark, ub.userid, s.[desc] AS StepDesc, s.DescUrdu as urdudesc FROM UserBookmark ub JOIN Dua d ON ub.duaid = d.id JOIN Signup u ON ub.userid = u.SignupId JOIN Step s ON d.stepId = s.id  WHERE ub.userid = @UserId AND ub.isbookmark = 1";
                using (SqlCommand cmd = new SqlCommand(q, con))
                {
                    cmd.Parameters.AddWithValue("@UserId" , userId);
                    using (SqlDataReader sda = cmd.ExecuteReader())
                    { 
                        while (sda.Read())
                        {
                            var v = new BookmarkModel()
                            {
                                //Id = sda.GetInt32(0),
                                Description = sda.GetString(1),
                                Translation = sda.GetString(2), 
                                Transliteration = sda.GetString(3),
                                 //StepId = sda.GetInt32(4),
                                IsBookmark = sda.GetInt32(4),
                                userId=sda.GetInt32(5),
                                StepDesc = sda.GetString(6),
                                DescUrdu = sda.GetString(7)
                            };

                            ls4.Add(v);
                        }
                    }
                }
            }

            return ls4;
        }
        
        [HttpPost]
        public IActionResult UpdateChecklist(Dictionary<int, int> Prayers, int stepid)
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

                    string updateQuery = " UPDATE UserBookmark SET isBookmark = @IsBookmark WHERE duaid = @PId and userid=@userId";
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(updateQuery, con))
                        {
                            cmd.Parameters.AddWithValue("@UserId", userId);
                            cmd.Parameters.AddWithValue("@IsBookmark", isBookmark);
                            cmd.Parameters.AddWithValue("@PId", itemId);
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

    }
}


