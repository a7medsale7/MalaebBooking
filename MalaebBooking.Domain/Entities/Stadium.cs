using MalaebBooking.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Domain.Entities;
public class Stadium : Auditable
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Address { get; set; } = string.Empty;
    public string? GoogleMapsUrl { get; set; }
    public decimal PricePerHour { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string? InstapayNumber { get; set; }

    public TimeOnly OpeningTime { get; set; }
    public TimeOnly ClosingTime { get; set; }
    public int SlotDurationMinutes { get; set; } = 60;
    public bool IsActive { get; set; } = true;
    // Foreign Keys
    public int SportTypeId { get; set; }
    public string OwnerId { get; set; } = string.Empty;
    // Navigation Properties
    public SportType SportType { get; set; } = default!;
    public ApplicationUser? Owner { get; set; } = default!;
    public ICollection<StadiumImage> Images { get; set; } = [];
    public ICollection<TimeSlot> TimeSlots { get; set; } = [];
    public ICollection<Review> Reviews { get; set; } = [];
}