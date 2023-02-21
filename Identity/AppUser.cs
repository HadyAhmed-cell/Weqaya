using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace VirtualClinic.Identity
{
    public class AppUser : IdentityUser
    {
        [Required, MaxLength(100)]
        public string Fname { get; set; }
        [Required, MaxLength(100)]

        public string Lname { get; set; }

    }
}
