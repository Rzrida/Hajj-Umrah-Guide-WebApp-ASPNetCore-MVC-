using HajjGuide.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Data.SqlClient;
using static System.Net.Mime.MediaTypeNames;


namespace HajjGuide.Controllers
{
    public class RequestedPrayers : Controller
    {
        public IActionResult Index()
        {
            List<RequestedPrayerModel> ls16 = new List<RequestedPrayerModel>();
            string constr =  @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=HajjGuide;Data Source=localhost";

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                //string q = "SELECT DISTINCT day FROM Step";
                string q = "SELECT DR.id, DR.[desc] AS DuaRequestDescription, DR.[user] AS DuaRequestUser, DR.isChecked, DR.Applicant, S.[desc] AS StepDescription FROM DuaRequest AS DR LEFT JOIN Step AS S ON DR.stepId = S.id;";

                using (SqlCommand cmd = new SqlCommand(q, con))
                {
                    using (SqlDataReader sda = cmd.ExecuteReader())
                    {
                        while (sda.Read())
                        {
                            var v = new RequestedPrayerModel()
                            {

                                Desc = sda.GetString(1),
                                User = sda.GetString(2),
                                 IsChecked = sda.GetInt32(3),
                                Applicant = sda.GetString(4),
                                 StepDesc = sda.GetString(5),
                       
                            };
                            ls16.Add(v);
                        }
                    }
                }
            }


            return View(ls16);
        }

       
        }
    }

