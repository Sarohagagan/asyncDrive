namespace asyncDrive.Models.Domain
{
    public class Websitepage
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
