namespace ExpenseTracker.Domain;

/// <summary>
/// Категорія витрати - value object для класифікації витрат
/// </summary>
public class Category
{
    public int Id { get; }
    public string Name { get; }

    public Category(int id, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Назва категорії не може бути порожньою");

        Id = id;
        Name = name;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Category other)
            return false;

        return Id == other.Id && Name == other.Name;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Name);
    }

    public override string ToString()
    {
        return $"[{Id}] {Name}";
    }
}
