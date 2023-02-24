using System.ComponentModel.DataAnnotations;

namespace Exam.Models.DataEntities
{
    public class Login
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
