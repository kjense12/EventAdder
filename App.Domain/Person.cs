using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain;
public class Person : DomainEntityMetaId
{
    [MaxLength(11)] [MinLength(11)] public string PersonalIdentificationCode { get; set; } = default!;
        [MaxLength(32)]
        [MinLength(2)]
        public string FirstName { get; set; } = default!;
        [MaxLength(32)]
        [MinLength(2)]
        public string LastName { get; set; } = default!;
        public PaymentOptions PaymentOption { get; set; }
        public string? AdditionInformation { get; set; }
        public ICollection<EventPersons>? PersonsParticipatingInEvent { get; set; }

        public string GetFullName()
        {
            var fullName = $"{FirstName} {LastName}";
            return fullName;
        }

    }