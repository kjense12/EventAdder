using Base.Domain;

namespace App.Domain;
public class Person : DomainEntityMetaId
    {
        public int PersonalIdentificationCode { get; set; }
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public PaymentOptions PaymentOption;
        public string AdditionInformation { get; set; } = default!;
        public ICollection<EventPersons>? PersonsParticipatingInEvent { get; set; }

    }