
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using HajjGuide.Models;
using Microsoft.Extensions.Localization;

namespace HajjGuide.Controllers
{
    public class StepsController : Controller
    {
        //private readonly IStringLocalizer<StepsController> _localizer;

        //public StepsController(IStringLocalizer<StepsController> localizer)
        //{
        //    _localizer = localizer;
        //}


        //public IActionResult Index()
        //{
        //    List<StepsModel> ls3 = new List<StepsModel>();
        //    string constr = @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=HajjGuide;Data Source=DESKTOP-QK26P6V\MSSQLSERVER01";

        //    using (SqlConnection con = new SqlConnection(constr))
        //    {
        //        con.Open();
        //        string q = "SELECT * FROM Step";
        //        using (SqlCommand cmd = new SqlCommand(q, con))
        //        {
        //            using (SqlDataReader sda = cmd.ExecuteReader())
        //            {
        //                while (sda.Read())
        //                {
        //                    var v = new StepsModel()
        //                    {
        //                        ID = sda.GetInt32(0),
        //                        Description = sda.GetString(1),
        //                        PlaceId = sda.GetInt32(2),
        //                        IsHajj = sda.GetInt32(3),
        //                        Day = sda.GetInt32(4),
        //                        UrduDesc = sda.GetString(5)
        //                    };
        //                    ls3.Add(v);
        //                }
        //            }
        //        }
        //    }
        private readonly IConfiguration _configuration;

        public StepsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IActionResult> Index(string day, string calledfrom = "")
        {
            List<StepsModel> steps = new List<StepsModel>();

            if (calledfrom == "Umrah")
            {
               
                steps = await GetStepsByUmrah(0);
            }
            else
            {
                steps = await GetStepsByDayAsync(day);
            }

            //  List<StepsModel> steps = await GetStepsByDayAsync(day);



            //string localizedString = _localizer["HelloWorld"];
            return View(steps);

        }

        private async Task<List<StepsModel>> GetStepsByDayAsync(string day)
        {
            List<StepsModel> steps = new List<StepsModel>();


            string constr =  @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=HajjGuide;Data Source=localhost";

            using (SqlConnection connection = new SqlConnection(constr))
            {
                await connection.OpenAsync();
                string query1 = "SELECT Translation,Translitration,UrduArbic from LanguageControl";
                SqlCommand cmd1 = new SqlCommand(query1, connection);
                SqlDataReader sda1 = cmd1.ExecuteReader();
                sda1.Read();
                int translation = int.Parse(sda1["Translation"].ToString());
                int translitration = int.Parse(sda1["Translitration"].ToString());
                int urduarabic = int.Parse(sda1["UrduArbic"].ToString());
                sda1.Close();
                string query = "SELECT * FROM Step WHERE day = @Day";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Day", day);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            StepsModel step = new StepsModel
                            {
                                ID = reader.GetInt32(0),
                                Description = reader.GetString(1),
                                PlaceId = reader.GetInt32(2),
                                IsHajj = reader.GetInt32(3),
                                UrduDesc = reader.GetString(4),
                                 Day = reader.GetString(5)
                            };
                        
                            steps.Add(step);
                          
                        }
                    }
                }
            }

            return steps;
        }


        private async Task<List<StepsModel>> GetStepsByUmrah(int isHajj)
        {
            List<StepsModel> steps = new List<StepsModel>();


            string constr =  @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=HajjGuide;Data Source=localhost";

            using (SqlConnection connection = new SqlConnection(constr))
            {
                await connection.OpenAsync();
                string query1 = "SELECT Translation,Translitration,UrduArbic from LanguageControl";
                SqlCommand cmd1 = new SqlCommand(query1, connection);
                SqlDataReader sda1 = cmd1.ExecuteReader();
                sda1.Read();
                int translation = int.Parse(sda1["Translation"].ToString());
                int translitration = int.Parse(sda1["Translitration"].ToString());
                int urduarabic = int.Parse(sda1["UrduArbic"].ToString());
                sda1.Close();
                string query = "SELECT * FROM Step WHERE isHajj = @isHajj";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@isHajj", isHajj);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            StepsModel step = new StepsModel
                            {
                                ID = reader.GetInt32(0),
                                Description = reader.GetString(1),
                                PlaceId = reader.GetInt32(2),
                                IsHajj = reader.GetInt32(3),
                                UrduDesc = reader.GetString(4),
                                Day = reader.GetString(5),
                            };

                            steps.Add(step);
                        }
                    }
                }
            }

            return steps;
        }

        private List<HomeModel> GetStepsByIsHajj(int isHajj)
        {
            List<HomeModel> steps = new List<HomeModel>();

            string constr =  @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=HajjGuide;Data Source=localhost";

            using (SqlConnection connection = new SqlConnection(constr))
            {
                connection.Open();
                string query1 = "SELECT Translation,Translitration,UrduArbic from LanguageControl";
                SqlCommand cmd1 = new SqlCommand(query1, connection);
                SqlDataReader sda1 = cmd1.ExecuteReader();
                sda1.Read();
                int translation = int.Parse(sda1["Translation"].ToString());
                int translitration = int.Parse(sda1["Translitration"].ToString());
                int urduarabic = int.Parse(sda1["UrduArbic"].ToString());
                sda1.Close();
                string query = "SELECT * FROM Step WHERE isHajj = @IsHajj";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IsHajj", isHajj);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            HomeModel step = new HomeModel
                            {
                                ID = reader.GetInt32(0),
                                Description = reader.GetString(1),
                                PlaceId = reader.GetInt32(2),
                                IsHajj = reader.GetInt32(3),
                                UrduDesc = reader.GetString(4),
                                 Day = reader.GetString(5)
                            };

                            steps.Add(step);
                        }
                    }
                }
            }

            return steps;
        }

    }
}
