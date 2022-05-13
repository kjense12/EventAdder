using App.Domain;
using Base.Domain;

namespace App.Domain;
public class EventPersons : DomainEntityMetaId
    {
        public Guid EventId { get; set; }
        public Event Event { get; set; } = default!;

        public Guid PersonId { get; set; }
        public Person Person { get; set; } = default!;
    }