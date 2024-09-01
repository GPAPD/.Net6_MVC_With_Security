using System.ComponentModel.DataAnnotations;

namespace ABC_Company.Models
{
    public class LogInModel : ErrorModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
