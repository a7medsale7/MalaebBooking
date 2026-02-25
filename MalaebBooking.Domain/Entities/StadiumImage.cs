using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Domain.Entities;
public class StadiumImage
{
    public int Id { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public bool IsPrimary { get; set; }
    public int DisplayOrder { get; set; }
    // Foreign Keys
    public int StadiumId { get; set; }

    // Navigation
    public Stadium Stadium { get; set; } = default!;
}