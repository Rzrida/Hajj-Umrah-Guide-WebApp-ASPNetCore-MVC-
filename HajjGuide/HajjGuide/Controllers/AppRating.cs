//using HajjGuide.Models;
//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Data.SqlClient;
//using System.Text;
//using Microsoft.AspNetCore.Http;

//namespace HajjGuide.Controllers
//{
//    public class AppRatingController : Controller
//    {
//        string constr = @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=HajjGuide;Data Source=DESKTOP-QK26P6V\MSSQLSERVER01";

//        public IActionResult Index()
//        {
//            return View();
//        }

//        [HttpPost]
//        public IActionResult RateApp(int rating)
//        {
//            byte[] userIdBytes = HttpContext.Session.Get("LoggedUserId");
//            int userId = 0;
//            if (userIdBytes != null)
//            {
//                string userIdString = Encoding.UTF8.GetString(userIdBytes);
//                userId = int.Parse(userIdString);
//            }

//            try
//            {
//                using (var connection = new SqlConnection(constr))
//                {
//                    connection.Open();

//                    // Insert rating into the database
//                    string query = "INSERT INTO AppRatings (UserId, Rating, RatedDateTime) VALUES (@UserId, @Rating, @RatedDateTime)";
//                    SqlCommand command = new SqlCommand(query, connection);
//                    command.Parameters.AddWithValue("@UserId", userId);
//                    command.Parameters.AddWithValue("@Rating", rating);
//                    command.Parameters.AddWithValue("@RatedDateTime", DateTime.Now);
//                    command.ExecuteNonQuery();

//                    ViewBag.Message = $"App rated {rating} stars successfully!";
//                }
//            }
//            catch (Exception ex)
//            {
//                // Handle any exceptions
//                ViewBag.Message = "An error occurred while rating the app.";
//                // Log the exception if needed
//            }

//            return View("Index");
//        }
//    }
//}



using HajjGuide.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace HajjGuide.Controllers
{
    public class AppRatingController : Controller
    {
        string constr =@"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=HajjGuide;Data Source=localhost";

        public IActionResult Index()
        {
            byte[] userIdBytes = HttpContext.Session.Get("LoggedUserId");
            int userId = 0;
            int userRating = 0;

            if (userIdBytes != null)
            {
                string userIdString = Encoding.UTF8.GetString(userIdBytes);
                userId = int.Parse(userIdString);

                // Check if the user has already rated
                try
                {
                    using (var connection = new SqlConnection(constr))
                    {
                        connection.Open();
                        string query = "SELECT Rating FROM AppRatings WHERE UserId = @UserId";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@UserId", userId);
                        object result = command.ExecuteScalar();

                        if (result != null)
                        {
                            userRating = Convert.ToInt32(result);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle exception
                }
            }

            ViewBag.UserRating = userRating;
            return View();
        }

        [HttpPost]
        public IActionResult RateApp(int rating)
        {
            byte[] userIdBytes = HttpContext.Session.Get("LoggedUserId");
            int userId = 0;

            if (userIdBytes != null)
            {
                string userIdString = Encoding.UTF8.GetString(userIdBytes);
                userId = int.Parse(userIdString);

                try
                {
                    using (var connection = new SqlConnection(constr))
                    {
                        connection.Open();

                        // Check if the user has already rated
                        string checkQuery = "SELECT COUNT(*) FROM AppRatings WHERE UserId = @UserId";
                        SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                        checkCommand.Parameters.AddWithValue("@UserId", userId);
                        int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                        if (count == 0)
                        {
                            // Insert rating into the database
                            string query = "INSERT INTO AppRatings (UserId, Rating, RatedDateTime) VALUES (@UserId, @Rating, @RatedDateTime)";
                            SqlCommand command = new SqlCommand(query, connection);
                            command.Parameters.AddWithValue("@UserId", userId);
                            command.Parameters.AddWithValue("@Rating", rating);
                            command.Parameters.AddWithValue("@RatedDateTime", DateTime.Now);
                            command.ExecuteNonQuery();

                            ViewBag.Message = $"App rated {rating} stars successfully!";
                        }
                        else
                        {
                            ViewBag.Message = "You have already rated the app!";
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle any exceptions
                    ViewBag.Message = "An error occurred while rating the app.";
                    // Log the exception if needed
                }
            }
            else
            {
                // Handle the case where user ID is not available
                ViewBag.Message = "User ID not found. Unable to rate the app.";
            }

            return RedirectToAction("Index");
        }
    }
}
