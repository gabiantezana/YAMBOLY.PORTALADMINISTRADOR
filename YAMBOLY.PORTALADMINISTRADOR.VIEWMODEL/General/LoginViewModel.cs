﻿using System;
using System.ComponentModel.DataAnnotations;

namespace YAMBOLY.PORTALADMINISTRADOR.VIEWMODEL.General
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Campo requerido")]
        [Display(Name = "Usuario")]
        public String Codigo { get; set; }
        [Required(ErrorMessage = "Campo requerido")]
        public String Password { get; set; }
    }
}