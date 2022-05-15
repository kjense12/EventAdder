namespace Base.Contract.Domain;

/// <summary>
/// Universal Domain Entity interface for adding metadata to entities
/// </summary>
public interface IDomainEntityMeta
{
    public string? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
}