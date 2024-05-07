namespace Client.Models
{
    public class Contact
    {
        [Key]
        public int ContactId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [MaxLength(100)]
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Region { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
        public int? Age { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? Description { get; set; }
        public int? CounterpartyId { get; set; }
        public int CreatorUserId { get; set; }

        public bool Lead { get; set; }
        public LeadStatus? LeadStatus { get; set; }
        public DateTime? LastContactDate { get; set; }
        public string? LastContactedBy { get; set; }
        public LeadSource? LeadSource { get; set; }

        public LeadPriority? Priority { get; set; }

    }
    public enum LeadStatus
    {
        New,
        Contacted,
        Qualified,
        Lost,
        Converted
    }
    public enum LeadPriority
    {
        Low,
        Medium,
        High
    }

    public enum LeadSource
    {
        Personal,
        International,
        Client
    }

}
