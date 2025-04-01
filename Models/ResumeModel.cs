using System.ComponentModel.DataAnnotations;
using System.Data;
using Microsoft.Data.SqlClient;
namespace Online_Job_Portal_MVC.Models
{
    public class ResumeModel
    {
        
        SqlConnection con = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=JobPortalDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        [Key]
        public int Id { get; set; }


        [Required(ErrorMessage = "Please enter Fullname")]
        public string ? Fullname { get; set; }


        [Required(ErrorMessage = "Please enter Username")]
        public string? Username { get; set; }


        [Required(ErrorMessage = "Please enter Address")]
        public string? Address { get; set; }


        [Required(ErrorMessage = "Please enter MoblieNumber")]
        public string? MoblieNumber { get; set; }


        [Required(ErrorMessage = "Please enter Email")]
        public string? Email { get; set; }


        [Required(ErrorMessage = "Please enter Country")]
        public string? Country { get; set; }


        [Required(ErrorMessage = "Please enter 10thPercentage")]
        public string? TenPercentage { get; set; }


        [Required(ErrorMessage = "Please enter Fullname")]
        public string? TWPercentage { get; set; }
        

        [Required(ErrorMessage = "Please enter GraduationGrade")]
        public string? GraduationGrade { get; set; }


        [Required(ErrorMessage = "Please enter GraduationGrade")]
        public string? PostGraduationGrade { get; set; }
        

        [Required(ErrorMessage = "Please enter PHDGrade")]
        public string? PHDGrade { get; set; }

        

        [Required(ErrorMessage = "Please enter JobProfile")]
        public string? JobProfile { get; set; }
        

        [Required(ErrorMessage = "Please enter WorksExperience")]
        public string? WorksExperience { get; set; }


        [Required(ErrorMessage = "Please upload your resume")]
        public IFormFile? UploadResumeFile { get; set; } // Change to IFormFile
        public string? UploadResume { get; set; } // Store the file path



        //Retrieve all records from a table
        public List<ResumeModel> getData()
        {
            List<ResumeModel> lstUser = new List<ResumeModel>();
            SqlDataAdapter apt = new SqlDataAdapter("select * from Resume", con);
            DataSet ds = new DataSet();
            apt.Fill(ds);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                lstUser.Add(new ResumeModel
                {
                    Id = Convert.ToInt32(dr["Id"].ToString()),
                    Fullname = dr["Fullname"].ToString(),
                    Username = dr["Username"].ToString(),
                    Address = dr["Address"].ToString(),
                    MoblieNumber = dr["MoblieNumber"].ToString(),
                    Email = dr["Email"].ToString(),
                    Country = dr["Country"].ToString(),
                    TenPercentage = dr["TenPercentage"].ToString(),
                    TWPercentage = dr["TWPercentage"].ToString(),
                    GraduationGrade = dr["GraduationGrade"].ToString(),
                    PostGraduationGrade = dr["PostGraduationGrade"].ToString(),
                    PHDGrade = dr["PHDGrade"].ToString(),
                    JobProfile = dr["JobProfile"].ToString(),
                    WorksExperience = dr["WorksExperience"].ToString(),
                    UploadResume = dr["UploadResume"].ToString(),

                });
            }
            return lstUser;
        }



        //Insert a record into a database table
        public bool insert(ResumeModel re)
        {
            using (SqlConnection con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=JobPortalDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"))
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Resume (Fullname, Username, Address, MoblieNumber, Email, Country, TenPercentage, TWPercentage, GraduationGrade, PostGraduationGrade, PHDGrade, JobProfile, WorksExperience, UploadResume) " + "VALUES (@Fullname, @Username, @Address, @MoblieNumber, @Email, @Country, @TenPercentage, @TWPercentage, @GraduationGrade, @PostGraduationGrade, @PHDGrade, @JobProfile, @WorksExperience, @UploadResume)", con);

                cmd.Parameters.AddWithValue("@Fullname", re.Fullname);
                cmd.Parameters.AddWithValue("@Username", re.Username);
                cmd.Parameters.AddWithValue("@Address", re.Address);
                cmd.Parameters.AddWithValue("@MoblieNumber", re.MoblieNumber);
                cmd.Parameters.AddWithValue("@Email", re.Email);
                cmd.Parameters.AddWithValue("@Country", re.Country);
                cmd.Parameters.AddWithValue("@TenPercentage", re.TenPercentage);
                cmd.Parameters.AddWithValue("@TWPercentage", re.TWPercentage);
                cmd.Parameters.AddWithValue("@GraduationGrade", re.GraduationGrade);
                cmd.Parameters.AddWithValue("@PostGraduationGrade", re.PostGraduationGrade);
                cmd.Parameters.AddWithValue("@PHDGrade", re.PHDGrade);
                cmd.Parameters.AddWithValue("@JobProfile", re.JobProfile);
                cmd.Parameters.AddWithValue("@WorksExperience", re.WorksExperience);
                cmd.Parameters.AddWithValue("@UploadResume", re.UploadResume);

                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();

                return i >= 1;
            }
        }



    }
}
