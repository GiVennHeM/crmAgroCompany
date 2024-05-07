namespace Client.Models
{
    public class Сounterparty
    {
        [Key]
        public int СounterpartyId { get; set; }
        [Required]
        [MaxLength(100)]
        public string СounterpartyCompany { get; set; }
        [Required]
        [MaxLength(50)]
        public string State { get; set; }
        [Required]
        [MaxLength(50)]
        public string Region { get; set; }
        public bool Lead { get; set; }
        public DateTime CreationDate { get; set; }
        [Required]
        [MaxLength(8)]
        public string USREOU { get; set; }
        [Required]
        public string Type { get; set; }
        public string Status { get; set; }
        public ICollection<Contact> Contact { get; set; }
    }
}
