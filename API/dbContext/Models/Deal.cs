namespace API.dbContext.Models
{
    public class Deal
    {
        [Key]
        public int Id { get; set; }
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
        [Required]
        [MaxLength(100)]
        public ICollection<Customer> CostumerId { get; set; }
        [Required]
        [MaxLength(100)]
        public ICollection<Product> ProductsId { get; set; }
    }
}
