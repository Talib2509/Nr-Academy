using Microsoft.EntityFrameworkCore;
using NrAcademyCORE.Entities.Identity;
using NrAcademyDAL.Context;
using System;

namespace NrAcademy.Tests.IntegrationTests;

public abstract class BaseIntegrationTest : IDisposable
{
    protected readonly AppDbContext _context;

    protected BaseIntegrationTest(TestDbContextFactory factory)
    {
        // Factory-nin özünün null olmadığını yoxla
        if (factory == null) throw new ArgumentNullException(nameof(factory));

        // DbContext-i yarat və protected field-ə mənimsət
        _context = factory.CreateContext();
    }
    protected async Task SeedRolesAsync()
    {
        // Rolları siyahı halında müəyyən edirik
        var roles = new[] { "Admin", "Student", "Teacher" };

        foreach (var roleName in roles)
        {
            var normalized = roleName.ToUpperInvariant();
            // Artıq mövcud olub-olmadığını yoxlayırıq
            if (!await _context.Roles.AnyAsync(r => r.NormalizedName == normalized))
            {
                await _context.Roles.AddAsync(new AppRole
                {
                    Name = roleName,
                    NormalizedName = normalized,
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                });
            }
        }

        await _context.SaveChangesAsync();
        // InMemory bazada bəzən mütləq lazımdır ki, Context yenilənsin
        _context.ChangeTracker.Clear();
    }
    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}