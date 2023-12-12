    using Assigment02_BussinessObject;
using Assigment02_WebClient.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Assigment02_WebClient.Controllers
{
    public class BooksController : Controller
    {
        private readonly HttpClient _httpClient;
        private string BookApiUrl = "https://localhost:7103/api/Book/";
        private string PublisherApiUrl = "https://localhost:7103/api/Publisher/";
        public BooksController()
        {
            _httpClient = new HttpClient();
        }

        private async Task<List<Publisher>> GetPublishers()
        {
            HttpResponseMessage message = await _httpClient.GetAsync(PublisherApiUrl + "GetAll");

            if (message.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string resData = await message.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };

                var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(resData);

                var categories = JsonConvert.DeserializeObject<List<Publisher>>(json["data"].ToString());

                return categories;
            }
            return null;
        }

        private void CheckValidate(Book book)
        {
            if (string.IsNullOrWhiteSpace(book.bookname))
            {
                throw new Exception("Book Name can not be empty!!!");
            }
            if (string.IsNullOrWhiteSpace(book.title))
            {
                throw new Exception("Title can not be empty!!!");
            }
            if (book.type <= 0)
            {
                throw new Exception("Type can not be empty and can not enter negative number!");
            }
            if (!IsNumeric(book.type.ToString()))
            {
                throw new Exception("Type must be a valid number!");
            }
            if (book.price <= 0)
            {
                throw new Exception("Price is not valid and can not enter negative number!");
            }
            if (!IsNumeric(book.price.ToString()))
            {
                throw new Exception("Price must be a valid number!");
            }
            if (book.advance <= 0)
            {
                throw new Exception("Advance can not be empty and can not enter negative number!");
            }
            if (!IsNumeric(book.advance.ToString()))
            {
                throw new Exception("Advance must be a valid number!");
            }
            if (book.royalty <= 0)
            {
                throw new Exception("Royalty is not valid and can not enter negative number!");
            }
            if (!IsNumeric(book.royalty.ToString()))
            {
                throw new Exception("Royalty must be a valid number!");
            }
            if (book.ytd_sales <=0)
            {
                throw new Exception("YtdSales can not be empty and can not enter negative number!");
            }
            if (!IsNumeric(book.ytd_sales.ToString()))
            {
                throw new Exception("YtdSales must be a valid number!");
            }
            if (string.IsNullOrWhiteSpace(book.notes))
            {
                throw new Exception("Notes can not be empty!!!");
            }
        }

        private bool IsNumeric(string value)
        {
            double number;
            return double.TryParse(value, out number);
        }


        public async Task<IActionResult> Index(string search, int pageIndex, int itemPerPage)
        {
            var role = HttpContext.Session.GetString("Role");
            ViewData["Role"] = role;
            HttpResponseMessage message;
            if (itemPerPage == null || itemPerPage == 0)
            {
                itemPerPage = 5;
            }
            if (string.IsNullOrEmpty(search))
            {
                message = await _httpClient.GetAsync(BookApiUrl + $"GetAllBook?pageIndex={pageIndex}&itemPerPage={itemPerPage}");
            }
            else
            {
                message = await _httpClient.GetAsync(BookApiUrl + $"GetAllBook?search={search}&pageIndex={pageIndex}&itemPerPage={itemPerPage}");
            }

            string resData = await message.Content.ReadAsStringAsync();

            var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(resData);

            if (message.StatusCode == System.Net.HttpStatusCode.InternalServerError || message.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                throw new Exception("Error");
            }


            int totalPage = 0;


            totalPage = int.Parse(json["totalPage"].ToString());

            pageIndex = int.Parse(json["pageIndex"].ToString());

            int totalValues = int.Parse(json["totalValues"].ToString());

            itemPerPage = int.Parse(json["itemPerPage"].ToString());

            var pros = ConvertProducts(resData);
            return View(new BookIndexVM
            {
                PageIndex = pageIndex,
                Books = pros,
                ItemPerPage = itemPerPage,
                TotalValues = totalValues,
                Search = search,
                TotalPage = totalPage
            });
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            var role = HttpContext.Session.GetString("Role");
            ViewData["Role"] = role;

            HttpResponseMessage message = await _httpClient.GetAsync(BookApiUrl + $"GetBookById?id={id}");

            if (message.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var resData = await message.Content.ReadAsStringAsync();

                var product = ConvertBook(resData);
                //ViewData["PublisherId"] = new SelectList(await GetPublishers(), "pub_id", "publisher_name");
                return View(product);
            }
            if (message.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return RedirectToAction(nameof(Error));
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Create()
        {
            CheckAdmin();

            var role = HttpContext.Session.GetString("Role");
            ViewData["Role"] = role;

            ViewData["PublisherId"] = new SelectList(await GetPublishers(), "pub_id", "publisher_name");
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book)
        {
            try
            {
                CheckAdmin();

                var role = HttpContext.Session.GetString("Role");
                ViewData["Role"] = role;

                CheckValidate(book);

                var token = HttpContext.Session.GetString("Token");
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage message = await _httpClient.PostAsJsonAsync(BookApiUrl + "AddBook", book);

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
                ViewData["PublisherId"] = new SelectList(await GetPublishers(), "pub_id", "publisher_name", book.pub_id);
                return View(book);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            CheckAdmin();

            var role = HttpContext.Session.GetString("Role");
            ViewData["Role"] = role;

            HttpResponseMessage message = await _httpClient.GetAsync(BookApiUrl + $"GetBookById?id={id}");

            if (message.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var resData = await message.Content.ReadAsStringAsync();

                var product = ConvertBook(resData);

                ViewData["PublisherId"] = new SelectList(await GetPublishers(), "pub_id", "publisher_name", product.pub_id);
                return View(product);
            }
            if (message.StatusCode == System.Net.HttpStatusCode.NotFound || message.StatusCode == System.Net.HttpStatusCode.InternalServerError || message.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                return RedirectToAction(nameof(Error));
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Book book)
        {
            CheckAdmin();

            var role = HttpContext.Session.GetString("Role");
            ViewData["Role"] = role;
            try
            {

                CheckValidate(book);

                var token = HttpContext.Session.GetString("Token");

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage message = await _httpClient.PutAsJsonAsync(BookApiUrl + $"UpdateBook?id={book.book_id}", book);


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
                ViewData["PublisherId"] = new SelectList(await GetPublishers(), "pub_id", "publisher_name", book.pub_id);
                ViewData["ErrMsg"] = ex.Message;
                return View(book);
            }
        }

        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            CheckAdmin();

            var role = HttpContext.Session.GetString("Role");
            ViewData["Role"] = role;

            HttpResponseMessage message = await _httpClient.GetAsync(BookApiUrl + $"GetBookById?id={id}");

            if (message.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var resData = await message.Content.ReadAsStringAsync();

                var product = ConvertBook(resData);

                return View(product);
            }
            if (message.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return RedirectToAction(nameof(Error));
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            CheckAdmin();

            var token = HttpContext.Session.GetString("Token");

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage message = await _httpClient.DeleteAsync(BookApiUrl + $"DeleteBook?id={id}");

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
                HttpResponseMessage getProduct = await _httpClient.GetAsync(BookApiUrl + $"GetBookById?id={id}");

                if (message.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var resDataPro = await getProduct.Content.ReadAsStringAsync();

                    var product = ConvertBook(resDataPro);

                    string resData = await message.Content.ReadAsStringAsync();

                    var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(resData);

                    ViewData["ErrMsg"] = json["message"].ToString();

                    return View(product);
                }
            }

            return RedirectToAction(nameof(Index));
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

        private Book ConvertBook(string resData)
        {
            var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(resData);

            var product = JsonConvert.DeserializeObject<Book>(json["data"].ToString());

            return product;
        }

        private List<Book> ConvertProducts(string resData)
        {
            var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(resData);

            var products = JsonConvert.DeserializeObject<List<Book>>(json["data"].ToString());

            return products;
        }
    }
}
