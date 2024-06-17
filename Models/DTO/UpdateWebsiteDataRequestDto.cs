namespace Models.DTO
{
    public class UpdateWebsiteDataRequestDto
    {
        public string Title { get; set; }
        public string Logo { get; set; }
        public string PrimaryPhoneNo { get; set; }
        public string? SecondaryPhoneNo { get; set; }
        public string? ToolFreePhoneNo { get; set; }
        public string? ShortDesc { get; set; }
        public string? Description { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
