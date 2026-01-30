namespace HajjGuide.Models
{
    public class AppRatingModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Rating { get; set; }
        public DateTime RatedDateTime { get; set; }
    }
}
