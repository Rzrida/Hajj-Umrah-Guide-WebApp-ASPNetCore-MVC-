namespace HajjGuide.Models
{
    public class DaysModel
    {
        public int ID { get; set; }
        public string? Description { get; set; }
        public int PlaceId { get; set; }
        public int IsHajj { get; set; }
        public string? UrduDesc { get; set; }
        public string? Day { get; set; }
        public string? dayUrdu { get; set; }
        public string? language { get; set; }
        public int ischecked { get; set; }

    }
}
