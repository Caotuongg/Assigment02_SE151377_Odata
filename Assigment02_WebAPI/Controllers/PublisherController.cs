using Assigment02_BussinessObject;
using Assigment02_Service;
using Assigment02_WebAPI.ViewModel;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using System.Security.Claims;

namespace Assigment02_WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PublisherController : ControllerBase
    {
        private readonly IPublisherService publisherService;
        private readonly IMapper mapper;

        public PublisherController(IPublisherService publisherService, IMapper mapper)
        {
            this.publisherService = publisherService;
            this.mapper = mapper;
        }

        [HttpGet]
        [EnableQuery]

        public IEnumerable<Publisher> Get()
        {
            var list = publisherService.GetAll();
            return list;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var list = publisherService.GetAll();
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
        public async Task<IActionResult> GetPublisherById(int id)
        {
            try
            {
                var age = await publisherService.GetPublisher(id);
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
        public async Task<IActionResult> AddPublisher(PublisherVM model)
        {
            try
            {
                var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                if (role == "Admin")
                {
                    var publisher = mapper.Map<Publisher>(model);
                    var check = await publisherService.Add(publisher);
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
        public async Task<IActionResult> UpdatePublisher(PublisherUpdateVM model)
        {
            try
            {
                var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                if (role == "Admin")
                {
                    var publisher = mapper.Map<Publisher>(model);
                var check = await publisherService.Update(publisher);
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
                return StatusCode(400, new
                {
                    Status = "Error",
                    ErrorMessage = ex.Message
                });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePublisher(int id)
        {
            try
            {
                var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                if (role == "Admin")
                {
                    var check = await publisherService.Delete(id);
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
                return StatusCode(400, new
                {
                    Status = "Error",
                    ErrorMessage = ex.Message
                });
            }
        }
    }
}
