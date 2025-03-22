﻿using System.ComponentModel.DataAnnotations;

namespace ShoeStoreMvc.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string Username { get; set; }  // Thêm trường Username

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
