using System.Text;

namespace HajjGuide.Models
{
    public class LoginSessionModel
    {
        public string? email { get; set; }
        public int id { get; set; }

        public static string GetUserId(HttpContext context)
        {
            string Id = string.Empty;
            var loginsession = context.Session.Get("LoggedUserId");
            if (loginsession != null)
            {
                Id = Encoding.UTF8.GetString(loginsession);
            };
            return Id;
        }
    }
}
