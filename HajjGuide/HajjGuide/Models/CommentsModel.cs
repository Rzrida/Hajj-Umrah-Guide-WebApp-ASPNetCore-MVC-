using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace HajjGuide.Models
{
    public class CommentsModel
    {
        public int id { get; set; }
        public string? desc { get; set; }
        public string? StepDesc { get; set; }
        public string? UrduDesc { get; set; }
  
        public string? Comment { get; set; }
        public int stepid { get; set; }
        public int userid { get; set; }
    }
}
