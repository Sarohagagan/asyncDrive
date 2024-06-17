using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using asyncDrive.API.CustomActionFilters;
using asyncDrive.API.Repositories;
using asyncDrive.API.Repositories.IRepository;
using Models.Domain;
using Utility;
using Models.DTO;


namespace asyncDrive.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        //private readonly asyncDriveDbContext dbContext;
        private readonly IUnitOfWork unitOfWork;

        public UsersController(IUnitOfWork unitOfWork)
        {
            //this.dbContext = dbContext;
            this.unitOfWork = unitOfWork;
        }
        //GET ALL Users
        [HttpGet]
        [Authorize(Roles = $"{SD.Role_SiteAdmin}")]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy, [FromQuery] bool isAscending = true, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {

            // Get Data From Database - Domain models
            var usersDomain = await unitOfWork.User.GetAllAsync(filterOn, filterQuery, sortBy, isAscending, pageNumber, pageSize);

            // Map Domain Models to DTOs
            var userDto = new List<UserDto>();
            foreach (var userDomain in usersDomain)
            {
                userDto.Add(new UserDto()
                {
                    Id = userDomain.Id,
                    FirstName = userDomain.FirstName,
                    LastName = userDomain.LastName,
                    Email = userDomain.Email,
                    PhoneNumber = userDomain.PhoneNumber,
                    PostalCode = userDomain.PostalCode,
                    Address = userDomain.Address,
                    City = userDomain.City,
                    State = userDomain.State,
                    Country = userDomain.Country
                });
            }
            // Return DTOs
            return Ok(userDto);
        }
        //GET SINGLE User (Get User By ID)
        [HttpGet]
        [Route("{id:guid}")]
        //[Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            //var user = dbContext.Users.Find(id); it takes only primary key
            var userDomain = await unitOfWork.User.GetAsync(u => u.Id == id);
            if (userDomain == null)
            {
                return NotFound();
            }
            var userDto = new UserDto
            {
                Id = userDomain.Id,
                FirstName = userDomain.FirstName,
                LastName = userDomain.LastName,
                Email = userDomain.Email,
                PhoneNumber = userDomain.PhoneNumber,
                PostalCode = userDomain.PostalCode,
                Address = userDomain.Address,
                City = userDomain.City,
                State = userDomain.State,
                Country = userDomain.Country
            };
            return Ok(userDto);
        }
        //POST to Create New User
        [HttpPost]
        [Authorize(Roles = $"{SD.Role_SiteAdmin+","+SD.Role_SuperAdmin}")]
        public async Task<IActionResult> Create([FromBody] AddUserRequestDto addUserRequestDto)
        {
            if (ModelState.IsValid)
            {
                var userDomainModel = new User
                {
                    FirstName = addUserRequestDto.FirstName,
                    LastName = addUserRequestDto.LastName,
                    Email = addUserRequestDto.Email,
                    Password = addUserRequestDto.Password,
                    PhoneNumber = addUserRequestDto.PhoneNumber,
                    PostalCode = addUserRequestDto.PostalCode,
                    Address = addUserRequestDto.Address,
                    City = addUserRequestDto.City,
                    State = addUserRequestDto.State,
                    Country = addUserRequestDto.Country,
                    CreatedOn = DateTime.UtcNow,
                    UpdatedOn = DateTime.UtcNow,
                    LoginUserId = addUserRequestDto.LoginUserId,
                };

                userDomainModel = await unitOfWork.User.AddAsync(userDomainModel);
                await unitOfWork.SaveAsync();
                var userDto = new UserDto
                {
                    Id = userDomainModel.Id,
                    FirstName = userDomainModel.FirstName,
                    LastName = userDomainModel.LastName,
                    Email = userDomainModel.Email,
                    PhoneNumber = userDomainModel.PhoneNumber,
                    PostalCode = userDomainModel.PostalCode,
                    Address = userDomainModel.Address,
                    City = userDomainModel.City,
                    State = userDomainModel.State,
                    Country = userDomainModel.Country
                };
                return CreatedAtAction(nameof(GetById), new { id = userDto.Id }, userDto);
            }
            else
                return BadRequest(ModelState);
        }
        //PUT to Update User
        [HttpPut]
        [Route("{id:guid}")]
        //[Authorize(Roles = "Writer")]
        [ValidateModel] //second way to implement validation using custom model attribute filter
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateUserRequestDto updateUserRequestDto)
        {
            var userDomainModel = await unitOfWork.User.GetAsync(u => u.Id == id);
            if (userDomainModel == null)
            {
                return NotFound();
            }
            userDomainModel.FirstName = updateUserRequestDto.FirstName;
            userDomainModel.LastName = updateUserRequestDto.LastName;
            userDomainModel.Email = updateUserRequestDto.Email;
            userDomainModel.PhoneNumber = updateUserRequestDto.PhoneNumber;
            userDomainModel.PostalCode = updateUserRequestDto.PostalCode;
            userDomainModel.Address = updateUserRequestDto.Address;
            userDomainModel.City = updateUserRequestDto.City;
            userDomainModel.State = updateUserRequestDto.State;
            userDomainModel.Country = updateUserRequestDto.Country;
            userDomainModel.Password = updateUserRequestDto.Password;
            await unitOfWork.User.UpdateAsync(userDomainModel);
            await unitOfWork.SaveAsync();

            var userDto = new UserDto
            {
                Id = id,
                FirstName = userDomainModel.FirstName,
                LastName = userDomainModel.LastName,
                Email = userDomainModel.Email,
                PhoneNumber = userDomainModel.PhoneNumber,
                PostalCode = userDomainModel.PostalCode,
                Address = userDomainModel.Address,
                City = userDomainModel.City,
                State = userDomainModel.State,
                Country = userDomainModel.Country
            };
            return Ok(userDto);
        }
        //Delete User
        [HttpDelete]
        [Route("{id:guid}")]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var userDomainModel = await unitOfWork.User.GetAsync(u => u.Id == id);
            if (userDomainModel == null) { return NotFound(); }
            userDomainModel = await unitOfWork.User.RemoveAsync(userDomainModel);
            await unitOfWork.SaveAsync();
            if (userDomainModel == null)
                return NotFound();

            //return deleted user
            var userDto = new UserDto
            {
                Id = id,
                FirstName = userDomainModel.FirstName,
                LastName = userDomainModel.LastName,
                Email = userDomainModel.Email,
                PhoneNumber = userDomainModel.PhoneNumber,
                PostalCode = userDomainModel.PostalCode,
                Address = userDomainModel.Address,
                City = userDomainModel.City,
                State = userDomainModel.State,
                Country = userDomainModel.Country
            };
            return Ok(userDto);
        }
    }
}
