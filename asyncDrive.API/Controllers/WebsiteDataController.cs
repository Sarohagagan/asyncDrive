using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Domain;
using asyncDrive.API.Repositories.IRepository;
using Models.DTO;

namespace asyncDrive.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebsiteDataController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        public WebsiteDataController(IUnitOfWork unitOfWork) {
            this.unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddWebsiteDataRequestDto addWebsiteDataRequestDto)
        {
            var websiteDataDomain = new Websitedata
            {
                Title = addWebsiteDataRequestDto.Title,
                Logo = addWebsiteDataRequestDto.Logo,
                PrimaryPhoneNo = addWebsiteDataRequestDto.PrimaryPhoneNo,
                SecondaryPhoneNo = addWebsiteDataRequestDto.SecondaryPhoneNo,
                ToolFreePhoneNo = addWebsiteDataRequestDto.ToolFreePhoneNo,
                ShortDesc = addWebsiteDataRequestDto.ShortDesc,
                Description = addWebsiteDataRequestDto.Description,
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now,
                WebsiteId = addWebsiteDataRequestDto.WebsiteId,
            };
            websiteDataDomain = await unitOfWork.WebsiteData.AddAsync(websiteDataDomain);
            await unitOfWork.SaveAsync();
            var websiteDataDto = new WebsiteDataDto
            {
                Id = websiteDataDomain.Id,
                Title = websiteDataDomain.Title,
                Logo = websiteDataDomain.Logo,
                PrimaryPhoneNo = websiteDataDomain.PrimaryPhoneNo,
                SecondaryPhoneNo = websiteDataDomain.SecondaryPhoneNo,
                ToolFreePhoneNo = websiteDataDomain.ToolFreePhoneNo,
                ShortDesc = websiteDataDomain.ShortDesc,
                Description = websiteDataDomain.Description,
                CreatedOn = websiteDataDomain.CreatedOn,
                UpdatedOn = websiteDataDomain.UpdatedOn,
            };
            return Ok(websiteDataDto);

        }

        //GET ALL Websites
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy, [FromQuery] bool isAscending = true, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            // Get Data From Database - Domain models
            var websitesDataDomain = await unitOfWork.WebsiteData.GetAllAsync(filterOn, filterQuery, sortBy, isAscending, pageNumber, pageSize, includeProperties: "Website");

            // Map Domain Models to DTOs
            var websiteDataDto = new List<WebsiteDataDto>();
            foreach (var websiteDataDomain in websitesDataDomain)
            {
                websiteDataDto.Add(new WebsiteDataDto()
                {
                    Id = websiteDataDomain.Id,
                    Title = websiteDataDomain.Title,
                    Logo = websiteDataDomain.Logo,
                    PrimaryPhoneNo = websiteDataDomain.PrimaryPhoneNo,
                    SecondaryPhoneNo = websiteDataDomain.SecondaryPhoneNo,
                    ToolFreePhoneNo = websiteDataDomain.ToolFreePhoneNo,
                    ShortDesc = websiteDataDomain.ShortDesc,
                    Description = websiteDataDomain.Description,
                    CreatedOn = websiteDataDomain.CreatedOn,
                    UpdatedOn = websiteDataDomain.UpdatedOn,
                    Website = websiteDataDomain.Website,
                });
            }
            // Return DTOs
            return Ok(websiteDataDto);
        }
        //GET SINGLE Websitedata (Get Websitedata By ID)
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            //var user = dbContext.Users.Find(id); it takes only primary key
            var websiteDataDomain = await unitOfWork.WebsiteData.GetAsync(u => u.Id == id, includeProperties: "Website");
            if (websiteDataDomain == null)
            {
                return NotFound();
            }
            var websiteDataDto = new WebsiteDataDto
            {
                Id = websiteDataDomain.Id,
                Title = websiteDataDomain.Title,
                Logo = websiteDataDomain.Logo,
                PrimaryPhoneNo = websiteDataDomain.PrimaryPhoneNo,
                SecondaryPhoneNo = websiteDataDomain.SecondaryPhoneNo,
                ToolFreePhoneNo = websiteDataDomain.ToolFreePhoneNo,
                ShortDesc = websiteDataDomain.ShortDesc,
                Description = websiteDataDomain.Description,
                CreatedOn = websiteDataDomain.CreatedOn,
                UpdatedOn = websiteDataDomain.UpdatedOn,
                Website = websiteDataDomain.Website,
            };
            return Ok(websiteDataDto);
        }
        //PUT to Update Websitedata
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWebsiteDataRequestDto updateWebsiteDataRequestDto)
        {
            var websiteDataDomainModel = await unitOfWork.WebsiteData.GetAsync(u => u.Id == id);
            if (websiteDataDomainModel == null)
            {
                return NotFound();
            }
            websiteDataDomainModel.Title = updateWebsiteDataRequestDto.Title;
            websiteDataDomainModel.Logo = updateWebsiteDataRequestDto.Logo;
            websiteDataDomainModel.PrimaryPhoneNo = updateWebsiteDataRequestDto.PrimaryPhoneNo;
            websiteDataDomainModel.SecondaryPhoneNo = updateWebsiteDataRequestDto.SecondaryPhoneNo;
            websiteDataDomainModel.ToolFreePhoneNo = updateWebsiteDataRequestDto.ToolFreePhoneNo;
            websiteDataDomainModel.ShortDesc = updateWebsiteDataRequestDto.ShortDesc;
            websiteDataDomainModel.Description = updateWebsiteDataRequestDto.Description;
            websiteDataDomainModel.UpdatedOn = DateTime.Now;
            if (websiteDataDomainModel == null)
            {
                return NotFound();
            }
            await unitOfWork.WebsiteData.UpdateAsync(websiteDataDomainModel);
            await unitOfWork.SaveAsync();

            var websiteDataDto = new WebsiteDataDto
            {
                Id = id,
                Title = updateWebsiteDataRequestDto.Title,
                Logo = updateWebsiteDataRequestDto.Logo,
                PrimaryPhoneNo = updateWebsiteDataRequestDto.PrimaryPhoneNo,
                SecondaryPhoneNo = updateWebsiteDataRequestDto.SecondaryPhoneNo,
                ToolFreePhoneNo = updateWebsiteDataRequestDto.ToolFreePhoneNo,
                ShortDesc = updateWebsiteDataRequestDto.ShortDesc,
                Description = updateWebsiteDataRequestDto.Description,
                UpdatedOn = DateTime.Now,
                Website = websiteDataDomainModel.Website,
            };
            return Ok(websiteDataDto);
        }
        //Delete Websitedata
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var websiteDataDomainModel = await unitOfWork.WebsiteData.GetAsync(u => u.Id == id);
            if (websiteDataDomainModel == null) { return NotFound(); }
            websiteDataDomainModel = await unitOfWork.WebsiteData.RemoveAsync(websiteDataDomainModel);
            await unitOfWork.SaveAsync();
            if (websiteDataDomainModel == null)
                return NotFound();

            //return deleted user
            var websiteDataDto = new WebsiteDataDto
            {
                Id = id,
                Title = websiteDataDomainModel.Title,
                Logo = websiteDataDomainModel.Logo,
                PrimaryPhoneNo = websiteDataDomainModel.PrimaryPhoneNo,
                SecondaryPhoneNo = websiteDataDomainModel.SecondaryPhoneNo,
                ToolFreePhoneNo = websiteDataDomainModel.ToolFreePhoneNo,
                ShortDesc = websiteDataDomainModel.ShortDesc,
                Description = websiteDataDomainModel.Description,
                UpdatedOn = websiteDataDomainModel.UpdatedOn,
                Website = websiteDataDomainModel.Website,
            };
            return Ok(websiteDataDto);
        }
    }
}
