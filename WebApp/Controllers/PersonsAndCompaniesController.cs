using App.DAL.EF;
using App.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace WebApp.Controllers;

public class PersonsAndCompaniesController : Controller
{
    private readonly ApplicationDbContext _context;

    public PersonsAndCompaniesController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    // GET Person
    public async Task<IActionResult> EditPerson(Guid? id, Guid? eventId)
    {
        if (id == null)
        {
            return NotFound();
        }
        var person = await _context.Persons
                .FirstOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                return NotFound();
            }
            
            return View(person);
    }
    
    // POST: Person/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    public async Task<IActionResult> EditPerson(Guid id, [Bind("Id, FirstName,LastName,PersonalIdentificationCode,PaymentOption,AdditionInformation")] Person person)
    {
        if (id != person.Id)
        {
            return NotFound();
        }

        Console.WriteLine(ModelState.IsValid);
        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(person);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
            }
            return Redirect($"~/");
        }
        return View(person);
    }

    // POST: Person/Delete/5
    [HttpPost]
    public async Task<IActionResult> DeleteConfirmedPerson(Guid id)
    {
        var person = await _context.Persons.FindAsync(id);
        if (person != null)
        {
            _context.Persons.Remove(person);
            await _context.SaveChangesAsync();
        }
        return Redirect($"~/");
    }
    
    // GET Person
    public async Task<IActionResult> EditCompany(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var company = await _context.Companies
            .FirstOrDefaultAsync(m => m.Id == id);
        if (company == null)
        {
            return NotFound();
        }
            
        return View(company);
    }
    
    // POST: Person/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    public async Task<IActionResult> EditCompany(Guid id, [Bind("Id,CompanyName,CompanyRegisterCode,PaymentOption,AdditionInformation")] Company company)
    {
        if (id != company.Id)
        {
            return NotFound();
        }

        Console.WriteLine(ModelState.IsValid);
        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(company);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
            }
            return Redirect($"~/");
        }
        return View(company);
    }
    
    // POST: Person/Delete/5
    [HttpPost]
    public async Task<IActionResult> DeleteConfirmedCompany(Guid id)
    {
        var company = await _context.Companies.FindAsync(id);
        if (company != null)
        {
            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
        }
        return Redirect($"~/");
    }
    
    // GET: Events/Delete/5
    public async Task<IActionResult> DeletePerson(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var person = await _context.Persons
            .FirstOrDefaultAsync(m => m.Id == id);
        if (person == null)
        {
            return NotFound();
        }

        return View(person);
    }
    
    // GET: Events/Delete/5
    public async Task<IActionResult> DeleteCompany(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var company = await _context.Companies
            .FirstOrDefaultAsync(m => m.Id == id);
        if (company == null)
        {
            return NotFound();
        }

        return View(company);
    }
}