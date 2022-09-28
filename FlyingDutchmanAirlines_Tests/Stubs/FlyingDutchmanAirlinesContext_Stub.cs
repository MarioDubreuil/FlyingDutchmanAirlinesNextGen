using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace FlyingDutchmanAirlines_Tests.Stubs;

public class FlyingDutchmanAirlinesContext_Stub : FlyingDutchmanAirlinesContext
{
    public FlyingDutchmanAirlinesContext_Stub(DbContextOptions<FlyingDutchmanAirlinesContext> options) : base(options)
    {
    }

    public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var pendingChanges = ChangeTracker.Entries().Where(e => e.State == EntityState.Added);
        var bookings = pendingChanges.Select(e => e.Entity).OfType<Booking>();
        if (bookings.Any(b => b.CustomerId != 1))
        {
            throw new Exception("Database Error!");
        }
        await base.SaveChangesAsync(cancellationToken);
        return 1;
    }
}
