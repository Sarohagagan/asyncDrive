namespace Models.DTO
{
    public class AddWebsiteRequestDto
    {
        public string Title { get; set; }
        public string DNS { get; set; }
        public string? Description { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        public Guid UserId { get; set; }
    }
}
