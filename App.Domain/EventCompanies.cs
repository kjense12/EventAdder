using App.Domain;
using Base.Domain;

namespace App.Domain;
public class EventCompanies : DomainEntityMetaId
    {
        public Guid EventId { get; set; }
        public Event Event { get; set; } = default!;

        public Guid CompanyId { get; set; }
        public Company Company { get; set; } = default!;
    }