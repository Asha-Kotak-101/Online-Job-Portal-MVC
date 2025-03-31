using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Online_Job_Portal_MVC.Models;

namespace Online_Job_Portal_MVC.Controllers
{
    public class HomeController : Controller
    {
        ResumeModel RM = new ResumeModel();
        RegisterModel RegObj = new RegisterModel();
        LoginModel LogObj = new LoginModel();
        ContactModel conobj = new ContactModel();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
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
            return View();
        }

        public IActionResult JobDetails()
        {
            return View();
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

        //[HttpPost]
        //public IActionResult Login(LoginModel lg)
        //{
        //    bool res;
        //    if (ModelState.IsValid)
        //    {
        //        LogObj = new LoginModel();
        //        res = LogObj.insert(lg);
        //        if (res)
        //        {
        //            TempData["msg"] = "Login successfully";
        //        }
        //        else
        //        {
        //            TempData["msg"] = "Not Login. Something went wrong..!!";
        //        }
        //    }
        //    return View();
        //}

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
            bool res;
            if (ModelState.IsValid)
            {
                RM = new ResumeModel();
                res = RM.insert(emp);
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