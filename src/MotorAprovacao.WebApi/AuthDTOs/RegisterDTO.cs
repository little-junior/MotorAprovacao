using System.ComponentModel.DataAnnotations;

namespace MotorAprovacao.WebApi.AuthDTOs
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Nome do Usuario é obrigatorio!")]
        public string? UserName { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "O e-mail é obrigatorio!")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatorio!")]
        public string? Password { get; set; }
    }
}
