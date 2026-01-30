namespace HajjGuide.Models
{
    public class ChecklistModel
    {
        public string? items { get; set; }
        public int UserId { get; set; }
        public int checklistid { get; set; }
        public string? Description { get; set; }
        public bool  IsChecked { get; set; }
        public string? DescriptionUrdu { get; set; }
       

    }
    public class MyChecklist
    {
        public List<ChecklistModel> chklst { get; set; }
        public List<ChecklistModel> chkitems { get; set; }
    }
}
