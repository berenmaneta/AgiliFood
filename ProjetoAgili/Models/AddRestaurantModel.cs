using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjetoAgili.Models
{
    public class AddRestaurantModel
    {
        [Required(ErrorMessage = "Email é obrigatório!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Nome é obrigatório!")]
        public string Name { get; set; }

        public string StreetAddress { get; set; }

        public string Phone { get; set; }
    }
}