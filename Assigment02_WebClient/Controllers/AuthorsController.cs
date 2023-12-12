using Assigment02_BussinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text.RegularExpressions;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace Assigment02_WebClient.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly HttpClient _httpClient;
        private string AuthorApiUrl = "https://localhost:7103/api/Author/";

        public AuthorsController()
        {
            _httpClient = new HttpClient();
        }



        public async Task<ActionResult> Index()
        {

            HttpResponseMessage message = await _httpClient.GetAsync(AuthorApiUrl + "GetAll");
            if (message.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string data = await message.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
                var authors = JsonConvert.DeserializeObject<List<Author>>(json["data"].ToString());
                return View(authors);
            }
            return RedirectToAction("Index", "Authors");
        }

        public async Task<IActionResult> Details(int? id)
        {
            var role = HttpContext.Session.GetString("Role");
            ViewData["Role"] = role;

            HttpResponseMessage message = await _httpClient.GetAsync(AuthorApiUrl + $"GetAuthorById?id={id}");

            if (message.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var resData = await message.Content.ReadAsStringAsync();

                var product = ConvertAuthor(resData);
                return View(product);
            }
            if (message.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return RedirectToAction(nameof(Error));
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            CheckAdmin();

            var role = HttpContext.Session.GetString("Role");
            ViewData["Role"] = role;

            //ViewData["CategoryId"] = new SelectList(await GetCategories(), "CategoryId", "CategoryName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Author author)
        {
            try
            {
                CheckAdmin();

                var role = HttpContext.Session.GetString("Role");
                ViewData["Role"] = role;

                CheckValidate(author);

                var token = HttpContext.Session.GetString("Token");
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage message = await _httpClient.PostAsJsonAsync(AuthorApiUrl + "AddAuthor", author);
                if (message.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    TempData["LoginFail"] = "You are not login";
                    return RedirectToAction("Login", "Users");
                }
                if (message.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    string resData = await message.Content.ReadAsStringAsync();

                    var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(resData);

                    throw new Exception(json["message"].ToString());

                }

                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                ViewData["ErrMsg"] = ex.Message;
                //ViewData["CategoryId"] = new SelectList(await GetCategories(), "CategoryId", "CategoryName", product.CategoryId);
                return View(author);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            CheckAdmin();

            var role = HttpContext.Session.GetString("Role");
            ViewData["Role"] = role;

            HttpResponseMessage message = await _httpClient.GetAsync(AuthorApiUrl + $"GetAuthorById?id={id}");

            if (message.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var resData = await message.Content.ReadAsStringAsync();

                var product = ConvertAuthor(resData);
                return View(product);
            }
            if (message.StatusCode == System.Net.HttpStatusCode.NotFound || message.StatusCode == System.Net.HttpStatusCode.InternalServerError || message.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                return RedirectToAction(nameof(Error));
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Author author)
        {
            CheckAdmin();

            var role = HttpContext.Session.GetString("Role");
            ViewData["Role"] = role;
            try
            {

                CheckValidate(author);

                var token = HttpContext.Session.GetString("Token");

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage message = await _httpClient.PutAsJsonAsync(AuthorApiUrl + $"UpdateAuthor?id={author.author_id}", author);


                if (message.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    TempData["LoginFail"] = "You are not login";
                    return RedirectToAction("Login", "Users");
                }

                if (message.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    string resData = await message.Content.ReadAsStringAsync();

                    var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(resData);

                    throw new Exception(json["message"].ToString());
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                
                ViewData["ErrMsg"] = ex.Message;
                return View(author);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            CheckAdmin();

            var role = HttpContext.Session.GetString("Role");
            ViewData["Role"] = role;

            HttpResponseMessage message = await _httpClient.GetAsync(AuthorApiUrl + $"GetAuthorById?id={id}");

            if (message.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var resData = await message.Content.ReadAsStringAsync();

                var product = ConvertAuthor(resData);

                return View(product);
            }
            if (message.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return RedirectToAction(nameof(Error));
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            CheckAdmin();

            var token = HttpContext.Session.GetString("Token");

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage message = await _httpClient.DeleteAsync(AuthorApiUrl + $"DeleteAuthor?id={id}");

            if (message.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction(nameof(Index));
            }

            if (message.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                TempData["LoginFail"] = "You are not login";
                return RedirectToAction("Login", "Members");
            }

            if (message.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                HttpResponseMessage getProduct = await _httpClient.GetAsync(AuthorApiUrl + $"GetAuthorById?id={id}");

                if (message.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var resDataPro = await getProduct.Content.ReadAsStringAsync();

                    var product = ConvertAuthor(resDataPro);

                    string resData = await message.Content.ReadAsStringAsync();

                    var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(resData);

                    ViewData["ErrMsg"] = json["message"].ToString();

                    return View(product);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        private Author ConvertAuthor(string resData)
        {
            var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(resData);

            var author = JsonConvert.DeserializeObject<Author>(json["data"].ToString());

            return author;
        }

        private void CheckValidate(Author author)
        {
            if (string.IsNullOrWhiteSpace(author.last_name))
            {
                throw new Exception("LastName must not be empty and cannot contain numbers!");
            }
            if (string.IsNullOrWhiteSpace(author.first_name))
            {
                throw new Exception("FirstName can not be empty!!!");
            }
            if (string.IsNullOrWhiteSpace(author.email_address))
            {
                throw new Exception("Email can not be empty!!!");
            }
            if (string.IsNullOrWhiteSpace(author.phone))
            {
                throw new Exception("Phone can not be empty!!!");
            }
            if (int.TryParse(author.phone, out int result) && result < 0)
            {
                throw new Exception("Phone cannot be a negative number!");
            }
            if (string.IsNullOrWhiteSpace(author.address))
            {
                throw new Exception("Address can not be empty!!!");
            }
            if (string.IsNullOrWhiteSpace(author.city))
            {
                throw new Exception("City can not be empty!!!");
            }
            if (string.IsNullOrWhiteSpace(author.state))
            {
                throw new Exception("State can not be empty!!!");
            }
            if (string.IsNullOrWhiteSpace(author.zip))
            {
                throw new Exception("Zip can not be empty!!!");
            }
        }

        
        private void CheckAdmin()
        {
            var role = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(role))
            {
                TempData["LoginFail"] = "You are not login";
                RedirectToAction("Login", "Users");
            }
            if (role != "Admin")
            {
                RedirectToAction("UserIndex", "Home");
            }
        }


        [HttpGet]
        public IActionResult Error()
        {
            var role = HttpContext.Session.GetString("Role");
            ViewData["Role"] = role;

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> LogoutAuthor()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Role")))
            {
                HttpContext.Session.Remove("Role");
                HttpContext.Session.Remove("Token");
                HttpContext.Session.Remove("RefreshToken");
                HttpContext.Session.Remove("Name");
                HttpContext.Session.Remove("Cart");
            }
            return RedirectToAction("AdminIndex");
        }
    }
}
