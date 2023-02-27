
using System.Threading.Tasks;
using Nop.Core.Caching;
using Nop.Core.Events;
using Nop.Plugin.Misc.CodeInjector.Services;
using Nop.Services.Caching;
using Nop.Services.Configuration;
using Nop.Services.Events;

namespace Nop.Plugin.Misc.CodeInjector.Infrastructure.Cache
{


    /// <summary>
    /// Model cache event consumer (used for caching of presentation layer models)
    /// </summary>
    public class ModelCacheEventConsumer :
        //tax rates
        IConsumer<EntityInsertedEvent<CodeToInject>>,
        IConsumer<EntityUpdatedEvent<CodeToInject>>,
        IConsumer<EntityDeletedEvent<CodeToInject>>
    {
        #region Fields

        private readonly IStaticCacheManager _staticCacheManager;
        private readonly ISettingService _settingService;

		#endregion

		#region Ctor

		public ModelCacheEventConsumer(IStaticCacheManager staticCacheManager, ISettingService settingService)
        {
            _staticCacheManager = staticCacheManager;
            _settingService = settingService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handle tax rate inserted event
        /// </summary>
        /// <param name="eventMessage">Event message</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task HandleEventAsync(EntityInsertedEvent<CodeToInject> eventMessage)
        {
            await ClearCache();
        }

        /// <summary>
        /// Handle tax rate updated event
        /// </summary>
        /// <param name="eventMessage">Event message</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task HandleEventAsync(EntityUpdatedEvent<CodeToInject> eventMessage)
        {
            await ClearCache();
        }

        /// <summary>
        /// Handle tax rate deleted event
        /// </summary>
        /// <param name="eventMessage">Event message</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task HandleEventAsync(EntityDeletedEvent<CodeToInject> eventMessage)
        {
            await ClearCache();
        }

        private async Task ClearCache()
        {
            await _staticCacheManager.RemoveAsync(CodeInjectorDefaults.WidgetZoneHtmlCacheKey);
		}



        #endregion
    }
}