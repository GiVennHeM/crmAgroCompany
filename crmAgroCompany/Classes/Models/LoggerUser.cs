namespace crmAgroCompany
{
    public class LoggerUser
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }
        [Required]
        [MaxLength(50)]
        public string Password { get; set; }
        [Required]
        [MaxLength(100)]
        public string Login { get; set; }
        [Required]
        [MaxLength(20)]
        public string PhoneNumber { get; set; }
        [Required]
        [MaxLength(200)]
        public string ProfilePicture { get; set; }
        [Required]
        [MaxLength(100)]
        public string Surname { get; set; }
    }
}
