using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class RegisterDto
    {
        // [Required] indica che il valore sottostante è obbligatorio da inserire, quindi non potrà essere lasciato vuoto al momento dell iscrizione dell account
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}