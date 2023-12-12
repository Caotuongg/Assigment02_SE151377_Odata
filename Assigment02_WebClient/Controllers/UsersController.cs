using Assigment02_BussinessObject;
using Assigment02_WebClient.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Assigment02_WebClient.Controllers
{
    public class UsersController : Controller
    {
        private readonly HttpClient _httpClient;
        private string UserApiUrl = "https://localhost:7103/api/User/";

        public UsersController()
        {
            _httpClient = new HttpClient();
        }

        public IActionResult Login()
        {
            try
            {
                if (!string.IsNullOrEmpty(TempData["LoginFail"] as string))
                {
                    throw new Exception(TempData["LoginFail"] as string);
                }
                return View();
            }
            catch (Exception ex)
            {
                ViewData["ErrMsg"] = ex.Message;
                return View();
            }
        }

        // POST: Members/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            HttpResponseMessage message = await _httpClient.PostAsJsonAsync(UserApiUrl + "Login", new
            {
                email = email,
                password = password
            });



            string resData = await message.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(resData);

            if (message.StatusCode == HttpStatusCode.NotFound || message.StatusCode == HttpStatusCode.InternalServerError)
            {
                TempData["LoginFail"] = json["message"].ToString();
                return RedirectToAction(nameof(Login));
            }
            if (message.StatusCode == HttpStatusCode.OK)
            {
                var tokenJson = JsonConvert.DeserializeObject<Dictionary<string, object>>(json["token"].ToString());
                HttpContext.Session.SetString("Token", tokenJson["token"].ToString());
                HttpContext.Session.SetString("RefreshToken", tokenJson["refreshToken"].ToString());

                HttpContext.Session.SetString("Role", json["role"].ToString());

                if (json["role"].ToString() == "Admin")
                {
                    HttpContext.Session.SetString("Name", "Admin");
                    return RedirectToAction("AdminIndex", "Home");
                }
                else if (json["role"].ToString() == "Customer")
                {
                    HttpContext.Session.SetString("Name", "Customer");
                    return RedirectToAction("UserIndex", "Home");
                }
            }
            return RedirectToAction(nameof(Index));
        }



        public async Task<IActionResult> Index()
        {
            var role = HttpContext.Session.GetString("Role");
            if (string.IsNullOrEmpty(role))
            {
                TempData["LoginFail"] = "You are not login";
                return RedirectToAction("Login", "Users");
            }
            ViewData["Role"] = role;
            if (role == "Admin")
            {
                HttpResponseMessage message = await _httpClient.GetAsync(UserApiUrl + "GetAll");

                string resData = await message.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };

                var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(resData);

                if (json["status"] == "Error")
                {
                    throw new Exception(json["Message"].ToString());
                }

                var mems = JsonConvert.DeserializeObject<List<User>>(json["data"].ToString());
                return View(mems);
            }
            return RedirectToAction("Index", "Users");
        }

        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Signup(UserVM user)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(user.email_address))
                {
                    throw new Exception("Email cannot be empty!!!");
                }
                if (string.IsNullOrWhiteSpace(user.password))
                {
                    throw new Exception("Password cannot be empty!!!");
                }
                if (int.TryParse(user.password, out int result) && result < 0)
                {
                    throw new Exception("Password cannot be a negative number!");
                }
                if (string.IsNullOrWhiteSpace(user.source))
                {
                    throw new Exception("Source cannot be empty!!!");
                }
                if (string.IsNullOrWhiteSpace(user.first_name))
                {
                    throw new Exception("First Name cannot be empty!!!");
                }
                if (string.IsNullOrWhiteSpace(user.middle_name))
                {
                    throw new Exception("Middle cannot be blank!!!");
                }
                if (string.IsNullOrWhiteSpace(user.last_name))
                {
                    throw new Exception("Last cannot be blank!!!");
                }
                
                HttpResponseMessage reponse = await _httpClient.PostAsJsonAsync(UserApiUrl + "AddUser", user);
                if (reponse.StatusCode == HttpStatusCode.OK)
                {
                    return RedirectToAction("Login");
                }
                if (reponse.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var data = await reponse.Content.ReadAsStringAsync();
                    var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
                    throw new Exception(json["reponse"].ToString());
                }
                return RedirectToAction("Signup");
            }
            catch
            {
                return View(user);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {

            var role = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(role))
            {
                TempData["LoginFail"] = "You are not login";
                return RedirectToAction("Login", "Users");
            }
            ViewData["Role"] = role;
            string url = "";
            if (role != "Admin")
            {
                url = UserApiUrl + "GetUserById";
            }
            else
            {
                url = UserApiUrl + $"GetUserById?id={id}";
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));

            HttpResponseMessage message = await _httpClient.GetAsync(url);

            if (message.StatusCode == HttpStatusCode.OK)
            {


                string resData = await message.Content.ReadAsStringAsync();

                var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(resData);

                var mem = JsonConvert.DeserializeObject<User>(json["data"].ToString());

                return View(mem);
            }
            else if (message.StatusCode == HttpStatusCode.Unauthorized)
            {
                TempData["LoginFail"] = "You are not login";
                return RedirectToAction("Login", "Users");
            }


            return RedirectToAction("AdminIndex", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var role = HttpContext.Session.GetString("Role");
            if (string.IsNullOrEmpty(role))
            {
                TempData["LoginFail"] = "You are not login";
                return RedirectToAction("Login", "Users");
            }
            ViewData["Role"] = role;
            if (role != "Admin")
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));

                HttpResponseMessage message = await _httpClient.GetAsync(UserApiUrl + $"GetUserById");

                if (message.StatusCode == HttpStatusCode.OK)
                {
                    string resData = await message.Content.ReadAsStringAsync();

                    var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(resData);

                    var mem = JsonConvert.DeserializeObject<User>(json["data"].ToString());

                    return View(mem);
                }
                else if (message.StatusCode == HttpStatusCode.Unauthorized)
                {
                    TempData["LoginFail"] = "You are not login";
                    return RedirectToAction("Login", "Users");
                }
            }
            return RedirectToAction("AdminIndex", "Home");
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, User user)
        {
            var role = HttpContext.Session.GetString("Role");
            ViewData["Role"] = role;
            if (string.IsNullOrEmpty(role))
            {
                TempData["LoginFail"] = "You are not login";
                return RedirectToAction("Login", "Users");
            }
            try
            {
                if (role != "Admin")
                {
                    if (string.IsNullOrWhiteSpace(user.email_address))
                    {
                        throw new Exception("Email cannot be empty!!!");
                    }
                    if (string.IsNullOrWhiteSpace(user.source))
                    {
                        throw new Exception("Source cannot be empty!!!");
                    }
                    if (string.IsNullOrWhiteSpace(user.first_name))
                    {
                        throw new Exception("Firstname cannot be empty!!!");
                    }
                    if (string.IsNullOrWhiteSpace(user.middle_name))
                    {
                        throw new Exception("MiddleName cannot be empty!!!");
                    }
                    if (string.IsNullOrWhiteSpace(user.last_name))
                    {
                        throw new Exception("Last Name cannot be empty!!!");
                    }
                    if (string.IsNullOrWhiteSpace(user.password))
                    {
                        throw new Exception("Password cannot be empty!!!");
                    }
                    if (int.TryParse(user.password, out int result) && result < 0)
                    {
                        throw new Exception("Password cannot be a negative number!");
                    }


                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));

                    HttpResponseMessage message = await _httpClient.PutAsJsonAsync(UserApiUrl + $"UpdateUser?id={user.user_id}", user);

                    if (message.StatusCode == HttpStatusCode.OK)
                    {
                        
                        return RedirectToAction(nameof(Details));

                    }
                    else if (message.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        TempData["LoginFail"] = "You are not login";
                        return RedirectToAction("Login", "Users");
                    }
                    else if (message.StatusCode == HttpStatusCode.InternalServerError || message.StatusCode == HttpStatusCode.NotFound)
                    {
                        string resData = await message.Content.ReadAsStringAsync();
                        var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(resData);
                        ViewData["ErrMsg"] = json["message"];
                        return View(user);
                    }
                }
                return RedirectToAction("AdminIndex", "Home");
            }
            catch (Exception ex)
            {
                ViewData["ErrMsg"] = ex.Message;
                return View("Details", user);
            }
        }




        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            var role = HttpContext.Session.GetString("Role");
            if (string.IsNullOrEmpty(role))
            {
                TempData["LoginFail"] = "You are not login";
                return RedirectToAction("Login", "Users");
            }
            ViewData["Role"] = role;
            if (role == "Admin")
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
                HttpResponseMessage response = await _httpClient.GetAsync(UserApiUrl + $"GetUserById?id={id}");
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);

                    var mem = JsonConvert.DeserializeObject<User>(json["data"].ToString());
                    return View(mem);
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    TempData["LoginFail"] = "You are not login";
                    return RedirectToAction("Login", "Users");
                }
                else if (response.StatusCode == HttpStatusCode.InternalServerError || response.StatusCode == HttpStatusCode.NotFound)
                {
                    string resData = await response.Content.ReadAsStringAsync();
                    var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(resData);
                    ViewData["ErrMsg"] = json["response"];

                    return RedirectToAction(nameof(Index));
                }

            }
            return RedirectToAction("Index", "Users");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var role = HttpContext.Session.GetString("Role");
            if (string.IsNullOrEmpty(role))
            {
                TempData["LoginFail"] = "You are not login";
                return RedirectToAction("Login", "Users");
            }
            if (role == "Admin")
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
                HttpResponseMessage response = await _httpClient.DeleteAsync(UserApiUrl + $"DeleteUser?id={id}");
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return RedirectToAction(nameof(Index));
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    TempData["LoginFail"] = "You are not login";
                    return RedirectToAction("Login", "Users");
                }
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Role")))
            {
                HttpContext.Session.Remove("Role");
                HttpContext.Session.Remove("Token");
                HttpContext.Session.Remove("RefreshToken");
                HttpContext.Session.Remove("Name");
                HttpContext.Session.Remove("Cart");
            }
            return RedirectToAction("Login");
        }


        [HttpGet]
        public IActionResult Error()
        {
            var role = HttpContext.Session.GetString("Role");
            ViewData["Role"] = role;

            return View();
        }
    }
}
