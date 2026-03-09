using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Application.Contracts.SportTypes;
public class UploadSportTypeIconRequest
{
    public IFormFile File { get; set; } = default!;
}