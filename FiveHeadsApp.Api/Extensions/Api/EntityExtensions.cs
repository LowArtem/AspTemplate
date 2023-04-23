using FiveHeadsApp.Core.Model._Base;
using AutoMapper;

namespace FiveHeadsApp.Api.Extensions.Api;

public static class EntityExtensions
{
    /// <summary>
    /// Обновление объекта
    /// </summary>
    /// <typeparam name="T">Тип объекта</typeparam>
    /// <param name="source">Исходные данные - от куда взять значения полей</param>
    /// <param name="mapper">Мапер</param>
    /// <param name="destination">Данные "места назначения" - куда вставить новые значения</param>
    /// <returns>Обновленный объект</returns>
    public static T UpdateEntity<T>(this T source, IMapper mapper, T destination)
        where T : IEntity
    {
        var res = mapper.Map(source, destination);

        // Не изменяем даты, они управляются другой логикой
        res.DateCreate = destination.DateCreate;
        res.DateUpdate = destination.DateUpdate;

        return res;
    }
}