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
    public class ViewComment : Controller
    {
        public IActionResult Index(int stepid)
        {
         
        //}
        //public IActionResult Comment(int stepid) // Pass stepid as a parameter
        //{
            byte[] userIdBytes = HttpContext.Session.Get("LoggedUserId");
            int userId = 0;
            if (userIdBytes != null)
            {
                string userIdString = Encoding.UTF8.GetString(userIdBytes);
                userId = int.Parse(userIdString);

                // Now you have the user's ID (userId) available for use.
            }
            List<CommentsModel> Prayers = GeRequestedCommentFromDatabase(stepid);
            return View(Prayers);
        }
          

        private List<CommentsModel> GeRequestedCommentFromDatabase(int stepId)
        {
            byte[] userIdBytes = HttpContext.Session.Get("LoggedUserId");
            int userId = 0;
            if (userIdBytes != null)
            {
                string userIdString = Encoding.UTF8.GetString(userIdBytes);
                userId = int.Parse(userIdString);

                // Now you have the user's ID (userId) available for use.
            }

            List<CommentsModel> ls14 = new List<CommentsModel>();
            string constr =  @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=HajjGuide;Data Source=localhost";
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string q = " select c.id, c.descs as Comment, st.[desc] as stepname, st.DescUrdu as stepurduname from Comments c join Step st on st.id = c.stepId where c.stepId = @stepId and c.userid = @userId"; // Filter by stepid
                using (SqlCommand cmd = new SqlCommand(q, con))
                {
                    cmd.Parameters.AddWithValue("@StepId", stepId);
                    cmd.Parameters.AddWithValue("@userId", userId); // Set the parameter value
                    using (SqlDataReader sda = cmd.ExecuteReader())
                    {
                        while (sda.Read())
                        {
                            var v = new CommentsModel()
                            {
                                id=sda.GetInt32(0),
                                desc = sda.GetString(1),
                                StepDesc = sda.GetString(2),
                                UrduDesc=sda.GetString(3),
                                //stepid = sda.GetInt32(2),

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
