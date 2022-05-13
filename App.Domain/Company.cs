using Base.Domain;

namespace App.Domain;
public class Company : DomainEntityMetaId
    {
        public string CompanyName { get; set; } = default!;
        public string CompanyRegisterCode { get; set; } = default!;
        public int ParticipatingGuestsNumber { get; set; }
        public PaymentOptions PaymentOption;
        public string AdditionInformation { get; set; } = default!;

        public ICollection<EventCompanies>? CompanyParticipatingInEvent { get; set; }
    }