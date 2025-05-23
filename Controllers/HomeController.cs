﻿using System.Configuration;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Online_Job_Portal_MVC.Models;

namespace Online_Job_Portal_MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<HomeController> _logger;

        ResumeModel RM = new ResumeModel();
        RegisterModel RegObj = new RegisterModel();
        LoginModel LogObj = new LoginModel();
        ContactModel conobj = new ContactModel();

       
        public HomeController(IConfiguration configuration, ILogger<HomeController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        
        public IActionResult JobListing()
        {
            List<AddJobModel> jobs = new List<AddJobModel>();
            string connectionString = _configuration.GetConnectionString("con");

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Jobs"; // Change table name as needed
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    jobs.Add(new AddJobModel
                    {
                        JobId = Convert.ToInt32(reader["JobId"]),
                        Title = reader["Title"].ToString(),
                        CompanyName = reader["CompanyName"].ToString(),
                        Country = reader["Country"].ToString(),
                        JobType = reader["JobType"].ToString(),
                        Salary = reader["Salary"].ToString(),
                        LastDateToApply = reader["LastDateToApply"].ToString()
                    });
                }
                con.Close();
            }

            ViewBag.TotalJobs = jobs.Count;
            return View(jobs);
        }


       
        public IActionResult JobDetails(int id)
        {
            AddJobModel job = new AddJobModel();
            string connectionString = _configuration.GetConnectionString("con");

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Jobs WHERE JobId = @JobId";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@JobId", id);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    job.JobId = Convert.ToInt32(reader["JobId"]);
                    job.Title = reader["Title"].ToString();
                    job.CompanyName = reader["CompanyName"].ToString();
                    job.Country = reader["Country"].ToString();
                    job.JobType = reader["JobType"].ToString();
                    job.Salary = reader["Salary"].ToString();
                    job.LastDateToApply = reader["LastDateToApply"].ToString();
                    job.Description = reader["Description"].ToString();
                }
            }

            return View(job);
        }

        [HttpPost]
        public IActionResult ApplyForJob(int JobId)
        {
            string username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                TempData["msg"] = "Login required to apply.";
                return RedirectToAction("Login");
            }

            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("con")))
            {
                string query = "INSERT INTO AppliedJobs (JobId, Username) VALUES (@JobId, @Username)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@JobId", JobId);
                cmd.Parameters.AddWithValue("@Username", username);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            TempData["msg"] = "Successfully applied!";
            return RedirectToAction("UserProfile");
        }



        public IActionResult Contact()
        {
            conobj = new ContactModel();
            List<ContactModel> lst = conobj.getData(); // Fetches all the records
            return View(new ContactModel());
        }


        public IActionResult UserProfile()
        {
            return View();
        }


        public IActionResult UserResumeBulid()
        {
            return View();
        }


        public IActionResult ResumeBulid()
        {
            RM = new ResumeModel();
            List<ResumeModel> lst = RM.getData(); // Fetches all the records
            return View(new ResumeModel());
        }

        public IActionResult Register()
        {
            RegObj = new RegisterModel();
            List<RegisterModel> lst = RegObj.getData(); // Fetches all the records
            return View(new RegisterModel());
        }

        public IActionResult Login()
        {
            LogObj = new LoginModel();
            List<LoginModel> lst = LogObj.getData(); // Fetches all the records
            return View(new LoginModel());
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // ✅ Remove all session data
            TempData["msg"] = "Logged out successfully!";
            return RedirectToAction("Login", "Home"); // ✅ Redirect to login page
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult Register(RegisterModel emp)
        {
            bool res;
            if (ModelState.IsValid)
            {
                RegObj = new RegisterModel();
                res = RegObj.insert(emp);
                if (res)
                {
                    TempData["msg"] = "Added successfully";
                }
                else
                {
                    TempData["msg"] = "Not Added. Something went wrong..!!";
                }
            }
            return View();
        }

       
        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Please enter all required fields!";
                return View(model);
            }

            using (SqlConnection con = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=JobPortalDB;Integrated Security=True;"))
            {
                string query = "SELECT Username, Role FROM Register WHERE Username = @Username AND Password = @Password";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Username", model.Username);
                    cmd.Parameters.AddWithValue("@Password", model.Password);

                    con.Open();
                    SqlDataReader sdr = cmd.ExecuteReader();

                    if (sdr.Read()) // ✅ User found in database
                    {
                        string username = sdr["Username"].ToString();
                        string role = sdr["Role"].ToString();

                        // ✅ Store in session
                        HttpContext.Session.SetString("Username", username);
                        HttpContext.Session.SetString("Role", role);

                        TempData["msg"] = "Login successful!";

                        if (role == "Admin")
                        {
                            return RedirectToAction("AdminDashboard", "Admin"); // ✅ Redirect Admin
                        }
                        else
                        {
                            return RedirectToAction("UserProfile", "Home"); // ✅ Redirect User
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Invalid Username or Password!";
                        return View(model);
                    }
                }
            }
        }



        

        [HttpPost]
        public IActionResult ResumeBulid(ResumeModel emp)
        {
            if (ModelState.IsValid)
            {
                // Save file to wwwroot/ResumeUploads
                if (emp.UploadResumeFile != null)
                {
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ResumeUploads");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + emp.UploadResumeFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        emp.UploadResumeFile.CopyTo(fileStream);
                    }

                    emp.UploadResume = "/ResumeUploads/" + uniqueFileName; // Save path to DB
                }

                RM = new ResumeModel();
                bool res = RM.insert(emp);

                if (res)
                {
                    TempData["msg"] = "Added successfully";
                }
                else
                {
                    TempData["msg"] = "Not Added. Something went wrong..!!";
                }
            }

            return View();
        }



       

        [HttpPost]
        public IActionResult Contact(ContactModel lg)
        {
            bool res;
            if (ModelState.IsValid)
            {
                conobj = new ContactModel();
                res = conobj.insert(lg);
                if (res)
                {
                    TempData["msg"] = "Added successfully";
                }
                else
                {
                    TempData["msg"] = "Not Added. Something went wrong..!!";
                }
            }
            return View();
        }


    }
}