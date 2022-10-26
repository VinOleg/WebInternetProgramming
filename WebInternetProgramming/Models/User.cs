using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebInternetProgramming.Models
{
    public class User : IdentityUser
    {
        public string FIO { get; set; }
    }
}
