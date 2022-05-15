using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
using Base.Contract.Domain;

namespace Base.Domain;

/// <summary>
/// default Guid based Domain Entity
/// </summary>
public abstract class DomainEntityMetaId : DomainEntityMetaId<Guid>, IDomainEntityId
{
    
}

/// <summary>
/// Universal Domain Entity for adding metadata and Id to entities based on generic PK Type
/// </summary>
public abstract class DomainEntityMetaId<TKey> : DomainEntityId<TKey> , IDomainEntityMeta
    where TKey : IEquatable<TKey>
{
    [MaxLength(32)]
    public string? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    [MaxLength(32)]
    public string? UpdatedBy { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}