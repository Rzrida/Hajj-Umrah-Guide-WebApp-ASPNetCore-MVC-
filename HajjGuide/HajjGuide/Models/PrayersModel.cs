namespace HajjGuide.Models
{
    public class PrayersModel
    {
            public int Id { get; set; }
            public string? Description { get; set; }
            public string? Translation { get; set; }
            public string? Transliteration { get; set; }
            public int StepId { get; set; }
            public int IsBookmark { get; set; }
        }
    }
