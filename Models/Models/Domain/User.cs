using System.ComponentModel.DataAnnotations;

namespace asyncDrive.Models.Domain
{
    public class User
    {
        [Key] //if id is diff name like userid etc then need to define key attribute
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PostalCode { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        public string? LoginUserId { get; set; }
        //Navigation properties
        //public ICollection<Website> Websites { get; set; }
    }
}
