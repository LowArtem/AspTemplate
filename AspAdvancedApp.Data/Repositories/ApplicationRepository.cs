using AspAdvancedApp.Core.Model._Base;
using AspAdvancedApp.Core.Repositories;
using Microsoft.Extensions.Logging;

namespace AspAdvancedApp.Data.Repositories;

/// <summary>
/// Базовый репозиторий приложения
/// </summary>
/// <typeparam name="TEntity">модель</typeparam>
public class ApplicationRepository<TEntity> : EfCoreRepository<TEntity, ApplicationContext>
    where TEntity : class, IEntity
{
    private readonly ILogger<IEfCoreRepository<TEntity>> _logger;

    public ApplicationRepository(ApplicationContext context,
        ILogger<IEfCoreRepository<TEntity>> logger) : base(context, logger)
    {
    }
}