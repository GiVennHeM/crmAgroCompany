namespace API.dbContext.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(100)]
        public string Type { get; set; }
        [Required]
        [MaxLength(100)]
        public double Price { get; set; }
        [Required]
        [MaxLength(100)]
        public double TotalAmount { get; set; }
        [Required]
        [MaxLength(100)]
        public ICollection<Deal> DealId { get; set; }

    }
}
