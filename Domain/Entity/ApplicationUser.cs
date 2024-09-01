
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entity;

// Add profile data for application users by adding properties to the AplicationUser class
public class ApplicationUser : IdentityUser
{
    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string FirstName { get; set; }

    [Column(TypeName = "nvarchar(100)")]
    public string LastName { get; set; }
}

