using crmAgroCompany;

namespace api.dbContext.Models
{
    public class Product
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
