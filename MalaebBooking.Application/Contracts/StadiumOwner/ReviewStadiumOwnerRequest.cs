using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Application.Contracts.StadiumOwner;
public class ReviewStadiumOwnerRequest
{
    public bool Approve { get; set; }
    public string? Remarks { get; set; }
}