using FiveHeadsApp.Api.Extensions.Api;
using FiveHeadsApp.Core.Model._Base;
using FiveHeadsApp.Core.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FiveHeadsApp.Api.Api._Base;

/// <summary>
/// Базовый контроллер выполняющий CRUD операции
/// </summary>
[ApiController]
public abstract class BaseCrudController<T, TAdd, TResponse> : ControllerBase
    where T : IEntity
{
    /// <summary>
    /// Репозиторий
    /// </summary>
    private readonly IEfCoreRepository<T> _repository;

    protected readonly ILogger<BaseCrudController<T, TAdd, TResponse>> _logger;
    protected readonly IMapper _mapper;

    /// <summary>
    /// Контроллер
    /// </summary>
    /// <param name="repository">репозиторий</param>
    /// <param name="logger">логирование</param>
    /// <param name="mapper">маппинг</param>
    public BaseCrudController(IEfCoreRepository<T> repository,
        ILogger<BaseCrudController<T, TAdd, TResponse>> logger, IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    /// <summary>
    /// Получение списка записей
    /// </summary>
    protected virtual IQueryable<T> List => _repository.GetListQuery();

    /// <summary>
    /// Получение списка всех записей
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [SwaggerResponse(200, "Список записей успешно получен")]
    public virtual ActionResult<List<TResponse>> ListEntities()
    {
        var result = List.OrderBy(p => p.DateCreate)
            .ToList();

        return Ok(_mapper.Map<List<TResponse>>(result));
    }

    /// <summary>
    /// Получение списка части записей
    /// </summary>
    /// <param name="limit">Количество записей в ответе (выбираются первые {limit} записей из выборки после {skip} элементов)
    /// <br/><br/>
    /// <b>Max value = 1000</b>
    /// </param>
    /// <param name="skip">Пропускается первых {skip} записей</param>
    /// <returns></returns>
    [HttpGet("piece")]
    [SwaggerResponse(200, "Список записей успешно получен")]
    public virtual ActionResult<List<TResponse>> ListEntitiesPiece(int limit = 1000, int skip = 0)
    {
        if (limit is < 0 or > 1000) limit = 1000;
        if (skip < 0) skip = 0;

        var result = List.OrderBy(p => p.DateCreate)
            .Skip(skip)
            .Take(limit)
            .Select(x => _mapper.Map<TResponse>(x))
            .ToList();

        return Ok(result);
    }

    /// <summary>
    /// Получение записи
    /// </summary>
    /// <param name="id">Идентификатор записи</param>
    /// <returns>Запись</returns>
    /// <response code="404">Если записи с данным идентификатором не существует</response>   
    [HttpGet("{id:int}")]
    [SwaggerResponse(404, "Запись с таким идентификатором не существует")]
    public virtual ActionResult<TResponse> Get(int id)
    {
        var entity = List.FirstOrDefault(p => p.Id == id);

        if (entity == null) return NotFound("Id не найден");
        return Ok(_mapper.Map<TResponse>(entity));
    }

    /// <summary>
    /// Добавление записи
    /// </summary>
    /// <param name="model">Запись</param>
    /// <response code="400">Запись не прошла валидацию</response>
    /// <response code="500">При добавлении записи произошла ошибка на сервере</response>   
    /// <returns>Добавленная запись</returns>
    [HttpPost]
    [SwaggerResponse(200, "Запись успешно добавлена. Содержит информацию о добавленной записи", typeof(BaseEntity))]
    [SwaggerResponse(400, "Ошибка валидации")]
    [SwaggerResponse(500, "Ошибка при добавлении записи")]
    public virtual ActionResult<TResponse> Add(TAdd model)
    {
        var mapped = _mapper.Map<T>(model);
        _repository.Add(mapped);
        _repository.SaveChanges();

        var created = _repository.GetListQuery().SingleOrDefault(x => x.DateCreate == mapped.DateCreate);

        return created?.Id > 0
            ? Ok(_mapper.Map<TResponse>(created))
            : StatusCode(500, "Произошла ошибка при добавлении записи");
    }

    /// <summary>
    /// Добавление записей
    /// </summary>
    /// <param name="models">Записи</param>
    /// <response code="400">Записи не прошли валидацию</response>
    /// <response code="500">При добавлении записи произошла ошибка на сервере</response>   
    /// <returns>Добавленная запись</returns>
    [HttpPost("range")]
    [SwaggerResponse(200, "Записи успешно добавлены. Содержит список добавленных записей", typeof(List<BaseEntity>))]
    [SwaggerResponse(500, "Произошла ошибка при добавлении записей")]
    public virtual ActionResult<List<TResponse>> AddRange(List<TAdd> models)
    {
        var mapped = _mapper.Map<List<T>>(models);

        _repository.AddRange(mapped);
        _repository.SaveChanges();

        var created = _repository.GetListQuery()
            .Where(x => mapped.Select(m => m.DateCreate).Contains(x.DateCreate))
            .ToList();
        
        if (created.Count == models.Count && created.All(p => p.Id > 0))
            return Ok(_mapper.Map<List<TResponse>>(created));

        return StatusCode(500, "Произошла ошибка при добавлении записей");
    }

    /// <summary>
    /// Удаление записи
    /// </summary>
    /// <param name="id">Идентификатор записи</param>
    /// <response code="500">При удалении записи произошла ошибка</response>   
    /// <returns></returns>
    [HttpDelete("{id}")]
    [SwaggerResponse(200, "Запись успешно удалена")]
    [SwaggerResponse(404, "Запись не найдена")]
    [SwaggerResponse(500, "Произошла ошибка при удаленнии записи")]
    public virtual IActionResult RemoveById(int id)
    {
        var entity = _repository.Get(id);
        if (entity == null) return NotFound();

        _repository.Remove(id);
        _repository.SaveChanges();
        return Ok();
    }

    /// <summary>
    /// Удаление записей
    /// </summary>
    /// <param name="ids">Идентификаторы записей</param>
    /// <response code="500">При удалении записей произошла ошибка</response>   
    /// <returns></returns>
    [HttpDelete("range")]
    [SwaggerResponse(200, "Записи успешно удалены")]
    [SwaggerResponse(400, "Не указаны Id записей, которые необходимо удалить")]
    [SwaggerResponse(500, "Произошла ошибка при удалении записей")]
    public virtual IActionResult RemoveRangeById(List<int> ids)
    {
        if (ids.Count == 0) return BadRequest();

        _repository.RemoveRange(ids);
        _repository.SaveChanges();
        return Ok();
    }

    /// <summary>
    /// Обновление записи
    /// </summary>
    /// <param name="model">Обновленная запись</param>
    /// <response code="400">Запись не найдена</response>   
    /// <response code="500">При удалении записи произошла ошибка</response>   
    /// <returns></returns>
    [HttpPut]
    [SwaggerResponse(200, "Запись успешно обновлена. Содержит информацию об обновленной записи", typeof(BaseEntity))]
    [SwaggerResponse(404, "Запись не найдена")]
    [SwaggerResponse(500, "При обновлении записи произошла ошибка")]
    public virtual IActionResult Update(T model)
    {
        var fromDb = _repository.Get(model.Id);
        if (fromDb == null)
            return NotFound("Запись с заданным идентификатором не найдена");

        var res = model.UpdateEntity(_mapper, fromDb);

        try
        {
            _repository.Update(res);
            _repository.SaveChanges();
            return Ok(res);
        }
        catch (Exception e)
        {
            _logger.LogError($"Error while updating an entity: {e}");
            return StatusCode(500, "Ошибка при обновлении записи");
        }
    }

    /// <summary>
    /// Обновление записей
    /// </summary>
    /// <param name="models">Обновленные записи</param>
    /// <response code="400">Записи не найдены</response>   
    /// <response code="500">При удалении записей произошла ошибка</response>   
    /// <returns></returns>
    [HttpPut("range")]
    [SwaggerResponse(200, "Записи успешно обновлены. Содержит информацию об обновленных записях",
        typeof(List<BaseEntity>))]
    [SwaggerResponse(400, "Нет записей для обновления")]
    [SwaggerResponse(500, "При обновлении записей произошла ошибка")]
    public virtual IActionResult UpdateRange(List<T> models)
    {
        if (models.Count == 0) return BadRequest();

        var fromDb = _repository.GetListQuery()
            .Where(p => models.Select(x => x.Id).Contains(p.Id))
            .ToList();

        var notFoundGuids = models.Select(p => p.Id)
            .Except(fromDb.Select(p => p.Id))
            .ToList();

        if (notFoundGuids.Count != 0) return BadRequest(notFoundGuids);

        models = fromDb.Join(models,
                d => d.Id,
                m => m.Id,
                (d, m) => m.UpdateEntity(_mapper, d))
            .ToList();

        _repository.UpdateRange(models);
        _repository.SaveChanges();
        return Ok(models);
    }
}