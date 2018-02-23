using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjetoAgili.Models
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "Senha é obrigatória!")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Nova senha é obrigatória!")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirmação da senha é obrigatória!")]
        [Compare("Password", ErrorMessage = "A senhas não conferem!")]
        public string ConfirmPassword { get; set; }
    }
}