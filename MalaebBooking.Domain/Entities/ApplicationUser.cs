using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Domain.Entities;
public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public bool IsDisabled { get; set; }
    // --- الربط مع بروفايل التوثيق الجديد (One-to-One) ---
    public StadiumOwnerProfile? StadiumOwnerProfile { get; set; }
    // Navigation Properties (زي ما هي عندك)
    public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
    public ICollection<Booking> Bookings { get; set; } = [];
    public ICollection<Review> Reviews { get; set; } = [];
}