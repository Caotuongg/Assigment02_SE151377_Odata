using Assigment02_BussinessObject;
using Assigment02_Service;
using Assigment02_WebAPI.ViewModel;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Security.Claims;

namespace Assigment02_WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthorController : ODataController
    {
        private readonly IAuthorService authorService;
        private readonly IMapper mapper;

        public AuthorController(IAuthorService authorService, IMapper mapper)
        {
            this.authorService = authorService;
            this.mapper = mapper;
        }
        
        
        [HttpGet]
        [EnableQuery]

        public IEnumerable<Author> Get()
        {
            var list = authorService.GetAll();
            return list;
        }

        //[HttpGet("{key}")]
        //[EnableQuery]

        //public IActionResult Get([FromODataUri] int key)
        //{
        //    var Author = authorService.GetAuthor(key);
        //    if (Author == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(Author);
        //}
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var list =  authorService.GetAll();
                //var author = new List<Author>();
                //foreach (var item in list)
                //{
                //    author.Add(mapper.Map<Author>(item));
                //}
                return Ok(new
                {
                    Status = "Success",
                    Data = list
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Status = "Error",
                    Message = ex.Message,
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAuthorById(int id)
        {
            try
            {
                var age = await authorService.GetAuthor(id);
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
        public async Task<IActionResult> AddAuthor(AuthorVM model)
        {
            try
            {
                var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                if (role == "Admin")
                {
                    var author = mapper.Map<Author>(model);
                    var check = await authorService.Add(author);
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
        public async Task<IActionResult> UpdateAuthor(AuthorUpdateVM model)
        {
            try
            {
                var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                if (role == "Admin")
                {
                    var author = mapper.Map<Author>(model);
                var check = await authorService.Update(author);
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
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            try
            {
                var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                if (role == "Admin")
                {
                    var check = await authorService.Delete(id);
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

        private Author Validate(AuthorVM authorVM)
        {
            if (string.IsNullOrEmpty(authorVM.last_name.Trim()))
            {
                throw new Exception("last_name cannot be empty!!!");
            }
            if (string.IsNullOrEmpty(authorVM.first_name.Trim()))
            {
                throw new Exception("first_name cannot be empty!!!");
            }

            return mapper.Map<Author>(authorVM);
        }
    }
}
    
    
               
            
            
        
        


