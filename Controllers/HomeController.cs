using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Online_Job_Portal_MVC.Models;

namespace Online_Job_Portal_MVC.Controllers
{
    public class HomeController : Controller
    {
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
        public IActionResult Login(LoginModel lg)
        {
            bool res;
            if (ModelState.IsValid)
            {
                LogObj = new LoginModel();
                res = LogObj.insert(lg);
                if (res)
                {
                    TempData["msg"] = "Login successfully";
                }
                else
                {
                    TempData["msg"] = "Not Login. Something went wrong..!!";
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
