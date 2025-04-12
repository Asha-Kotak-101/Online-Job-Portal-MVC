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
        public AdminController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        //public IActionResult AdminDashboard()
        //{
        //    return View();
        //}
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

        //[HttpPost]
        //public IActionResult NewJob(AddJobModel emp)
        //{
        //    bool res;
        //    if (ModelState.IsValid)
        //    {
        //        job = new AddJobModel();
        //        res = job.insert(emp);
        //        if (res)
        //        {
        //            TempData["msg"] = "New Job Added successfully";
        //        }
        //        else
        //        {
        //            TempData["msg"] = "Not Added. Something went wrong..!!";
        //        }
        //    }
        //    return View();
        //}


        //[HttpPost]
        //public IActionResult NewJob(AddJobModel emp)
        //{
        //    foreach (var state in ModelState)
        //    {
        //        Console.WriteLine($"Key: {state.Key}, Errors: {string.Join(", ", state.Value.Errors.Select(e => e.ErrorMessage))}");
        //    }

        //    if (!ModelState.IsValid)
        //    {
        //        // Collect validation errors
        //        var errors = ModelState.Values.SelectMany(v => v.Errors)
        //                                      .Select(e => e.ErrorMessage)
        //                                      .ToList();

        //        // Store errors in TempData
        //        TempData["msg"] = "⚠️ Validation Failed! Errors: " + string.Join(", ", errors);

        //        return View(emp);  // Return the form with errors
        //    }

        //    job = new AddJobModel();
        //    bool res = job.insert(emp);

        //    if (res)
        //    {
        //        TempData["msg"] = "✅ New Job Added Successfully!";
        //        return RedirectToAction("JobList");
        //    }
        //    else
        //    {
        //        TempData["msg"] = "❌ Error: Job Not Added!";
        //    }

        //    return View(emp);
        //}



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


        //public IActionResult ViewResume()
        //{
        //    return View();
        //}
        public IActionResult ViewResume()
        {
            List<JobWithUserViewModel> resumeList = new List<JobWithUserViewModel>();

            using (SqlConnection con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=JobPortalDB;Integrated Security=True;"))
            {
                con.Open();
                string query = @"
        SELECT 
            j.CompanyName, j.Title, 
            r.FullName, r.Email, r.MobileNumber 
        FROM Jobs j
        JOIN Register r ON j.Email = r.Email;";

                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    resumeList.Add(new JobWithUserViewModel
                    {
                        Job = new AddJobModel
                        {
                            CompanyName = reader["CompanyName"].ToString(),
                            Title = reader["Title"].ToString()
                        },
                        User = new RegisterModel
                        {
                            Fullname = reader["Fullname"].ToString(),
                            Email = reader["Email"].ToString(),
                            MobileNumber = reader["MobileNumber"].ToString()
                        }
                    });
                }
            }

            return View(resumeList);  // Pass data to the View
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

    }
}
