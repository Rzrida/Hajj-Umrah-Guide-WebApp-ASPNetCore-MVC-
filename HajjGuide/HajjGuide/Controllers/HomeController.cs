using HajjGuide.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Diagnostics;

namespace HajjGuide.Controllers
{

    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
          
            List<HomeModel> steps = GetStepsByIsHajj(1);

            return View(steps);
        }

        [HttpPost]
        public IActionResult FetchUmrahSteps(int isHajj)
        {
            
            List<HomeModel> steps = GetStepsByIsHajj(isHajj);

            return View("Index", steps);
        }

        private List<HomeModel> GetStepsByIsHajj(int isHajj)
        {
            List<HomeModel> steps = new List<HomeModel>();

            string constr = @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=HajjGuide;Data Source=localhost";

            using (SqlConnection connection = new SqlConnection(constr))
            {
                connection.Open();

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

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}