using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
namespace LABA1.ViewModel
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name ="Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Логин")]
        public string Nickname { get; set; }

        [Required]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        [StringLength(16, ErrorMessage = "{0} должен иметь минимум {2} и максимум {1} символов.", MinimumLength = 5)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [Display(Name ="Подтверждение пароля")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }
    }
}
