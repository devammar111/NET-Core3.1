﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DastgyrAPI.Models.ViewModels.RequestModels
{
    public class ChangePasswordRequest
    {
        //public string Email { get; set; }
        public string OldPassword { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

    }
}
