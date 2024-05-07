namespace Client.Models
{
    public class ProductModel
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(100)]
        public string Type { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public double TotalAmount { get; set; }
    }
}
