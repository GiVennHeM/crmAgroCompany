namespace crmAgroCompany
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [MaxLength(100)]
        public string Company { get; set; }
        [Required]
        [MaxLength(50)]
        public string Region { get; set; }
        [Required]
        public DateTime Birthday { get; set; }
        [Required]
        [MaxLength(20)]
        public string NumberOfPhone { get; set; }
        [Required]
        [MaxLength(50)]
        public string Email { get; set; }
        public ICollection<Deal> DealsId { get; set; }
        public int Lead { get; set; }
    }
}
