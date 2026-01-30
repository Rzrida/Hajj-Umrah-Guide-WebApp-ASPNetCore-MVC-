

using Azure.Core;
using HajjGuide.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text;
using System.Transactions;

namespace HajjGuide.Controllers
{
    public class PrayersController : Controller
    {
        private const string connectionString =  @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=HajjGuide;Data Source=localhost";

        public ActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search(int stepId)
        {
            List<DuasModel> prayers = GetPrayersByStep(stepId);
            return View("PrayersList", prayers);
        }

        private List<DuasModel> GetPrayersByStep(int stepId)
        {
            byte[] userIdBytes = HttpContext.Session.Get("LoggedUserId");
            int userId = 0;
            if (userIdBytes != null)
            {
                string userIdString = Encoding.UTF8.GetString(userIdBytes);
                userId = int.Parse(userIdString);

                // Now you have the user's ID (userId) available for use.
            }
            List<DuasModel> prayers = new List<DuasModel>();
            List<DuasModel> requestedpayers = new List<DuasModel>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "SELECT du.[desc],du.translation,du.translatration,du.stepId, ub.isbookmark, u.Username FROM [Dua] du LEFT JOIN [UserBookmark] ub ON du.id = ub.duaid AND ub.userid = @userId  LEFT JOIN [Signup] u ON ub.userid = u.SignupId  WHERE du.stepId = @StepId, select * from DuaRequest where stepId = @StepId";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.Parameters.AddWithValue("@StepId", stepId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DuasModel prayer = new DuasModel
                            {
                                PId = reader.GetInt32(0),
                                Description = reader.GetString(1),
                                translation = reader.GetString(2),
                                translatration = reader.GetString(3),
                                stepid = reader.GetInt32(4),
                                isBookmark = reader.GetInt32(5),

                            };

                            prayers.Add(prayer);
                        }
                    }
                }
            }

            return prayers;
        }

    
    private readonly IConfiguration _configuration;

        public PrayersController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index(int stepId)
        {
            MyViewModel viewModel = new MyViewModel();
            viewModel.DuasList = GetDuaList(stepId);
            viewModel.DuaRequestsList = GetDuaRequestsList(stepId);
            return View(viewModel);
        }

        private List<DuasModel> GetDuaList(int stepId)
        {
            byte[] userIdBytes = HttpContext.Session.Get("LoggedUserId");
            int userId = 0;
            if (userIdBytes != null)
            {
                string userIdString = Encoding.UTF8.GetString(userIdBytes);
                userId = int.Parse(userIdString);

                // Now you have the user's ID (userId) available for use.
            }
            List<DuasModel> prayers = new List<DuasModel>();

            string constr = @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=HajjGuide;Data Source=localhost";


            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string query1 = "SELECT Translation,Translitration,UrduArbic from LanguageControl";
                SqlCommand cmd1 = new SqlCommand(query1, con);
                SqlDataReader sda1 = cmd1.ExecuteReader();
                sda1.Read();
                int translation = int.Parse(sda1["Translation"].ToString());
                int translitration = int.Parse(sda1["Translitration"].ToString());
                int urduarabic = int.Parse(sda1["UrduArbic"].ToString());
                sda1.Close();

                string query = "SELECT   Dua.id,   Dua.[desc],  Dua.translation,  Dua.translatration, Dua.stepId, Step.[desc] AS StepDesc, Step.DescUrdu, ub.isbookmark AS UserBookmarkIsBookmark FROM  Dua INNER JOIN  Step ON Dua.stepId = Step.id LEFT JOIN  UserBookmark ub ON Dua.id = ub.duaid AND ub.userid = @userId WHERE  Dua.stepId = @StepId";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.Parameters.AddWithValue("@StepId", stepId);
                    using (SqlDataReader sda = cmd.ExecuteReader())
                    {
                        while (sda.Read())
                        {
                            var v = new DuasModel()
                            {
                                PId = sda.GetInt32(0),
                                stepid = sda.GetInt32(4),
                                isBookmark = sda.IsDBNull(7) ? 0 : sda.GetInt32(7),
                                StepDesc = sda.GetString(5),
                                DescUrdu = sda.GetString(6)
                            };
                            if (translation == 1)
                            {
                                v.translation = sda.GetString(3);
                            }
                            else
                                v.translation = "";

                            if (translitration == 1)
                            {
                                v.translatration = sda.GetString(2);
                            }
                            else
                                v.translatration = "";

                            if (urduarabic == 1)
                            {
                                v.Description = sda.GetString(1);
                            }
                            else
                                v.Description = "";

                            prayers.Add(v);
                        }
                    }

                    con.Close();
                }

            }


            return prayers;

        }


        private List<DuasModel> GetDuaRequestsList(int stepId)
        {
            byte[] userIdBytes = HttpContext.Session.Get("LoggedUserId");
            int userId = 0;
            if (userIdBytes != null)
            {
                string userIdString = Encoding.UTF8.GetString(userIdBytes);
                userId = int.Parse(userIdString);

                // Now you have the user's ID (userId) available for use.
            }

            List<DuasModel> requestedpayers = new List<DuasModel>();
            string constr = @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=HajjGuide;Data Source=localhost";


            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string query = "select dr.id, dr.Applicant, dr.[desc] as paryer, st.[desc] as stepname, st.DescUrdu as stepurduname from DuaRequest dr join Step st on st.id = dr.stepId where dr.stepId = @StepId and dr.userid = @userId";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@StepId", stepId);
                    cmd.Parameters.AddWithValue("@UserId", userId);


                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var prayer = new DuasModel
                            {
                                PId = reader.GetInt32(0),
                                Applicant = reader.GetString(1),
                                Description = reader.GetString(2),
                                Desc = reader.GetString(3),
                                UrduDesc = reader.GetString(4)
                            };

                            requestedpayers.Add(prayer);
                        }
                    }
                }

                con.Close();
            }

            return requestedpayers;

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

        public ActionResult YourAction(int stepId)
        {
            List<DuasModel> duaRequests = new List<DuasModel>();


            string constr = @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=HajjGuide;Data Source=localhost";

            using (SqlConnection connection = new SqlConnection(constr))
            {
                connection.Open();
                string sql = "SELECT * FROM DuaRequest where step";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@StepId", stepId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DuasModel duaRequest = new DuasModel
                            {
                                DrId = reader.GetInt32(0),
                                Desc = reader.GetString(1),
                                User = reader.GetString(2),
                                stepid = reader.GetInt32(3),
                                IsChecked = reader.GetInt32(4)
                            };

                            duaRequests.Add(duaRequest);
                        }
                    }
                }
            }

            return View(duaRequests);
        }
        //public string GetStepName(int stepId)
        //{
        //    {
        //        List<DuasModel> ls6 = new List<DuasModel>();
        //        string constr = @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=HajjGuide;Data Source=DESKTOP-QK26P6V\MSSQLSERVER01";

        //        using (SqlConnection con = new SqlConnection(constr))
        //        {
        //            con.Open();
        //            string query = "SELECT Dua.id, Dua.[desc], Dua.translation, Dua.translatration, Dua.stepId, Dua.isBookmark, Step.[desc] AS StepDesc, Step.DescUrdu FROM Dua INNER JOIN Step ON Dua.stepId = Step.id WHERE Dua.stepId = @StepId";


        //            using (SqlCommand cmd = new SqlCommand(query, con))
        //            {
        //                cmd.Parameters.AddWithValue("@StepId", stepId);
        //                using (SqlDataReader sda = cmd.ExecuteReader())
        //                {
        //                    while (sda.Read())
        //                    {
        //                        var v = new DuasModel()
        //                        {
        //                            PId = sda.GetInt32(0),
        //                            Description = sda.GetString(1),
        //                            translation = sda.GetString(2),
        //                            translatration = sda.GetString(3),
        //                            stepid = sda.GetInt32(4),
        //                            isBookmark = sda.GetInt32(5),
        //                            StepDesc = sda.GetString(6),
        //                            DescUrdu = sda.GetString(7)
        //                        };
        //                        ls6.Add(v);
        //                    }
        //                }
        //            }
        //        }

        //        return View(ls6);
        //    }



        //public IActionResult Comentview(int stepid)
        //{

        //    //}
        //    //public IActionResult Comment(int stepid) // Pass stepid as a parameter
        //    //{
        //    byte[] userIdBytes = HttpContext.Session.Get("LoggedUserId");
        //    int userId = 0;
        //    if (userIdBytes != null)
        //    {
        //        string userIdString = Encoding.UTF8.GetString(userIdBytes);
        //        userId = int.Parse(userIdString);

        //        // Now you have the user's ID (userId) available for use.
        //    }
        //    List<DuasModel> Prayers = GeRequestedCommentFromDatabase(stepid);
        //    return View(Prayers);
        //}


        //private List<DuasModel> GeRequestedCommentFromDatabase(int stepId)
        //{
        //    byte[] userIdBytes = HttpContext.Session.Get("LoggedUserId");
        //    int userId = 0;
        //    if (userIdBytes != null)
        //    {
        //        string userIdString = Encoding.UTF8.GetString(userIdBytes);
        //        userId = int.Parse(userIdString);

        //        // Now you have the user's ID (userId) available for use.
        //    }

        //    List<DuasModel> ls14 = new List<DuasModel>();
        //    string constr = @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=HajjGuide;Data Source=DESKTOP-QK26P6V\MSSQLSERVER01";

        //    using (SqlConnection con = new SqlConnection(constr))
        //    {
        //        con.Open();
        //        string q = " select c.id, c.descs as Comment, st.[desc] as stepname, st.DescUrdu as stepurduname from Comments c join Step st on st.id = c.stepId where c.stepId = 1 and c.userid = 1\r\n"; // Filter by stepid
        //        using (SqlCommand cmd = new SqlCommand(q, con))
        //        {
        //            cmd.Parameters.AddWithValue("@StepId", stepId);
        //            cmd.Parameters.AddWithValue("@userId", userId); // Set the parameter value
        //            using (SqlDataReader sda = cmd.ExecuteReader())
        //            {
        //                while (sda.Read())
        //                {
        //                    var v = new DuasModel()
        //                    {

        //                        Desc = sda.GetString(0),
        //                        StepDesc = sda.GetString(1),
        //                        UrduDesc = sda.GetString(2),
        //                        //stepid = sda.GetInt32(2),

        //                    };

        //                    ls14.Add(v);
        //                }
        //            }
        //        }
        //    }

        //    return ls14;
        //}
    }
}

