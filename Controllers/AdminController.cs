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

        public IActionResult AdminDashboard()
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
            return View();
        }


        public IActionResult ContactList()
        {
            return View();
        }
    }
}
