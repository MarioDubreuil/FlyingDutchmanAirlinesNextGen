using FlyingDutchmanAirlines.DatabaseLayer;
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
        return await base.SaveChangesAsync(cancellationToken);
    }
}
