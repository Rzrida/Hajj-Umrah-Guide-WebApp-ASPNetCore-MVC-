using HajjGuide.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Text;

namespace HajjGuide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel user)
        {
            if (IsValidLogin(user.Email, user.Password))
            {
                // Replace with your logic to generate and return a token or user info
                var userid = LoginSessionModel.GetUserId(HttpContext);
                return Ok(new { Message = "Login successful", UserId = userid });
            }
            else
            {
                return Unauthorized(new { Message = "Invalid email or password" });
            }
        }

        private bool IsValidLogin(string email, string password)
        {
            LoginSessionModel? loginUser = null;
            string constr = @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=HajjGuide;Data Source=localhost";

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string query = "SELECT top 1 * FROM [Signup] WHERE [Email] = @Email AND [PasswordHash] = @Password";
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Password", password);

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
                    return true;
                }
                return false;
            }
        }
    }
}
