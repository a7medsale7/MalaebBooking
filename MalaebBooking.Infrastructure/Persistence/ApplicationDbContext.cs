using MalaebBooking.Domain.Entities;
using MalaebBooking.Domain.Entities.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Infrastructure.Persistence;
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options , IHttpContextAccessor httpContextAccessor) : base(options)
    {
        this.httpContextAccessor = httpContextAccessor;
    }
    // هنا بنعرف الـ Tables بتاعتنا
    public DbSet<SportType> SportTypes { get; set; }
    public DbSet<Stadium> Stadiums { get; set; }
    public DbSet<StadiumImage> StadiumImages { get; set; }
    public DbSet<TimeSlot> TimeSlots { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Review> Reviews { get; set; }
    // RefreshTokens هيبقوا جوا الـ ApplicationUser عشان عملناهم [Owned]
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder); // مهم جداً عشان جداول الـ Identity تتكريت
        // السطر السحري ده بيروح يدور على أي كلاس بيطبق الـ IEntityTypeConfiguration
        // في نفس الـ Assembly دي، ويطبق كل الـ Fluent API بتاعنا مرة واحدة!
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }


    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // جلب الـ UserId من الـ HttpContext
        var userId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            userId = "System"; // قيمة افتراضية لو مش موجود مستخدم مسجل دخول
        }

        foreach (var entry in ChangeTracker.Entries<Auditable>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedById = userId;
                    entry.Entity.CreatedOn = DateTime.UtcNow; // ضبط تاريخ الإنشاء
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdatedById = userId;
                    entry.Entity.UpdatedOn = DateTime.UtcNow; // ضبط تاريخ آخر تعديل
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}