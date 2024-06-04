namespace asyncDrive.Models.DTO
{
    public class UpdateWebsiteRequestDto
    {
        public string Title { get; set; }
        public string DNS { get; set; }
        public string? Description { get; set; }
        public bool Status { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
