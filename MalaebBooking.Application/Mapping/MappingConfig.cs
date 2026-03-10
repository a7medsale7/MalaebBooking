using MalaebBooking.Application.Contracts.Bookings;
using MalaebBooking.Domain.Entities;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Application.Mapping;

public class MappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // إعدادات الـ Mapping للـ BookingDetailsResponse
        config.NewConfig<Booking, BookingDetailsResponse>()
            // سحب بيانات الملعب والوقت من الـ TimeSlot المرتبط بالحجز
            .Map(dest => dest.StadiumId, src => src.TimeSlot != null ? src.TimeSlot.StadiumId : 0)
            .Map(dest => dest.Date, src => src.TimeSlot != null ? src.TimeSlot.Date : default)
            .Map(dest => dest.StartTime, src => src.TimeSlot != null ? src.TimeSlot.StartTime : default)
            .Map(dest => dest.EndTime, src => src.TimeSlot != null ? src.TimeSlot.EndTime : default)

            // سحب اسم اللاعب من الـ ApplicationUser المرتبط بالحجز
            // عدّل الـ FirstName و الـ LastName لو أسماءهم عندك مختلفة في كلاس الـ ApplicationUser
            .Map(dest => dest.PlayerName, src => src.Player != null ? $"{src.Player.FirstName} {src.Player.LastName}" : string.Empty);

        // باقي الخصائص المتطابقة في الاسمين (Id, Status, TotalPrice, etc.) 
        // الـ Mapster بينقلها لوحده تلقائي ومش محتاجين نكتبلها Map.
    }
}
