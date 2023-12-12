using Assigment02_BussinessObject;
using Assigment02_Repositories.IRepositories;
using Assigment02_Service;
using Assigment02_WebAPI.ViewModel;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using System.Security.Claims;

namespace Assigment02_WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BookController : ODataController
    {
        private readonly IBookService bookService;
        private readonly IMapper mapper;

        public BookController(IBookService bookService, IMapper mapper)
        {
            this.bookService = bookService;
            this.mapper = mapper;
        }

        [HttpGet]
        [EnableQuery]

        public IEnumerable<Book> Get()
        {
            var list = bookService.GetAll();
            return list;
        }



        [HttpGet]
        public async Task<IActionResult> GetAllBook(string? search, int? pageIndex, int itemPerPage)
        {
            try
            {
                var products = await bookService.GetBooks(search);
                int totalPage = (int)Math.Ceiling((decimal)(products.Count() / itemPerPage));
                if (pageIndex == null || pageIndex == 0 || pageIndex > totalPage)
                {
                    pageIndex = 1;
                }
                return StatusCode(200, new
                {
                    Status = "Success",
                    TotalPage = totalPage,
                    PageIndex = pageIndex,
                    ItemPerPage = itemPerPage,
                    TotalValues = products.Count(),
                    Data = products.Skip((pageIndex.Value - 1) * itemPerPage).Take(itemPerPage).ToList()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Status = "Error",
                    Message = ex.Message
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetBookById(int id)
        {
            try
            {
                var age = await bookService.GetBookById(id);
                return Ok(new
                {
                    Status = "Success",
                    Data = age
                });
            }
            catch (Exception ex)
            {
                return StatusCode(400, new
                {
                    Status = "Error",
                    ErrorMessage = ex.Message
                });
            }
        }



        [HttpPost]
        public async Task<IActionResult> AddBook(BookVM model)
        {
            try
            {
                var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                if (role == "Admin")
                {
                    var book = mapper.Map<Book>(model);
                    var check = await bookService.Add(book);
                    return check ? Ok(new
                    {
                        Status = 1,
                        Message = "Add Success!!!"
                    }) : Ok(new
                    {
                        Status = 0,
                        Message = "Add Fail!!!"
                    });
                }
                else
                {
                    return StatusCode(404, new
                    {
                        Status = "Error",
                        Message = "Not Found",
                    });
                }
                }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Status = "Error",
                    ErrorMessage = ex.Message
                });
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBook(BookUpdateVM model)
        {
            try
            {
                var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                if (role == "Admin")
                {
                var book = mapper.Map<Book>(model);
                var check = await bookService.Update(book);
                return check ? Ok(new
                {
                    Status = 1,
                    Message = "Update Success!!!"
                }) : Ok(new
                {
                    Status = 0,
                    Message = "Update Fail!!!"
                });
                }
                else
                    {
                        return StatusCode(404, new
                        {
                            Status = "Error",
                            Message = "Not Found",
                        });
                    }
                }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Status = "Error",
                    ErrorMessage = ex.Message
                });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBook(int id)
        {
            try
            {
                var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                if (role == "Admin")
                {
                var check = await bookService.Delete(id);
                return check ? Ok(new
                {
                    Message = "Delete Success!!!"
                }) : Ok(new
                {
                    Message = "Delete Fail!!!"
                });
                }
                else
                {
                    return StatusCode(404, new
                    {
                        Status = "Error",
                        Message = "Not Found",
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Status = "Error",
                    ErrorMessage = ex.Message
                });
            }
        }
    }

}
