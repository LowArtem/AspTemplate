namespace FiveHeadsApp.Core.Exceptions;

/// <summary>
/// Ошибка не найденной сущности
/// </summary>
[Serializable]
public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(Type entityType)
        : base($"{entityType} not found")
    {
    }

    public EntityNotFoundException(Type entityType, object value)
        : base($"{entityType} with {value} value not found")
    {
    }
}