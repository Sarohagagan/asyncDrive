namespace asyncDrive.Models.Domain
{
    public class Websitedata
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Logo { get; set; }
        public string PrimaryPhoneNo { get; set; }
        public string? SecondaryPhoneNo { get; set; }
        public string? ToolFreePhoneNo { get; set; }
        public string? ShortDesc { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        public Guid WebsiteId { get; set; }
        //Navigation properties
        public Website Website { get; set; }

        //public ICollection<Websitepage> Websitepages { get; set; }
    }
}
