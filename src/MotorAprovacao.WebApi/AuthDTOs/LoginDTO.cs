using System.ComponentModel.DataAnnotations;

namespace MotorAprovacao.WebApi.AuthDTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Nome do Usuario é obrigatorio!")]
        public string? UserName { get; set; }


        [Required(ErrorMessage = "A senha é obrigatorio!")]
        public string? Password { get; set; }
    }
}
