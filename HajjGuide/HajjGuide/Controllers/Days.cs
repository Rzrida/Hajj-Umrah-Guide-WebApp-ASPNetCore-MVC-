using System;
using System.Data;
using System.Data.SqlClient;
using HajjGuide.Models;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;



namespace HajjGuide.Controllers
{
	
	public class Days : Controller
	{
		

		public IActionResult Index()
		{
            List<DaysModel> ls6 = new List<DaysModel>();
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
                //string q = "SELECT DISTINCT day FROM Step";
                string q = "SELECT DISTINCT sortsequence, day, dayUrdu FROM Step ORDER BY sortsequence";

                using (SqlCommand cmd = new SqlCommand(q, con))
                {
                    using (SqlDataReader sda = cmd.ExecuteReader())
                    {
                        while (sda.Read())
                        {
                            var v = new DaysModel()
                            {

                            };
                            if (translation == 1) 
                            {
                                v.Day = sda.GetString(1);
                            }
                            else
                                v.Day = "";
                            if (urduarabic == 1)
                            {


                                v.dayUrdu = sda.GetString(2);
                            }
                            else
                                v.dayUrdu = "";
                            
                            ls6.Add(v);
                        }
                    }
                }
            }
           

            return View(ls6);
        }
        //SELECT DISTINCT sortsequence, day, dayUrdu FROM Step WHERE day != '7th Dhul Hijjah' ORDER BY sortsequence;
        public ActionResult LC()
        {
            List<DaysModel> ls6 = new List<DaysModel>();
            string constr = @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=HajjGuide;Data Source=localhost";

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                //string q = "SELECT DISTINCT day FROM Step";
                string q = "  select * from LanguageControl";

                using (SqlCommand cmd = new SqlCommand(q, con))
                {
                    using (SqlDataReader sda = cmd.ExecuteReader())
                    {
                        while (sda.Read())
                        {
                            var v = new DaysModel()
                            {

                                ischecked = sda.GetInt32(1),
                                language = sda.GetString(2)
                            };
                            ls6.Add(v);
                        }
                    }
                }
            }


            return View(ls6);
        }
    } 
}
