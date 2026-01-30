using System.Collections.Generic;
namespace HajjGuide.Models
{
    public class AddDuaModel
    {
        public int Id { get; set; }
        public string? Desc { get; set; }
        public string? User { get; set; }
        public int StepId { get; set; }
        public int IsChecked { get; set; }
    }
    public interface IDuaRequestModel
    {
       
        public int stepid { get; set; }
        public string? Description { get; set; }
        public string? UrduDesc { get; set; }
        public int IsChecked { get; set; }
        public int isBookmark { get; set; }
        public string? translation { get; set; }
        public string? translatration { get; set; }
        public int desc { get; set; }
        public string? User { get; set; }
        public string? Desc { get; set; }


    }
    public class DuasModel : IDuaRequestModel
    {
   
        public string? Description { get; set; }
        public string? UrduDesc { get; set; }
        public int stepid { get; set; }
        public int PId { get; set; }
        public string? Desc { get; set; }
        public string? User { get; set; }
        public int IsChecked { get; set; }
        public int isBookmark  { get; set; }
        public string? translation { get; set; }
         public string? translatration { get; set; }
          public string? StepDesc { get; set; }
        public string? DescUrdu { get; set; }
        public int desc { get; set; }
        public int DrId { get; set; }
        public string? Applicant { get; set; }

        public int cid { get; set; }


    }

    public class MyViewModel
    {
        public List<DuasModel> DuasList { get; set; }
        public List<DuasModel> DuaRequestsList { get; set; }
        public List<DuasModel> Comments { get; set; }
    }

}

