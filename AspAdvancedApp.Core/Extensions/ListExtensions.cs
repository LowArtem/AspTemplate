namespace AspAdvancedApp.Core.Extensions;

/// <summary>
/// Расширения для List
/// </summary>
public static class ListExtensions
{
    /// <summary>
    /// Словарь символов
    /// </summary>
    private static Dictionary<string, string> ServicSymbols = new()
    {
        {@"\", @"\\"},
        {"\0", @"\\0"},
        {"\a", @"\\a"},
        {"\b", @"\\b"},
        {"\f", @"\\f"},
        {"\n", @"\\n"},
        {"\r", @"\\r"},
        {"\t", @"\\t"},
        {"\v", @"\\v"},
    };

    /// <summary>
    /// Объединяет список значений в строку
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="separator"></param>
    /// <returns></returns>
    public static string Join<T>(this IEnumerable<T> list, string separator, bool isEscaping = true)
    {
        // Замена спец-символов
        var newList = list?.Select(p => p.ToString());
        if (isEscaping)
            newList = list.Escaping();
        return string.Join(separator, newList);
    }

    /// <summary>
    /// Объединяет список значений в строку
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="separator"></param>
    /// <returns></returns>
    public static string Join<T>(this IEnumerable<T> list, char separator, bool isEscaping = true)
    {
        // Замена спец-символов
        var newList = list?.Select(p => p.ToString());
        if (isEscaping)
            newList = list.Escaping();
        return string.Join(separator, newList);
    }

    /// <summary>
    /// Экранирование спец-символов
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list">Список объектов</param>
    /// <returns></returns>
    private static IEnumerable<string?> Escaping<T>(this IEnumerable<T> list)
    {
        return list.Select(p =>
        {
            var s = p?.ToString();
            if (string.IsNullOrWhiteSpace(s)) return s;

            foreach (var item in ServicSymbols)
            {
                s = s.Replace(item.Key, item.Value);
            }

            return s;
        }).ToList();
    }
}