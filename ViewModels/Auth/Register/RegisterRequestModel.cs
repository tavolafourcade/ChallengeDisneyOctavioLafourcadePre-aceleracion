﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChallengeDisneyOctavioLafourcadePre_Aceleracion.ViewModels.Auth.Register
{
    public class RegisterRequestModel
    {
        [Required]
        [MinLength(6)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

    }
}
