using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mk_Core_API_MVC.Models;
using Newtonsoft.Json;
using NuGet.Protocol.Plugins;
using System.Diagnostics;
using System.Security.Claims;

namespace Mk_Core_API_MVC.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {

            // 4th Step of using Session
            HttpContext.Session.SetString("Name", "MVC-Core Third Projact Using API");
            // 5th step of using Session
            var ses = HttpContext.Session.GetString("Name");

            //...........1st step...........
            HttpClient client = new HttpClient();

            //...........2nd step...........
            var data = client.GetAsync("http://localhost:5213/api/Manish/Api/GetAllData").Result;

            //...........3rd step...........
            var readdata = data.Content.ReadAsStringAsync().Result;

            //.....4th Convert JSON to Class.......... 
            var empres = JsonConvert.DeserializeObject<List<EmpModel>>(readdata);

            return View(empres);
        }

        //---------------Add Data in Table---------------------

        [HttpGet]
        public IActionResult AddEmp()
        {

            return View();
        }

        [HttpPost]
        public IActionResult AddEmp(EmpModel obj)
        {
            //...........1st step...........
            HttpClient client = new HttpClient();
            var data = JsonConvert.SerializeObject(obj);
            StringContent postdata = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            var result = client.PostAsync("http://localhost:5213/api/Manish/Api/AddData", postdata).Result;
            return RedirectToAction("Index");
        }

        //---------------Delete Data from Table---------------------
        public IActionResult Delete(int Id)
        {
            HttpClient client = new HttpClient();
            var result = client.GetAsync("http://localhost:5213/api/Manish/Api/DeleteData" + "?Id=" + Id).Result;
            return RedirectToAction("Index");
        }

        //---------------Edit Data from Table---------------------
        public IActionResult Edit(int Id)
        {
            HttpClient client = new HttpClient();
            var result = client.GetAsync("http://localhost:5213/api/Manish/Api/EditData" + "?Id=" + Id).Result;
            var data = result.Content.ReadAsStringAsync().Result;
            var objdata = JsonConvert.DeserializeObject<EmpModel>(data);
            return View("AddEmp", objdata);
        }

        //---------------Login in Table---------------------
        [AllowAnonymous]
        [HttpGet]

        public IActionResult login()
        {
            return View();
        }

         [AllowAnonymous]
        [HttpPost]

        public IActionResult login(login obj)
        {
            HttpClient client = new HttpClient();
            var dsta = JsonConvert.SerializeObject(obj);
            StringContent post =new StringContent(dsta,System.Text.Encoding.UTF8, "application/json");
            var res = client.PostAsync("http://localhost:5213/api/Manish/Api/Login"+"?Email="+obj.Email+"&Password="+obj.password, post).Result;
            var redda = res.Content.ReadAsStringAsync().Result;

            if (redda == "no")
            {
                TempData["Invalid Email"] = "Email Not Found..............";
               
                return RedirectToAction("Index");
            }
            else
            {
                if (redda == "Yes")
                {
                    // Step-1 for Auth
                    var Claims = new[]{ new Claim(ClaimTypes.Name, obj.Email),
                                        new Claim(ClaimTypes.Email, obj.password)};
                    // Step-2 for Auth
                    var identity = new ClaimsIdentity(Claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true
                    };
                    // Step-3 for Auth
                    HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(identity),
                        authProperties);

                    HttpContext.Session.SetString("Email",obj.Email);
                    HttpContext.Session.GetString("Email");

                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Invalid Password"] = "Password is Invalid........... ";
                }
                return View();
            }
        }

        //----------------LogOut Method-----------------------
        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync(null, null);
            HttpContext.Session.Clear();
            return View("login");
        }

        //---------------Regestraiton Form---------------------
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Regestration()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Regestration(login obj)
        {
            
            HttpClient client = new HttpClient();
            var data = JsonConvert.SerializeObject(obj);
            StringContent postdata = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            var result = client.PostAsync("http://localhost:5213/api/Login/Api/EditLoginData", postdata).Result;
            var read=result.Content.ReadAsStringAsync().Result;
            if (read == "no")
            {
                TempData["Email"] = "This Email is Already Exist.........!";
            }
            else
            {
                return RedirectToAction("login");
            }
            return View();
        }

        //------------------Show Login Table Data-------------------
        public IActionResult Privacy()
        {
            HttpClient client = new HttpClient();
            var data = client.GetAsync("http://localhost:5213/api/Login/Api/GetloginData").Result;
            var readdata = data.Content.ReadAsStringAsync().Result;
            var empres = JsonConvert.DeserializeObject<List<login>>(readdata);
            return View(empres);
        }

        //---------------Delete Login table Data---------------------
        public IActionResult DeleteLogin(int Id)
        {
            HttpClient client = new HttpClient();
            var result = client.GetAsync("http://localhost:5213/api/Login/Api/DeleteData" + "?Id=" + Id).Result;
            return RedirectToAction("Privacy");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}