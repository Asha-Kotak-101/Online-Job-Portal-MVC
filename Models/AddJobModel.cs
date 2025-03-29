
using System.ComponentModel.DataAnnotations;
using System.Data;
using Microsoft.Data.SqlClient;
namespace Online_Job_Portal_MVC.Models
{
    public class AddJobModel
    {
        SqlConnection con = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=JobPortalDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

        [Key]
        public int JobId { get; set; }

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

        //[Required(ErrorMessage = "Please enter CompanyLogo")]
        //public string? CompanyLogo { get; set; }

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
        //public List<AddJobModel> getData()
        //{
        //    List<AddJobModel> jobList = new List<AddJobModel>();
        //    SqlDataAdapter apt = new SqlDataAdapter("SELECT * FROM Jobs", con);
        //    DataSet ds = new DataSet();
        //    apt.Fill(ds);

        //    foreach (DataRow dr in ds.Tables[0].Rows)
        //    {
        //        jobList.Add(new AddJobModel
        //        {
        //            Id = Convert.ToInt32(dr["Id"]),
        //            Title = dr["Title"].ToString(),
        //            NoOfPost = dr["NoOfPost"].ToString(),
        //            Description = dr["Description"].ToString(),
        //            Qualification = dr["Qualification"].ToString(),
        //            Experience = dr["Experience"].ToString(),
        //            Specialization = dr["Specialization"].ToString(),
        //            LastDateToApply = dr["LastDateToApply"].ToString(),
        //            Salary = dr["Salary"].ToString(),
        //            JobType = dr["JobType"].ToString(),
        //            CompanyName = dr["CompanyName"].ToString(),
        //            //CompanyLogo = dr["CompanyLogo"].ToString(),
        //            Website = dr["Website"].ToString(),
        //            Email = dr["Email"].ToString(),
        //            Address = dr["Address"].ToString(),
        //            Country = dr["Country"].ToString(),
        //            State = dr["State"].ToString(),
        //        });
        //    }
        //    return jobList;
        //}

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
                    JobId = Convert.ToInt32(dr["JobId"]),
                    Title = dr["Title"].ToString(),
                    NoOfPost = dr["NoOfPost"].ToString(),
                    Description = dr["Description"].ToString(),
                    Qualification = dr["Qualification"].ToString(),
                    Experience = dr["Experience"].ToString(),
                    Specialization = dr["Specialization"].ToString(),
                    LastDateToApply = dr["LastDateToApply"].ToString(),
                    Salary = dr["Salary"].ToString(),
                    JobType = dr["JobType"].ToString(),
                    CompanyName = dr["CompanyName"].ToString(),
                    //CompanyLogo = dr["CompanyLogo"].ToString(),
                    Website = dr["Website"].ToString(),
                    Email = dr["Email"].ToString(),
                    Address = dr["Address"].ToString(),
                    Country = dr["Country"].ToString(),
                    State = dr["State"].ToString(),

                });
            }
            return lstUser;
        }

        //Insert a record into a database table
        public bool insert(AddJobModel re)
        {
            using (SqlConnection con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=JobPortalDB;Integrated Security=True;"))
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Jobs (Title, NoOfPost, Description, Qualification,Experience, Specialization, LastDateToApply, Salary, JobType, CompanyName, Website,Email, Address, Country, State) " +
                    "                                     VALUES (@Title, @NoOfPost, @Description, @Qualification, @Experience, @Specialization, @LastDateToApply, @Salary, @JobType, @CompanyName, @Website, @Email, @Address, @Country, @State)", con);

                cmd.Parameters.AddWithValue("@Title", re.Title);
                cmd.Parameters.AddWithValue("@NoOfPost", re.NoOfPost);
                cmd.Parameters.AddWithValue("@Description", re.Description);
                cmd.Parameters.AddWithValue("@Qualification", re.Qualification);
                cmd.Parameters.AddWithValue("@Experience", re.Experience);
                cmd.Parameters.AddWithValue("@Specialization", re.Specialization);
                cmd.Parameters.AddWithValue("@LastDateToApply", re.LastDateToApply);
                cmd.Parameters.AddWithValue("@Salary", re.Salary);
                cmd.Parameters.AddWithValue("@JobType", re.JobType);
                cmd.Parameters.AddWithValue("@CompanyName", re.CompanyName);
                cmd.Parameters.AddWithValue("@Website", re.Website);
                cmd.Parameters.AddWithValue("@Email", re.Email);
                cmd.Parameters.AddWithValue("@Address", re.Address);
                cmd.Parameters.AddWithValue("@Country", re.Country);
                cmd.Parameters.AddWithValue("@State", re.State);


                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();

                return i >= 1;
            }
        }

    }
}
