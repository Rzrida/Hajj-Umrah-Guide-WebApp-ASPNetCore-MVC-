using HajjGuide.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace HajjGuide.Controllers
{
    public class SignupController : Controller
    {
        // GET: Signup
        public IActionResult Index()
        {
            return View(new SignUpModel());
        }

        // POST: Signup/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(SignUpModel user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Index", user);
                }

                if (!IsEmailValid(user.Email))
                {
                    ModelState.AddModelError(nameof(SignUpModel.Email), "Email is not valid.");
                    return View("Index", user);
                }

                if (user.Password != user.ConfirmPassword)
                {
                    ModelState.AddModelError(nameof(SignUpModel.ConfirmPassword), "Passwords do not match.");
                    return View("Index", user);
                }

                string constr = @"Integrated Security=SSPI;
                                  Persist Security Info=False;
                                  Initial Catalog=HajjGuide;
                                  Data Source=localhost";

                using (SqlConnection con = new SqlConnection(constr))
                {
                    con.Open();

                    string query = @"INSERT INTO [Signup] 
                                    (Email, PasswordHash, Username) 
                                    VALUES (@Email, @Password, @Username)";

                    using (SqlCommand command = new SqlCommand(query, con))
                    {
                        command.Parameters.AddWithValue("@Email", user.Email);
                        command.Parameters.AddWithValue("@Password", user.Password); // hash later
                        command.Parameters.AddWithValue("@Username", user.Username);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            return RedirectToAction("Index", "Login");
                        }
                    }
                }

                ModelState.AddModelError("", "Registration failed. Please try again.");
                return View("Index", user);
            }
            catch
            {
                ModelState.AddModelError("", "An error occurred while processing your request.");
                return View("Index", user);
            }
        }

        private bool IsEmailValid(string email)
        {
            string emailPattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            return Regex.IsMatch(email, emailPattern);
        }
    }
}
