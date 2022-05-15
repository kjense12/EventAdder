using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain;

public class Event : DomainEntityMetaId
    {
        [MaxLength(128)]
        [MinLength(2)]
        public string EventName { get; set; } = default!;
        [MaxLength(128)]
        [MinLength(2)]
        public string EventLocation { get; set; } = default!;
        public DateTime EventTime { get; set; }
        [MaxLength(4096)]
        public string? EventDescription { get; set; } = default!;
        public ICollection<EventPersons>? PersonsParticipatingInEvent { get; set; }
        public ICollection<EventCompanies>? CompanyParticipatingInEvent { get; set; }

    }
