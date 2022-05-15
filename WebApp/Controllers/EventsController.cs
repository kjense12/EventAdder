#nullable disable
using System.Dynamic;
using App.DAL.EF;
using App.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace WebApp.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public class CollectionDataModel
        {
            public bool IsClientFromCompany { get; set; }
            public Event @Event { get; set; }
            public IEnumerable<Person> Persons { get; set; }
            public IEnumerable<Company> Companies { get; set; }
            public Company SingularCompany { get; set; }
            public Person SingularPerson { get; set; }
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event @event)
        {
            var newEvent = new Event()
            {
                EventName = @event.EventName,
                EventLocation = @event.EventLocation,
                EventTime = @event.EventTime,
                EventDescription = @event.EventDescription
            };
            _context.Add(newEvent);
                await _context.SaveChangesAsync();
                return Redirect("~/");
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,EventName,EventLocation,EventTime,EventDescription")] Event @event)
        {
            if (id != @event.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Redirect("~/");
            }
            return View(@event);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var @event = await _context.Events.FindAsync(id);
            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();
            return Redirect("~/");
        }

        private bool EventExists(Guid id)
        {
            return _context.Events.Any(e => e.Id == id);
        }

        // GET: Events/ShowParticipants/5
        public async Task<IActionResult> ShowParticipants(Guid? id, bool isClientFromCompany)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events.
                FirstOrDefaultAsync(m => m.Id == id);
            if(@event == null)
            {
                return NotFound();
            }
            
            //Get Participants
            var participants = GetPersonsParticipatingInEvent(id);
            var companies = GetCompaniesParticipatingInEvent(id);
            
            var model = new CollectionDataModel();
            
            model.Persons = participants;
            model.Companies = companies;
            model.@Event = @event;
            model.IsClientFromCompany = isClientFromCompany;
            model.SingularPerson = new Person();
            model.SingularCompany = new Company();

            return View(model);
        }

        public  IEnumerable<Person> GetPersonsParticipatingInEvent(Guid? eventId)
        {
            var persons = _context.Persons.Where(person => person.PersonsParticipatingInEvent.Any(j => j.EventId == eventId));
            
            return persons;
        }
        
        public IEnumerable<Company> GetCompaniesParticipatingInEvent(Guid? eventId)
        {
            var companies = _context.Companies.Where(company => company.CompanyParticipatingInEvent.Any(j => j.EventId == eventId));
            
            return companies;
        }
        
        // POST: Persons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePerson(CollectionDataModel personDataModel)
        {
            if (personDataModel.Event.EventTime < DateTime.UtcNow)
            {
                return Redirect($"/Events/ShowParticipants/{personDataModel.Event.Id}");
            }

            if (!Int64.TryParse(personDataModel.SingularPerson.PersonalIdentificationCode, out _))
            {
                return Redirect($"/Events/ShowParticipants/{personDataModel.Event.Id}");
            }

            var person = new Person()
            {
                PersonalIdentificationCode = personDataModel.SingularPerson.PersonalIdentificationCode,
                FirstName = personDataModel.SingularPerson.FirstName,
                LastName = personDataModel.SingularPerson.LastName,
                PaymentOption = personDataModel.SingularPerson.PaymentOption,
                AdditionInformation = personDataModel.SingularPerson.AdditionInformation,
                UpdatedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };
            var eventPerson = new EventPersons()
                {
                    EventId = personDataModel.Event.Id,
                    PersonId = person.Id,
                    UpdatedAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow
                };

            if (person.PersonsParticipatingInEvent == null)
            {
                person.PersonsParticipatingInEvent = new List<EventPersons>();
            }
            
            person.PersonsParticipatingInEvent.Add(eventPerson);

            var @event = _context.Events.FirstOrDefault(m => m.Id == personDataModel.Event.Id);

            if (@event!.PersonsParticipatingInEvent == null)
            {
                @event.PersonsParticipatingInEvent = new List<EventPersons>();
            }
            
            @event.PersonsParticipatingInEvent.Add(eventPerson);
                
                _context.Persons.Add(person);
                _context.EventsPersons.Add(eventPerson);
                await _context.SaveChangesAsync();
                return Redirect($"/Events/ShowParticipants/{@event.Id}");
            }
        // POST: Persons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCompany(CollectionDataModel personDataModel)
        {
            if (personDataModel.Event.EventTime < DateTime.UtcNow)
            {
                return Redirect($"/Events/ShowParticipants/{personDataModel.Event.Id}");
            }

            if (!Int64.TryParse(personDataModel.SingularCompany.CompanyRegisterCode, out _))
            {
                return Redirect($"/Events/ShowParticipants/{personDataModel.Event.Id}");
            }

            var company = new Company()
            {
                CompanyName = personDataModel.SingularCompany.CompanyName,
                CompanyRegisterCode = personDataModel.SingularCompany.CompanyRegisterCode,
                PaymentOption = personDataModel.SingularCompany.PaymentOption,
                AdditionInformation = personDataModel.SingularCompany.AdditionInformation,
                UpdatedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };
            var eventCompany = new EventCompanies()
                {
                    EventId = personDataModel.Event.Id,
                    CompanyId = company.Id,
                    UpdatedAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow
                };

            if (company.CompanyParticipatingInEvent == null)
            {
                company.CompanyParticipatingInEvent = new List<EventCompanies>();
            }
            
            company.CompanyParticipatingInEvent.Add(eventCompany);

            var @event = _context.Events.FirstOrDefault(m => m.Id == personDataModel.Event.Id);

            if (@event!.CompanyParticipatingInEvent == null)
            {
                @event.CompanyParticipatingInEvent = new List<EventCompanies>();
            }
            
            @event.CompanyParticipatingInEvent.Add(eventCompany);
                
                _context.Companies.Add(company);
                _context.EventsCompanies.Add(eventCompany);
                await _context.SaveChangesAsync();
                return Redirect($"/Events/ShowParticipants/{@event.Id}");
            }
    }
}
