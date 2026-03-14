using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Application.Contracts.Reviews;

public class ReviewResponse
{
    public int Id { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public int StadiumId { get; set; }
    public string PlayerId { get; set; } = string.Empty;
    public string PlayerName { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; }   // ✅ مش CreatedAt
}
