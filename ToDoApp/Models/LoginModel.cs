using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Models
{
    public class LoginModel
    {
        public bool IsAuthenticated { get; set; }
        public int UserId { get; set; }
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}