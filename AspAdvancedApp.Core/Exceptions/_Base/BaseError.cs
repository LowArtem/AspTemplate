namespace AspAdvancedApp.Core.Exceptions._Base;

/// <summary>
/// Базовый класс ошибки
/// </summary>
public class BaseError
{
    /// <summary>
    /// Сообщение ошибки
    /// </summary>
    public string ErrorMessage { get; set; }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="errorMessage">сообщение ошибки</param>
    public BaseError(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }
}