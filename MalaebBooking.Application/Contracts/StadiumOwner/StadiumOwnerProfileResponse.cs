using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Application.Contracts.StadiumOwner;
public class StadiumOwnerProfileResponse
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string NationalId { get; set; } = string.Empty;
    public string NationalIdImageFront { get; set; } = string.Empty;
    public string NationalIdImageBack { get; set; } = string.Empty;
    public string? OwnershipContractImage { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? AdminRemarks { get; set; }
    public DateTime CreatedOn { get; set; }
}