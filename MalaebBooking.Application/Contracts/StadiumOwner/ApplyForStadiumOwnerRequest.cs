using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Application.Contracts.StadiumOwner;
public class ApplyForStadiumOwnerRequest
{
    public string NationalId { get; set; } = string.Empty;
    public IFormFile NationalIdImageFront { get; set; } = default!;
    public IFormFile NationalIdImageBack { get; set; } = default!;
    public IFormFile? OwnershipContractImage { get; set; }
    public string? CommercialRegisterNumber { get; set; }
    public IFormFile? CommercialRegisterImage { get; set; }
    public IFormFile? TradeLicenseImage { get; set; }
}