using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Online_Job_Portal_MVC.Models;

namespace Online_Job_Portal_MVC.Controllers
{
    public class AdminController : Controller
    {
        AddJobModel job = new AddJobModel();
        private readonly ILogger<HomeController> _logger;

        private readonly string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=JobPortalDB;Integrated Security=True;";

        public AdminController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

       
        public IActionResult AdminDashboard()
        {
            DashboardViewModel model = new DashboardViewModel();

            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=JobPortalDB;Integrated Security=True;";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                // Get Total Users
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Register WHERE Role = 'User'", con))
                {
                    model.TotalUsers = (int)cmd.ExecuteScalar();
                }

                // Get Total Jobs
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Jobs", con))
                {
                    model.TotalJobs = (int)cmd.ExecuteScalar();
                }

                // Get Total Applied Jobs
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Resume", con))
                {
                    model.TotalAppliedJobs = (int)cmd.ExecuteScalar();
                }

                // Get Contacted Users
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Contact", con))
                {
                    model.ContactedUsers = (int)cmd.ExecuteScalar();
                }
            }

            return View(model);
        }


        public IActionResult AdminProfile()
        {
            return View();
        }

        public IActionResult NewJob()
        {
            job = new AddJobModel();
            List<AddJobModel> lst = job.getData(); // Fetches all the records
            return View(new AddJobModel());
        }



       


        [HttpPost]
        public IActionResult NewJob(AddJobModel emp)
        {
            bool res;
            if (ModelState.IsValid)
            {
                job = new AddJobModel();
                res = job.insert(emp);
                if (res)
                {
                    TempData["msg"] = "New Job Added successfully";
                }
                else
                {
                    TempData["msg"] = "Not Added. Something went wrong..!!";
                }
            }
            return View();
        }

        public IActionResult JobList()
        {
            job = new AddJobModel();
            List<AddJobModel> jobList = job.getData();  // Fetch all jobs
            return View(jobList);
        }


        public IActionResult ViewResume()
        {
            List<JobWithUserViewModel> resumeList = new List<JobWithUserViewModel>();

            using (SqlConnection con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=JobPortalDB;Integrated Security=True;"))
            {
                con.Open();
                string query = @"SELECT Id, Fullname, Email, MoblieNumber, UploadResume FROM Resume";

                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    resumeList.Add(new JobWithUserViewModel
                    {
                        User = new RegisterModel
                        {
                            Fullname = reader["Fullname"].ToString(),
                            Email = reader["Email"].ToString(),
                            MobileNumber = reader["MoblieNumber"].ToString()
                        },
                        ResumePath = reader["UploadResume"].ToString(),
                        ResumeId = Convert.ToInt32(reader["Id"])
                    });
                }
            }

            return View(resumeList);
        }



        public IActionResult UserList()
        {
            List<UserListModel> users = new List<UserListModel>();

            using (SqlConnection con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=JobPortalDB;Integrated Security=True;"))
            {
                string query = "SELECT Username, Email, MobileNumber, Country FROM Register";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    users.Add(new UserListModel
                    {
                        GetUser = new RegisterModel
                        {
                            Username = reader["Username"].ToString(),
                            Email = reader["Email"].ToString(),
                            MobileNumber = reader["MobileNumber"].ToString(),
                            Country = reader["Country"].ToString()
                        }
                    });
                }
                reader.Close();
            }

            return View(users); // Pass the user list to the view
        }



        public IActionResult ContactList()
        {
            List<UserListModel> contacts = new List<UserListModel>();

            using (SqlConnection con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=JobPortalDB;Integrated Security=True;"))
            {
                string query = "SELECT *  FROM Contact";  // Assuming contact details are stored in Contact table
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    contacts.Add(new UserListModel
                    {
                        GetContact = new ContactModel
                        {
                            FullName = reader["FullName"].ToString(),
                            Email = reader["Email"].ToString(),
                            Message = reader["Message"].ToString(),
                            Subject = reader["Subject"].ToString()
                        }
                    });
                }
                reader.Close();
            }

            return View(contacts); // Pass the contact list to the view
        }


