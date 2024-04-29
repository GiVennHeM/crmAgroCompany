﻿namespace crmAgroCompany
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
        [MaxLength(100)]
        public double Cash { get; set; }
        [Required]
        [MaxLength(100)]
        public DateTime Date { get; set; }
        [Required]
        [MaxLength(1)]
        public int Lead { get; set; }
    }
}
