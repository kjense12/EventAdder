using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain;

public class Company : DomainEntityMetaId
    {
        [MaxLength(32)]
        [MinLength(2)]
        public string CompanyName { get; set; } = default!;
        [MaxLength(18)]
        [MinLength(6)]
        public string CompanyRegisterCode { get; set; } = default!;
        public int? ParticipatingGuestsNumber { get; set; }
        public PaymentOptions PaymentOption { get; set; }
        [MaxLength(4096)]
        public string? AdditionInformation { get; set; } = default!;

        public ICollection<EventCompanies>? CompanyParticipatingInEvent { get; set; }
    }