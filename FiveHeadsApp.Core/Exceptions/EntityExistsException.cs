namespace FiveHeadsApp.Core.Exceptions;

/// <summary>
/// Ошибка повторного добавления уже существующей сущности
/// </summary>
[Serializable]
public class EntityExistsException : Exception
{
    public EntityExistsException(Type entityType)
        : base($"This {entityType} already exists")
    {
    }

    public EntityExistsException(Type entityType, object value)
        : base($"The {entityType} with {value} value already exists")
    {
    }
}