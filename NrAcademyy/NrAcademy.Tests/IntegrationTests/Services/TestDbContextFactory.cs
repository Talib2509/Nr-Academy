using Microsoft.EntityFrameworkCore;
using NrAcademyDAL.Context;
using System;

namespace NrAcademy.Tests.IntegrationTests;

public class TestDbContextFactory
{
    public AppDbContext CreateContext()
    {
        // Hər test üçün tamamilə yeni və unikal bir baza adı (Guid)
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new AppDbContext(options);

        // Bazanın yaradılmasına və sxemin hazır olmasına zəmanət veririk
        context.Database.EnsureCreated();

        return context;
    }
}