using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class UserProfileModel
    {
        public int CustomerTypeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
    }
    public class RegistrationModel : UserProfileModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
