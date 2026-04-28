using Microsoft.EntityFrameworkCore;
using NrAcademyCORE.Entities.Identity;
using NrAcademyDAL.Context;
using System;

namespace NrAcademy.Tests.IntegrationTests;

public abstract class BaseIntegrationTest : IDisposable
{
    protected readonly AppDbContext _dbContext;

    protected BaseIntegrationTest(TestDbContextFactory factory)
    {
        // Factory-nin özünün null olmadığını yoxla
        if (factory == null) throw new ArgumentNullException(nameof(factory));

        // DbContext-i yarat və protected field-ə mənimsət
        _dbContext = factory.CreateContext();
    }
    protected async Task SeedRolesAsync()
    {
        // IdentityRole<int> əvəzinə AppRole istifadə edirik
        var roles = new[] { "Admin", "Student", "Teacher" };
        foreach (var roleName in roles)
        {
            if (!await _dbContext.Roles.AnyAsync(r => r.Name == roleName))
            {
                await _dbContext.Roles.AddAsync(new AppRole
                {
                    Name = roleName,
                    NormalizedName = roleName.ToUpper()
                });
            }
        }
        await _dbContext.SaveChangesAsync();
    }
    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
}