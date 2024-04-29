namespace api.dbContext.Models
{
    public class Deal
    {
        [Key]
        public int DealId { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(100)]
        public string Status { get; set; }
        [Required]
        [MaxLength(100)]
        public string Region { get; set; }
        [Required]
        public double Cash { get; set; }
        [Required]
        public int Lead { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public ICollection<Product> ProductsId { get; set; }
    }
}
