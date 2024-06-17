using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Domain;
using asyncDrive.API.Repositories.IRepository;
using Models.DTO;

namespace asyncDrive.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebsitePageController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        public WebsitePageController(IUnitOfWork unitOfWork) {
            this.unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddWebsitePageRequestDto addWebsitePageRequestDto)
        {
            var websitePageDomain = new Websitepage
            {
                Title = addWebsitePageRequestDto.Title,
                ShortDesc = addWebsitePageRequestDto.ShortDesc,
                Description = addWebsitePageRequestDto.Description,
                ImageURL = addWebsitePageRequestDto.ImageURL,
                BannerURL = addWebsitePageRequestDto.BannerURL,
                JsonData = addWebsitePageRequestDto.JsonData,
            };
            websitePageDomain = await unitOfWork.WebsitePage.AddAsync(websitePageDomain);
            await unitOfWork.SaveAsync();
            var websitePageDto = new WebsitePageDto
            {
                Id = websitePageDomain.Id,
                Title = websitePageDomain.Title,
                ShortDesc = websitePageDomain.ShortDesc,
                Description = websitePageDomain.Description,
                ImageURL = websitePageDomain.ImageURL,
                BannerURL = websitePageDomain.BannerURL,
                JsonData = websitePageDomain.JsonData,
            };
            return Ok(websitePageDto);

        }

        //GET ALL Websites
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy, [FromQuery] bool isAscending = true, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            // Get Data From Database - Domain models
            var websitesPageDomain = await unitOfWork.WebsitePage.GetAllAsync(filterOn, filterQuery, sortBy, isAscending, pageNumber, pageSize);

            // Map Domain Models to DTOs
            var websitePageDto = new List<WebsitePageDto>();
            foreach (var websitePageDomain in websitesPageDomain)
            {
                websitePageDto.Add(new WebsitePageDto()
                {
                    Id = websitePageDomain.Id,
                    Title = websitePageDomain.Title,
                    ShortDesc = websitePageDomain.ShortDesc,
                    Description = websitePageDomain.Description,
                    ImageURL = websitePageDomain.ImageURL,
                    BannerURL = websitePageDomain.BannerURL,
                    JsonData = websitePageDomain.JsonData,
                });
            }
            // Return DTOs
            return Ok(websitePageDto);
        }
        //GET SINGLE Websitepage (Get Websitepage By ID)
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            //var user = dbContext.Users.Find(id); it takes only primary key
            var websitePageDomain = await unitOfWork.WebsitePage.GetAsync(u => u.Id == id);
            if (websitePageDomain == null)
            {
                return NotFound();
            }
            var websitePageDto = new WebsitePageDto
            {
                Id = websitePageDomain.Id,
                Title = websitePageDomain.Title,
                ShortDesc = websitePageDomain.ShortDesc,
                Description = websitePageDomain.Description,
                ImageURL = websitePageDomain.ImageURL,
                BannerURL = websitePageDomain.BannerURL,
                JsonData = websitePageDomain.JsonData,
            };
            return Ok(websitePageDto);
        }
        //PUT to Update Websitepage
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWebsitePageRequestDto updateWebsitePageRequestDto)
        {
            var websitePageDomainModel = await unitOfWork.WebsitePage.GetAsync(u => u.Id == id);
            if (websitePageDomainModel == null)
            {
                return NotFound();
            }
            websitePageDomainModel.Title = updateWebsitePageRequestDto.Title;
            websitePageDomainModel.ShortDesc = updateWebsitePageRequestDto.ShortDesc;
            websitePageDomainModel.Description = updateWebsitePageRequestDto.Description;
            websitePageDomainModel.ImageURL = updateWebsitePageRequestDto.ImageURL;
            websitePageDomainModel.BannerURL = updateWebsitePageRequestDto.BannerURL;
            websitePageDomainModel.JsonData = updateWebsitePageRequestDto.JsonData;
            await unitOfWork.WebsitePage.UpdateAsync(websitePageDomainModel);
            await unitOfWork.SaveAsync();
            if (websitePageDomainModel == null)
            {
                return NotFound();
            }

            var websitePageDto = new WebsitePageDto
            {
                Id = id,
                Title = websitePageDomainModel.Title,
                ShortDesc = websitePageDomainModel.ShortDesc,
                Description = websitePageDomainModel.Description,
                ImageURL = websitePageDomainModel.ImageURL,
                BannerURL = websitePageDomainModel.BannerURL,
                JsonData = websitePageDomainModel.JsonData,
            };
            return Ok(websitePageDto);
        }
        //Delete Websitedata
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var websitePageDomainModel = await unitOfWork.WebsitePage.GetAsync(u => u.Id == id);
            if (websitePageDomainModel == null) { return NotFound(); }
            websitePageDomainModel = await unitOfWork.WebsitePage.RemoveAsync(websitePageDomainModel);
            await unitOfWork.SaveAsync();
            if (websitePageDomainModel == null)
                return NotFound();

            //return deleted user
            var websitePageDto = new WebsitePageDto
            {
                Id = id,
                Title = websitePageDomainModel.Title,
                ShortDesc = websitePageDomainModel.ShortDesc,
                Description = websitePageDomainModel.Description,
                ImageURL = websitePageDomainModel.ImageURL,
                BannerURL = websitePageDomainModel.BannerURL,
                JsonData = websitePageDomainModel.JsonData,
            };
            return Ok(websitePageDto);
        }
    }
}
