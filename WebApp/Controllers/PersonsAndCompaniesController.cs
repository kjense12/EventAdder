using App.DAL.EF;
using App.Domain;
using Base.Contracts.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace WebApp.Controllers;

public class PersonsAndCompaniesController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IUnitOfWork _UOW;

    public PersonsAndCompaniesController(ApplicationDbContext context, IUnitOfWork unitOfWork)
    {
        _context = context;
        _UOW = unitOfWork;
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
    [HttpPost]
    public async Task<IActionResult> EditPerson(Guid id, [Bind("Id, FirstName,LastName,PersonalIdentificationCode,PaymentOption,AdditionInformation")] Person person)
    {
        var eventId = FindParentEventId(person.Id, true);
        if (id != person.Id)
        {
            return NotFound();
        }
        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(person);
                await _UOW.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
            }
            return Redirect($"~/Events/ShowParticipants/{eventId}");
        }
        return View(person);
    }

    // POST: Person/Delete/5
    [HttpPost]
    public async Task<IActionResult> DeleteConfirmedPerson(Guid id)
    {
        var eventId = FindParentEventId(id, true);
        var person = await _context.Persons.FindAsync(id);

        
        
        if (person != null)
        {
            _context.Persons.Remove(person);
            await _UOW.SaveChangesAsync();
        }
        return Redirect($"~/Events/ShowParticipants/{eventId}");
    }
    
    // GET Company
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
    [HttpPost]
    public async Task<IActionResult> EditCompany(Guid id, [Bind("Id,CompanyName,CompanyRegisterCode,PaymentOption,AdditionInformation")] Company company)
    {
        if (id != company.Id)
        {
            return NotFound();
        }
        var eventId = FindParentEventId(id, false);
        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(company);
                await _UOW.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
            }
            return Redirect($"~/Events/ShowParticipants/{eventId}");
        }
        return View(company);
    }
    
    // POST: Person/Delete/5
    [HttpPost]
    public async Task<IActionResult> DeleteConfirmedCompany(Guid id)
    {
        var eventId = FindParentEventId(id, false);
        
        var company = await _context.Companies.FindAsync(id);
        if (company != null)
        {
            _context.Companies.Remove(company);
            await _UOW.SaveChangesAsync();
        }
        return Redirect($"~/Events/ShowParticipants/{eventId}");
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
    public async Task<IActionResult> DeleteCompany(Guid id)
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

    /// <summary>
    /// Finds parent event guid. Only works since we bind only one person to an event.
    /// </summary>
    /// <param name="personId"></param>
    /// <param name="personToFind"></param>
    /// <returns></returns>
    public Guid FindParentEventId(Guid personId, bool personToFind)
    {
        if (personToFind)
        {
            var eventPersons = _context.EventsPersons.FirstOrDefault(m => m.PersonId == personId);
            if (eventPersons != null)
            {
                return eventPersons.EventId;
            }

            return Guid.Empty;
        }

        var eventCompanies = _context.EventsCompanies.FirstOrDefault(m => m.CompanyId == personId);
        if (eventCompanies != null)
        {
         return  eventCompanies.EventId;
        }
        
        return Guid.Empty;
    }
}