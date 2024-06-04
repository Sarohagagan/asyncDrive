using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using asyncDrive.Models.Domain;
using asyncDrive.Models.DTO;
using asyncDrive.API.Repositories.IRepository;

namespace asyncDrive.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebsiteController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        public WebsiteController(IUnitOfWork unitOfWork) {
            this.unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddWebsiteRequestDto addWebsiteRequestDto)
        {
            var websiteDomain = new Website
            {
                Title = addWebsiteRequestDto.Title,
                DNS = addWebsiteRequestDto.DNS,
                Description = addWebsiteRequestDto.Description,
                Status = addWebsiteRequestDto.Status,
                CreatedOn = addWebsiteRequestDto.CreatedOn,
                UpdatedOn = addWebsiteRequestDto.UpdatedOn,
                UserId = addWebsiteRequestDto.UserId
            };
            websiteDomain = await unitOfWork.Website.AddAsync(websiteDomain);
            await unitOfWork.SaveAsync();
            var websiteDto = new WebsiteDto
            {
                Id = websiteDomain.Id,
                Title = websiteDomain.Title,
                DNS = websiteDomain.DNS,
                Description = websiteDomain.Description,
                Status = websiteDomain.Status,
                CreatedOn = websiteDomain.CreatedOn,
                UpdatedOn = websiteDomain.UpdatedOn,
                User = websiteDomain.User
            };
            return Ok(websiteDto);

        }

        //GET ALL Websites
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy, [FromQuery] bool isAscending = true, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            // Get Data From Database - Domain models
            //pass multiple navigate props by  includeProperties: "User,Website"
            var websitesDomain = await unitOfWork.Website.GetAllAsync(filterOn, filterQuery, sortBy, isAscending, pageNumber, pageSize, includeProperties: "User");

            // Map Domain Models to DTOs
            var websiteDto = new List<WebsiteDto>();
            foreach (var websiteDomain in websitesDomain)
            {
                websiteDto.Add(new WebsiteDto()
                {
                    Id = websiteDomain.Id,
                    Title = websiteDomain.Title,
                    DNS = websiteDomain.DNS,
                    Description = websiteDomain.Description,
                    Status = websiteDomain.Status,
                    UpdatedOn = DateTime.Now,
                    User = websiteDomain.User
                });
            }
            // Return DTOs
            return Ok(websiteDto);
        }
        //GET SINGLE Website (Get Website By ID)
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            //var user = dbContext.Users.Find(id); it takes only primary key
            var websiteDomain = await unitOfWork.Website.GetAsync(u => u.Id == id, includeProperties: "User");
            if (websiteDomain == null)
            {
                return NotFound();
            }
            var userDto = new WebsiteDto
            {
                Id = websiteDomain.Id,
                Title = websiteDomain.Title,
                DNS = websiteDomain.DNS,
                Description = websiteDomain.Description,
                Status = websiteDomain.Status,
                UpdatedOn = DateTime.Now,
                User = websiteDomain.User
            };
            return Ok(userDto);
        }
        //PUT to Update Website
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWebsiteRequestDto updateWebsiteRequestDto)
        {
            var websiteDomainModel = await unitOfWork.Website.GetAsync(u => u.Id == id);
            if (websiteDomainModel == null)
            {
                return NotFound();
            }
            websiteDomainModel.Title = updateWebsiteRequestDto.Title;
            websiteDomainModel.DNS = updateWebsiteRequestDto.DNS;
            websiteDomainModel.Description = updateWebsiteRequestDto.Description;
            websiteDomainModel.Status = updateWebsiteRequestDto.Status;
            websiteDomainModel.UpdatedOn = DateTime.Now;

            await unitOfWork.Website.UpdateAsync(websiteDomainModel);
            await unitOfWork.SaveAsync();

            var websiteDto = new WebsiteDto
            {
                Id = id,
                Title = websiteDomainModel.Title,
                DNS = websiteDomainModel.DNS,
                Description = websiteDomainModel.Description,
                Status = websiteDomainModel.Status,
                UpdatedOn = websiteDomainModel.UpdatedOn,
                User = websiteDomainModel.User
            };
            return Ok(websiteDto);
        }
        //Delete Website
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var websiteDomainModel = await unitOfWork.Website.GetAsync(u => u.Id == id);
            if (websiteDomainModel == null) { return NotFound(); }
            websiteDomainModel = await unitOfWork.Website.RemoveAsync(websiteDomainModel);
            await unitOfWork.SaveAsync();
            if (websiteDomainModel == null)
                return NotFound();

            //return deleted user
            var websiteDto = new WebsiteDto
            {
                Id = id,
                Title = websiteDomainModel.Title,
                DNS = websiteDomainModel.DNS,
                Description = websiteDomainModel.Description,
                Status = websiteDomainModel.Status,
                UpdatedOn = websiteDomainModel.UpdatedOn,
                User = websiteDomainModel.User
            };
            return Ok(websiteDto);
        }
    }
}