        [HttpPost]
        public IActionResult DeleteUser(string email)
        {
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=JobPortalDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Register WHERE Email = @Email";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            TempData["msg"] = "User deleted successfully.";
            return RedirectToAction("UserList");
        }


        [HttpPost]
        public IActionResult DeleteContact(string email)
        {
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=JobPortalDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Contact WHERE Email = @Email";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            TempData["msg"] = "User Details deleted successfully.";
            return RedirectToAction("ContactList");
        }

        [HttpPost]
        public IActionResult DeleteJob(int id)
        {
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=JobPortalDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"; // Replace with your actual connection string

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Jobs WHERE JobId = @JobId";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@JobId", id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            TempData["msg"] = "Job deleted successfully.";
            return RedirectToAction("JobList");
        }

      

        public IActionResult DownloadResume(int id)
        {
            using (SqlConnection con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=JobPortalDB;Integrated Security=True;"))
            {
                con.Open();
                string query = "SELECT UploadResume FROM Resume WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Id", id);
                string? relativePath = cmd.ExecuteScalar()?.ToString();

                if (!string.IsNullOrEmpty(relativePath))
                {
                    // Build full file path
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath.TrimStart('/'));

                    // Log path for debugging
                    Console.WriteLine("Checking file path: " + filePath);

                    if (System.IO.File.Exists(filePath))
                    {
                        byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
                        string fileName = Path.GetFileName(filePath);
                        return File(fileBytes, "application/octet-stream", fileName);
                    }
                    else
                    {
                        Console.WriteLine("File does NOT exist at: " + filePath);
                    }
                }
            }

            TempData["msg"] = "File not found.";
            return RedirectToAction("ViewResume");
        }





        public IActionResult DeleteResume(int id)
        {
            using (SqlConnection con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=JobPortalDB;Integrated Security=True;"))
            {
                con.Open();

                // ✅ Corrected column name
                string getPathQuery = "SELECT UploadResume FROM Resume WHERE Id=@Id";
                SqlCommand getPathCmd = new SqlCommand(getPathQuery, con);
                getPathCmd.Parameters.AddWithValue("@Id", id);
                string? fileName = getPathCmd.ExecuteScalar()?.ToString();

                string deleteQuery = "DELETE FROM Resume WHERE Id=@Id";
                SqlCommand deleteCmd = new SqlCommand(deleteQuery, con);
                deleteCmd.Parameters.AddWithValue("@Id", id);
                int result = deleteCmd.ExecuteNonQuery();

                if (result > 0 && !string.IsNullOrEmpty(fileName))
                {
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ResumeUploads", fileName);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                    TempData["msg"] = "Resume deleted successfully!";
                }
                else
                {
                    TempData["msg"] = "Resume not deleted or file not found.";
                }
            }

            return RedirectToAction("ViewResume");
        }


        public IActionResult ViewApplicants(int jobId)
        {
            List<(string Username, DateTime AppliedDate)> applicants = new List<(string, DateTime)>();

            using (SqlConnection con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=JobPortalDB;Integrated Security=True;"))
            {
                string query = "SELECT Username, AppliedDate FROM AppliedJobs WHERE JobId = @JobId";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@JobId", jobId);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    applicants.Add((
                        reader["Username"].ToString(),
                        Convert.ToDateTime(reader["AppliedDate"])
                    ));
                }
                con.Close();
            }

            ViewBag.JobId = jobId;
            return View(applicants);
        }


        public IActionResult ViewAppliedJob(int jobId)
        {
            List<AppliedJobsModel> applicants = new List<AppliedJobsModel>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT Username, AppliedDate FROM AppliedJobs WHERE JobId = @JobId";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@JobId", jobId);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    applicants.Add(new AppliedJobsModel
                    {
                        UserName = reader["UserName"].ToString(),
                        AppliedDate = Convert.ToDateTime(reader["AppliedDate"])
                    });
                }
                con.Close();
            }

            ViewBag.JobId = jobId;
            return View(applicants);
        }
    }
}



    

