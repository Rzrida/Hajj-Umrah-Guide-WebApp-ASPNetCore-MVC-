    
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using HajjGuide.Models;
namespace HajjGuide.Controllers
{
    public class HajjType : Controller
    {

        public IActionResult Index(int isHajj)
        {
            List<HajjTypeModel> ls5 = new List<HajjTypeModel>();
            string constr =  @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=HajjGuide;Data Source=localhost";


            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();

                string q = "SELECT *FROM HajjType";
                using (SqlCommand cmd = new SqlCommand(q, con))
                {
                    cmd.Parameters.AddWithValue("@isHajj", isHajj);
                    using (SqlDataReader sda = cmd.ExecuteReader())
                    {
                        while (sda.Read())
                        {
                            var v = new HajjTypeModel()
                            {
                                desc = sda.GetString(0),
                                stepId = sda.GetInt32(1),
                                
                            };
                            ls5.Add(v);
                        }
                    }
                }
            }

            // Pass the data to the view
            return View(ls5);
        }
    }
}
        
