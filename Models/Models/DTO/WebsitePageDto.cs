using asyncDrive.Models.Domain;

namespace asyncDrive.Models.DTO
{
    public class WebsitePageDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string ShortDesc { get; set; }
        public string Description { get; set; }
        public string? ImageURL { get; set; }

        public string? BannerURL { get; set; }

        public string JsonData { get; set; }

    }
}
