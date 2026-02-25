using MalaebBooking.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Domain.Entities;
public class Review : Auditable
{
    public int Id { get; set; }

    // مثلاً التقييم من 1 لـ 5
    public int Rating { get; set; }
    public string? Comment { get; set; }
    // Foreign Keys
    public int StadiumId { get; set; }
    public string PlayerId { get; set; } = string.Empty;
    // Navigation Properties
    public Stadium Stadium { get; set; } = default!;
    public ApplicationUser Player { get; set; } = default!;
}