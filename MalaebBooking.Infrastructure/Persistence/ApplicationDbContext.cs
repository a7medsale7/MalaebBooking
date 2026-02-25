using MalaebBooking.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Infrastructure.Persistence;
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
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
}