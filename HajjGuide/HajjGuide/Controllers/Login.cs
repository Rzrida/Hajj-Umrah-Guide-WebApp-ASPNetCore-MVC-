using HajjGuide.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Text;

namespace HajjGuide.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            // Login page view
            return View();
        }

        [HttpPost]
        public IActionResult LogIn(LoginModel user)
        {
            if (IsValidLogin(user.Email, user.Password))
            {
                // Redirect to the next page if login is successful
                var userid = LoginSessionModel.GetUserId(HttpContext);
                return Redirect("https://localhost:7124/Home");
            }
            else
            {
                // Failed login logic
                ModelState.AddModelError(string.Empty, "Invalid email or password");
                return View("Index", user);
            }
        }

        private bool IsValidLogin(string email, string password)
        {
            LoginSessionModel? loginUser = null;
            string constr = @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=HajjGuide;Data Source=localhost";

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();

                // Prepare a SQL query to check for valid credentials in Signup table
                // Email should be unique in database
                string query = "SELECT top 1 * FROM [Signup] WHERE [Email] = @Email AND [PasswordHash] = @Password";
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Password", password);
                //return count > 0; // If count > 0, credentials are valid
                using (SqlDataReader sda = command.ExecuteReader())
                {
                    while (sda.Read())
                    {
                        loginUser = new LoginSessionModel()
                        {
                            id = Convert.ToInt32(sda["SignupId"]),
                            email = sda["Email"] != DBNull.Value ? sda["Email"].ToString() : ""
                        };
                    }
                }

                if (loginUser != null)
                {
                    HttpContext.Session.Set("LoggedUserId", Encoding.UTF8.GetBytes(loginUser.id.ToString()));
                     //HttpContext.Session.Set("LoggedUserEmail", Encoding.UTF8.GetBytes(loginUser.email??""));
                    return true;
                }
                return false;
            }
        }
    }
}