using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Online_Job_Portal_MVC.Models;
using Microsoft.AspNetCore.Mvc;

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
            ViewBag.Countries = new List<string> { "India", "USA", "Canada", "UK", "Australia" };
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
        //[HttpPost]
        //public IActionResult CreateJob(JobModel job)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Save job details to the database
        //        TempData["SuccessMessage"] = "Job posted successfully!";
        //        return RedirectToAction("JobList");
        //    }
        //    ViewBag.Countries = new List<string> { "India", "USA", "Canada", "UK", "Australia" };
        //    return View("NewJob");
        //}

        public IActionResult JobList()
        {
            return View();
        }


        public IActionResult ViewResume()
        {
            return View();
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
