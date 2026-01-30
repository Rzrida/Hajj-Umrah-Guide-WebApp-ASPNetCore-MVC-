using HajjGuide.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace HajjGuide.Controllers
{
    public class LanguageControlController : Controller
    {
        public IActionResult Index()
        {
            List<LanguageControlModel> LC = GetLCFromDatabase();
            return View(LC);
        }

        private List<LanguageControlModel> GetLCFromDatabase()
        {
            List<LanguageControlModel> ls9 = new List<LanguageControlModel>();
            string constr = @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=HajjGuide;Data Source=localhost";


            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string q = "SELECT * FROM LanguageControl";
                using (SqlCommand cmd = new SqlCommand(q, con))
                {
                    using (SqlDataReader sda = cmd.ExecuteReader())
                    {
                        while (sda.Read())
                        {
                            var v = new LanguageControlModel()
                            {
                                id = sda.GetInt32(0),
                                Translation = sda.GetInt32(1),
                                Translitration = sda.GetInt32(2),
                                UrduArabic = sda.GetInt32(3)
                            };
                            ls9.Add(v);
                        }
                    }
                }
            }
            return ls9;
        }

        [HttpPost]
        public IActionResult UpdateLClist(Dictionary<int, Dictionary<string, int>> LC)
        {
            string constr = @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=HajjGuide;Data Source=localhost";

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();

                foreach (var kvp in LC)
                {
                    int itemId = kvp.Key;
                    var checkboxValues = kvp.Value;

                    if (checkboxValues.ContainsKey("Translation"))
                    {
                        int translationValue = checkboxValues["Translation"];
                        UpdateColumnValue(con, itemId, "Translation", translationValue);
                    }

                    if (checkboxValues.ContainsKey("Translitration"))
                    {
                        int transliterationValue = checkboxValues["Translitration"];
                        UpdateColumnValue(con, itemId, "Translitration", transliterationValue);
                    }

                    if (checkboxValues.ContainsKey("UrduArabic"))
                    {
                        int urduArabicValue = checkboxValues["UrduArabic"];
                        UpdateColumnValue(con, itemId, "UrduArbic", urduArabicValue);
                    }
                }
            }

            return RedirectToAction("Index");
        }

        private void UpdateColumnValue(SqlConnection con, int itemId, string columnName, int columnValue)
        {
            int newValue = columnValue == 1 ? 1 : 0;

            // Update the column based on the provided scenario
            string updateQuery = $"UPDATE LanguageControl SET {columnName} = @ColumnValue WHERE id = @Id";

            try
            {
                using (SqlCommand cmd = new SqlCommand(updateQuery, con))
                {
                    cmd.Parameters.AddWithValue("@ColumnValue", newValue);
                    cmd.Parameters.AddWithValue("@Id", itemId);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                // Log the exception to understand any issues with the SQL query or database connection
                Console.WriteLine($"Error updating {columnName}: {ex.Message}");
            }
        }

    }
}

