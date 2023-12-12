using Assigment02_BussinessObject;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;

namespace Assigment02_WebClient.Controllers
{
    public class PublishersController : Controller
    {
        private readonly HttpClient _httpClient;
        private string PublisherApiUrl = "https://localhost:7103/api/Publisher/";

        public PublishersController()
        {
            _httpClient = new HttpClient();
        }



        public async Task<ActionResult> Index()
        {

            HttpResponseMessage message = await _httpClient.GetAsync(PublisherApiUrl + "GetAll");
            if (message.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string data = await message.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
                var publishers = JsonConvert.DeserializeObject<List<Publisher>>(json["data"].ToString());
                return View(publishers);
            }
            return RedirectToAction("Index", "Publishers");
        }

        public async Task<IActionResult> Details(int? id)
        {
            var role = HttpContext.Session.GetString("Role");
            ViewData["Role"] = role;

            HttpResponseMessage message = await _httpClient.GetAsync(PublisherApiUrl + $"GetPublisherById?id={id}");

            if (message.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var resData = await message.Content.ReadAsStringAsync();

                var product = ConvertPublisher(resData);
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
        public async Task<IActionResult> Create(Publisher publisher)
        {
            try
            {
                CheckAdmin();

                var role = HttpContext.Session.GetString("Role");
                ViewData["Role"] = role;

                CheckValidate(publisher);

                var token = HttpContext.Session.GetString("Token");
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage message = await _httpClient.PostAsJsonAsync(PublisherApiUrl + "AddPublisher", publisher);
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
                return View(publisher);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            CheckAdmin();

            var role = HttpContext.Session.GetString("Role");
            ViewData["Role"] = role;

            HttpResponseMessage message = await _httpClient.GetAsync(PublisherApiUrl + $"GetPublisherById?id={id}");

            if (message.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var resData = await message.Content.ReadAsStringAsync();

                var product = ConvertPublisher(resData);
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
        public async Task<IActionResult> Edit(int id, Publisher publisher)
        {
            CheckAdmin();

            var role = HttpContext.Session.GetString("Role");
            ViewData["Role"] = role;
            try
            {

                CheckValidate(publisher);

                var token = HttpContext.Session.GetString("Token");

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage message = await _httpClient.PutAsJsonAsync(PublisherApiUrl + $"UpdatePublisher?id={publisher.pub_id}", publisher);


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
                return View(publisher);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            CheckAdmin();

            var role = HttpContext.Session.GetString("Role");
            ViewData["Role"] = role;

            HttpResponseMessage message = await _httpClient.GetAsync(PublisherApiUrl + $"GetPublisherById?id={id}");

            if (message.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var resData = await message.Content.ReadAsStringAsync();

                var product = ConvertPublisher(resData);

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

            HttpResponseMessage message = await _httpClient.DeleteAsync(PublisherApiUrl + $"DeletePublisher?id={id}");

            if (message.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction(nameof(Index));
            }

            if (message.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                TempData["LoginFail"] = "You are not login";
                return RedirectToAction("Login", "Users");
            }

            if (message.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                HttpResponseMessage getProduct = await _httpClient.GetAsync(PublisherApiUrl + $"GetPublisherById?id={id}");

                if (message.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var resDataPro = await getProduct.Content.ReadAsStringAsync();

                    var product = ConvertPublisher(resDataPro);

                    string resData = await message.Content.ReadAsStringAsync();

                    var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(resData);

                    ViewData["ErrMsg"] = json["message"].ToString();

                    return View(product);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        private void CheckValidate(Publisher publisher)
        {
            if (string.IsNullOrWhiteSpace(publisher.publisher_name))
            {
                throw new Exception("PublisherName can not be empty!!!");
            }
            if (int.TryParse(publisher.publisher_name, out int result) && result < 0)
            {
                throw new Exception("Publisher must not be empty and cannot contain numbers!");
            }
            if (string.IsNullOrWhiteSpace(publisher.city))
            {
                throw new Exception("City can not be empty!!!");
            }
            if (int.TryParse(publisher.city, out int result1) && result1 < 0)
            {
                throw new Exception("City must not be empty and cannot contain numbers!");
            }
            if (string.IsNullOrWhiteSpace(publisher.state))
            {
                throw new Exception("State can not be empty!!!");
            }
            if (int.TryParse(publisher.state, out int result2) && result2 < 0)
            {
                throw new Exception("State must not be empty and cannot contain numbers!");
            }
            if (string.IsNullOrWhiteSpace(publisher.country))
            {
                throw new Exception("Country cmust not be empty and cannot contain numbers!");
            }
            if (int.TryParse(publisher.country, out int result3) && result3 < 0)
            {
                throw new Exception("Country must not be empty and cannot contain numbers!");
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
        private Publisher ConvertPublisher(string resData)
        {
            var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(resData);

            var publisher = JsonConvert.DeserializeObject<Publisher>(json["data"].ToString());

            return publisher;
        }
        [HttpGet]
        public IActionResult Error()
        {
            var role = HttpContext.Session.GetString("Role");
            ViewData["Role"] = role;

            return View();
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
            return RedirectToAction("AdminIndex");
        }
    }
}
