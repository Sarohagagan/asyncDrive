using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace asyncDrive.Models.DTO
{
    public class AddUserRequestDto
    {
        [Required]
        [MinLength(3,ErrorMessage = "Name has to be a minimum 3 character")]
        [MaxLength(10, ErrorMessage = "Name has to be a maximum 10 character")]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public string? PostalCode { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        public string LoginUserId { get; set; }
    }
}
