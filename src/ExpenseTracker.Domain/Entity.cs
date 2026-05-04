namespace ExpenseTracker.Domain;

/// <summary>
/// Абстрактна базова сутність для використання ID
/// </summary>
public abstract class Entity
{
    public int Id { get; protected set; }

    public override bool Equals(object? obj)
    {
        if (obj is not Entity other || GetType() != obj.GetType())
            return false;

        return Id == other.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
