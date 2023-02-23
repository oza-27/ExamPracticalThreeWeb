﻿using System.ComponentModel.DataAnnotations;

namespace Exam.Models
{
    public class Register
    {
        [Required]
        public string Username { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
