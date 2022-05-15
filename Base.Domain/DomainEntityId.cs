using Base.Contract.Domain;

namespace Base.Domain;

/// <summary>
/// default Guid based Domain Entity
/// </summary>
public abstract class DomainEntityId : DomainEntityId<Guid>, IDomainEntityId
{
}

/// <summary>
/// Universal Domain Entity based on generic PK Type
/// </summary>
/// <typeparam name="TKey"></typeparam>
public abstract class DomainEntityId<TKey> : IDomainEntityId<TKey>
    where TKey : IEquatable<TKey>
{
    public TKey Id { get; set; } = default!;
}