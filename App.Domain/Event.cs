using Base.Domain;

namespace App.Domain;

public class Event : DomainEntityMetaId
    {
        public string EventName { get; set; } = default!;
        public string EventLocation { get; set; } = default!;
        public DateTime EventTime { get; set; }
        public string EventDescription { get; set; } = default!;
        public ICollection<EventPersons>? PersonsParticipatingInEvent { get; set; }
        public ICollection<EventCompanies>? CompanyParticipatingInEvent { get; set; }

    }
