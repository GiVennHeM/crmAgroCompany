namespace Client.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }
        [Required]
        [MaxLength(256)]
        public string Password { get; set; }
        [Required]
        [MaxLength(100)]
        public string Login { get; set; }
        [Required]
        [MaxLength(20)]
        public string PhoneNumber { get; set; }
        [Required]
        [MaxLength(200)]
        public string Avatar { get; set; }
        [Required]
        [MaxLength(100)]
        public string Surname { get; set; }
        [Required]
        public string Role { get; set; }
        [Required]
        public string Email { get; set; }
        public int QuantityOfСreated { get; set; }
    }
}
