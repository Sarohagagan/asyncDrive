using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Domain
{
    public class Website
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string DNS { get; set; }
        public string? Description { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        public Guid UserId { get; set; }
        //Navigation properties
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
