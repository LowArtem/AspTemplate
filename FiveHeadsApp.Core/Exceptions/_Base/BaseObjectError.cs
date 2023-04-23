namespace FiveHeadsApp.Core.Exceptions._Base;

/// <summary>
/// Базовый класс ошибки с объектом
/// </summary>
public class BaseObjectError : BaseError
{
    /// <summary>
    /// Объект ошибки
    /// </summary>
    public object? ErrorObject { get; set; }

    public BaseObjectError(string errorMessage, object? errorObject) : base(errorMessage)
    {
        ErrorObject = errorObject;
    }
}