using MalaebBooking.Domain.Entities.Base;
using MalaebBooking.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Domain.Entities;
public class StadiumOwnerProfile : Auditable
{
    public int Id { get; set; }
    // الربط مع اليوزر
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = default!;
    // 1. إثبات الهوية الشخصية
    public string NationalId { get; set; } = string.Empty;
    public string NationalIdImageFront { get; set; } = string.Empty;
    public string NationalIdImageBack { get; set; } = string.Empty;

    // 2. إثبات "الصفة" كصاحب ملاعب (المستندات اللي طلبتها)

    // صورة عقد ملكية عام أو عقد إدارة (بيثبت إنه فعلاً شغال في المجال ده)
    public string? OwnershipContractImage { get; set; }

    // السجل التجاري (لو مسجل كشركة إدارة ملاعب)
    public string? CommercialRegisterNumber { get; set; }
    public string? CommercialRegisterImage { get; set; }
    // رخصة مزاولة النشاط الرياضي (لو معاه رخصة من وزارة الشباب والرياضة مثلاً)
    public string? TradeLicenseImage { get; set; }
    // 3. حالة التوثيق والتحكم
    public StadiumOwnerStatus Status { get; set; } = StadiumOwnerStatus.Pending;
    public string? AdminRemarks { get; set; }

    public string? ApprovedById { get; set; }
    public ApplicationUser? ApprovedBy { get; set; }
    public DateTime? VerifiedAt { get; set; }
    public ICollection<Stadium> Stadiums { get; set; } = [];

}