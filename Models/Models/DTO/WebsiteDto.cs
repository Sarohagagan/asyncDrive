using asyncDrive.Models.Domain;

namespace asyncDrive.Models.DTO
{
    public class WebsiteDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string DNS { get; set; }
        public string? Description { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        public User User { get; set; }
    }
}
