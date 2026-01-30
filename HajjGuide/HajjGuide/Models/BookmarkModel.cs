namespace HajjGuide.Models
{
    public class BookmarkModel
    {
        
        public int Id { get; set; }
        public string? Description { get; set; }
        public string? Translation { get; set; }
        public string? Transliteration { get; set; }
        public int StepId { get; set; }
        public int userId { get; set; }
        public int IsBookmark { get; set; }
        public string? StepDesc { get; set; }
        public string? DescUrdu { get; set; }
        public int PId { get; set; }
        public int stepid { get; set; }
    }
}
