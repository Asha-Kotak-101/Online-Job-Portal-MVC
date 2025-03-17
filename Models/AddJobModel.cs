
using System.ComponentModel.DataAnnotations;
using System.Data;
using Microsoft.Data.SqlClient;
namespace Online_Job_Portal_MVC.Models
{
    public class AddJobModel
    {
        SqlConnection con = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=JobPortalDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter Title")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Please enter NoOfPost")] 
        public string? NoOfPost { get; set; }

        [Required(ErrorMessage = "Please enter Description")]
        public string? Description { get; set; }
        
        [Required(ErrorMessage = "Please enter Qualification")]
        public string? Qualification { get; set; }

        [Required(ErrorMessage = "Please enter Experience")]
        public string? Experience { get; set; }

        [Required(ErrorMessage = "Please enter Specialization")]
        public string? Specialization { get; set; }

        [Required(ErrorMessage = "Please enter LastDateToApply")]
        public string? LastDateToApply { get; set; }

        [Required(ErrorMessage = "Please enter Salary")]
        public string? Salary { get; set; }

        [Required(ErrorMessage = "Please enter JobType")]
        public string? JobType { get; set; }

        [Required(ErrorMessage = "Please enter CompanyName")]
        public string? CompanyName { get; set; }

        [Required(ErrorMessage = "Please enter CompanyLogo")]
        public string? CompanyLogo { get; set; }

        [Required(ErrorMessage = "Please enter Website")]
        public string? Website { get; set; }

        [Required(ErrorMessage = "Please enter Email")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Please enter Address")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Please enter Country")]
        public string? Country { get; set; }

        [Required(ErrorMessage = "Please enter State")]
        public string? State { get; set; }


        //Retrieve all records from a table
        public List<AddJobModel> getData()
        {
            List<AddJobModel> lstUser = new List<AddJobModel>();
            SqlDataAdapter apt = new SqlDataAdapter("select * from Jobs", con);
            DataSet ds = new DataSet();
            apt.Fill(ds);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                lstUser.Add(new AddJobModel
                {
                    Id = Convert.ToInt32(dr["Id"].ToString()),
                    Title = dr["Username"].ToString(),
                    NoOfPost = dr["Password"].ToString(),
                    Description = dr["ConfirmPassword"].ToString(),
                    Qualification = dr["Fullname"].ToString(),
                    Experience = dr["Address"].ToString(),
                    Specialization = dr["MobileNumber"].ToString(),
                    LastDateToApply = dr["Email"].ToString(),
                    Salary = dr["Country"].ToString(),
                    JobType = dr["Role"].ToString(),
                    CompanyName = dr["ConfirmPassword"].ToString(),
                    CompanyLogo = dr["Fullname"].ToString(),
                    Website = dr["Address"].ToString(),
                    Email = dr["MobileNumber"].ToString(),
                    Address = dr["Email"].ToString(),
                    Country = dr["MobileNumber"].ToString(),
                    State = dr["Email"].ToString(),

                });
            }
            return lstUser;
        }


        //Insert a record into a database table
        public bool insert(AddJobModel re)
        {
            using (SqlConnection con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=JobPortalDB;Integrated Security=True;"))
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Jobs (Title, NoOfPost, Description, Qualification,Address, Experience, Email, Country, Specialization, LastDateToApply, Salary, JobType, CompanyName, CompanyLogo, Website, State) VALUES (@Title, @NoOfPost, @Description, @Qualification, @Address, @Experience, @Email, @Country, @Specialization, @LastDateToApply, @Salary, @JobType, @CompanyName, @CompanyLogo, @Website, @State)", con);

                cmd.Parameters.AddWithValue("@Title", re.Title);
                cmd.Parameters.AddWithValue("@NoOfPost", re.NoOfPost);
                cmd.Parameters.AddWithValue("@Description", re.Description);
                cmd.Parameters.AddWithValue("@Qualification", re.Qualification);
                cmd.Parameters.AddWithValue("@Address", re.Address);
                cmd.Parameters.AddWithValue("@Experience", re.Experience);
                cmd.Parameters.AddWithValue("@Email", re.Email);
                cmd.Parameters.AddWithValue("@Country", re.Country);
                cmd.Parameters.AddWithValue("@Specialization", re.Specialization);
                cmd.Parameters.AddWithValue("@LastDateToApply", re.LastDateToApply);
                cmd.Parameters.AddWithValue("@Salary", re.Salary);
                cmd.Parameters.AddWithValue("@JobType", re.JobType);
                cmd.Parameters.AddWithValue("@CompanyName", re.CompanyName);
                cmd.Parameters.AddWithValue("@CompanyLogo", re.CompanyLogo);
                cmd.Parameters.AddWithValue("@Website", re.Website);
                cmd.Parameters.AddWithValue("@State", re.State);

                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();

                return i >= 1;
            }
        }
    }
}
