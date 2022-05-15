using System.Dynamic;
using App.DAL.EF;
using App.Domain;
using Base.Contracts.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace WebApp.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _UOW;

        public EventsController(ApplicationDbContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _UOW = unitOfWork;
        }

        /// <summary>
        /// Collection class to bind multiple models
        /// </summary>
        public class CollectionDataModel
        {
            public bool IsClientFromCompany { get; set; }
            public Event @Event { get; set; } = default!;
            public IEnumerable<Person> Persons { get; set; } = default!;
            public IEnumerable<Company> Companies { get; set; } = default!;
            public Company SingularCompany { get; set; } = default!;
            public Person SingularPerson { get; set; } = default!;
        } 

        // GET: Events/Details/{GUID}
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
                await _UOW.SaveChangesAsync();
            return Redirect("~/");
        }

        // GET: Events/Edit/{GUID}
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

        // POST: Events/Edit/{GUID}
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
                    await _UOW.SaveChangesAsync();
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

        // GET: Events/Delete/{GUID}
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

        // POST: Events/Delete/{GUID}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var @event = await _context.Events.FindAsync(id);
            _context.Events.Remove(@event);
            await _UOW.SaveChangesAsync();
            return Redirect("~/");
        }

        private bool EventExists(Guid id)
        {
            return _context.Events.Any(e => e.Id == id);
        }

        /// <summary>
        /// Gets event and with that information queries all companies/persons associated with the event
        /// Binds all members into CollectionDataModel 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isClientFromCompany"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Inner join for persons that are attending the event
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public  IEnumerable<Person> GetPersonsParticipatingInEvent(Guid? eventId)
        {
            var persons = _context.Persons.Where(person => person.PersonsParticipatingInEvent.Any(j => j.EventId == eventId));
            
            return persons;
        }
        
        /// <summary>
        /// Inner join for companis that are attending the event
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public IEnumerable<Company> GetCompaniesParticipatingInEvent(Guid? eventId)
        {
            var companies = _context.Companies.Where(company => company.CompanyParticipatingInEvent.Any(j => j.EventId == eventId));
            
            return companies;
        }
        
        /// <summary>
        /// Creates and binds person to the event
        /// Checks if event has already happened or not.
        /// </summary>
        /// <param name="personDataModel"></param>
        /// <returns></returns>
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
                await _UOW.SaveChangesAsync();
                return Redirect($"/Events/ShowParticipants/{@event.Id}");
            }
        
        /// <summary>
        /// Creates and binds company to the event
        /// Checks if event has already happened or not.
        /// </summary>
        /// <param name="personDataModel"></param>
        /// <returns></returns>
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
                await _UOW.SaveChangesAsync();
                return Redirect($"/Events/ShowParticipants/{@event.Id}");
            }
    }
}
