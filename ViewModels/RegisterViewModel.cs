using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System;

namespace SocialMediaMustBeGood2.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Логин")]
        public string LogIn { get; set; }

        [Required]
        [Display(Name = "Имя")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string PasswordConfirm { get; set; }

        [Required]
        [Display(Name = "Ссылка на аватар")]
        public string Avatar { get; set; }

        [Required]
        [Display(Name = "О себе")]
        public string UserInfo { get; set; }

        [Required]
        [Display(Name = "Пол")]
        public string Sex { get; set; }

        [Required]
        [Display(Name = "Номер телефона")]
        //[DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

    }
}