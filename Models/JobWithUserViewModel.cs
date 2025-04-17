namespace Online_Job_Portal_MVC.Models
{
    public class JobWithUserViewModel
    {
            public AddJobModel Job { get; set; }  // Job Details
            public RegisterModel User { get; set; }  // User Details
            public ResumeModel user { get; set; }  // Resume Details
            public string? ResumePath { get; set; }  // For resume file path
            public int ResumeId { get; set; }       // To identify resume for delete
      
    }
}
