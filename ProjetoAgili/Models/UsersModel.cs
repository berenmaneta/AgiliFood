using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjetoAgili.Models
{
    public class UsersModel
    {
        [Required(ErrorMessage = "Email é obrigatório!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Senha é obrigatória!")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirmação da senha é obrigatória!")]
        [Compare("Password", ErrorMessage = "A senhas não conferem!")]
        public string PasswordCheck { get; set; }
        public int IdUsuario { get; set; }
        public int IdProfile { get; set; }

        public string Name { get; set; }
        public string StreetAddress { get; set; }
        public string Phone { get; set; }
    }
}