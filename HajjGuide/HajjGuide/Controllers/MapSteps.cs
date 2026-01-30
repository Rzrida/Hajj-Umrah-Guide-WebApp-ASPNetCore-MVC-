using HajjGuide.Models;
using Microsoft.AspNetCore.Mvc;
using Azure.Core;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Transactions;
namespace HajjGuide.Controllers
{ 
    public class MapSteps : Controller
    {
        public ActionResult Index(int placeid)

        {
            List<Map1Model> places = new List<Map1Model>();


            string constr = @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=HajjGuide;Data Source=localhost";
            string query = "SELECT id,placeId,[desc],DescUrdu FROM Step where placeId=@placeid ";

            using (SqlConnection connection = new SqlConnection(constr))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@placeid", placeid);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                 

                    while (reader.Read())
                    {
                        Map1Model place = new Map1Model
                        {
                            id = (int)reader["id"],
                            placeid = (int)reader["placeId"],
                            Desc = reader["desc"].ToString(),
                            urdudesc = reader["DescUrdu"].ToString(),

                        };
                        places.Add(place);
                    }

                    reader.Close();
                }
            }

            return View(places); // Pass the places data to the view
        }
    }
}






//private async Task<List<StepsModel>> GetStepsByDayAsync(string day)
//{
//    List<StepsModel> steps = new List<StepsModel>();


//    string constr = @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=HajjGuide;Data Source=DESKTOP-QK26P6V\MSSQLSERVER01";

//    using (SqlConnection connection = new SqlConnection(constr))
//    {
//        await connection.OpenAsync();
//        string query1 = "SELECT Translation,Translitration,UrduArbic from LanguageControl";
//        SqlCommand cmd1 = new SqlCommand(query1, connection);
//        SqlDataReader sda1 = cmd1.ExecuteReader();
//        sda1.Read();
//        int translation = int.Parse(sda1["Translation"].ToString());
//        int translitration = int.Parse(sda1["Translitration"].ToString());
//        int urduarabic = int.Parse(sda1["UrduArbic"].ToString());
//        sda1.Close();
//        string query = "SELECT * FROM Step WHERE day = @Day";
//        using (SqlCommand command = new SqlCommand(query, connection))
//        {
//            command.Parameters.AddWithValue("@Day", day);

//            using (SqlDataReader reader = await command.ExecuteReaderAsync())
//            {
//                while (await reader.ReadAsync())
//                {
//                    StepsModel step = new StepsModel
//                    {
//                        ID = reader.GetInt32(0),
//                        Description = reader.GetString(1),
//                        PlaceId = reader.GetInt32(2),
//                        IsHajj = reader.GetInt32(3),
//                        UrduDesc = reader.GetString(4),
//                        Day = reader.GetString(5)
//                    };

//                    steps.Add(step);

//                }