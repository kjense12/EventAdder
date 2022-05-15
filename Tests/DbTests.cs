#nullable disable
using System;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks.Dataflow;
using App.DAL.EF;
using App.Domain;
using ASP;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using NUnit.Framework;
using NUnit.Framework.Internal;
using WebApp.Controllers;

namespace Tests;

public class DbTests
{
    private ApplicationDbContext _context;
    
    public DbTests()
    {
        _context = GetDbContext();
    }

    public static ApplicationDbContext GetDbContext()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(connection).Options;

        var dbContext = new ApplicationDbContext(options);

        using (var context = new ApplicationDbContext(options))
        {
            context.Database.EnsureCreated();
        }

        return dbContext!;
    }

    [Test]
    public void TestAddClient()
    {

        var client = new Person()
        {
            FirstName = "Test",
            LastName = "Test",
            PersonalIdentificationCode = "32802300213",
            PaymentOption = 0
        };
        
        _context.Persons.Add(client);

        _context.SaveChanges();

        var contextPersonTrue = _context.Persons.FirstOrDefault();


        Assert.That(contextPersonTrue, Is.Not.Null);
    }
    
    [Test]
    public void TestAddCompany()
    {
        var company = new Company()
        {
            CompanyName = "Test",
            CompanyRegisterCode = "Test",
            PaymentOption = 0
        };
        
        _context.Companies.Add(company);

        _context.SaveChanges();

        var contextCompanyTrue = _context.Companies.FirstOrDefault();


        Assert.That(contextCompanyTrue, Is.Not.Null);
    }
    
    [Test]
    public void TestAddEvent()
    {
        var @event = new Event()
        {
            EventName = "Test",
            EventLocation =  "Test",
            EventTime = DateTime.UtcNow,
        };
        
        _context.Events.Add(@event);

        _context.SaveChanges();

        var contextEventTrue = _context.Events.FirstOrDefault();


        Assert.That(contextEventTrue, Is.Not.Null);
    }
}