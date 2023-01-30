using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Nop.Core;
using Nop.Plugin.Misc.CodeInjector.Models;
using Nop.Plugin.Misc.CodeInjector.Services;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Models.Extensions;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.CodeInjector.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class CodeInjectorController : BasePluginController
    {

        #region Fields

        private readonly ICodeInjectorService _codeInjectorService;
        private readonly IStoreContext _storeContext;
        private readonly ISettingService _settingService;
        private readonly INotificationService _notificationService;
        private readonly ILocalizationService _localizationService;
        #endregion

        #region Ctor

        public CodeInjectorController(ICodeInjectorService codeInjectorService, ILocalizationService localizationService, IStoreContext storeContext, ISettingService settingService, INotificationService notificationService)
        {
            _codeInjectorService = codeInjectorService;
            _storeContext = storeContext;
            _settingService = settingService;
            _notificationService = notificationService;
            _localizationService = localizationService;
        }

        #endregion


        #region Methods

        private async Task PrepareModelAsync(ConfigurationModel model)
        {
            //load settings for active store scope
            var storeId = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var settings = await _settingService.LoadSettingAsync<CodeInjectorSettings>(storeId);

            model.Settings = settings;
        }

        [AuthorizeAdmin]
        [Area(AreaNames.Admin)]
        [HttpPost, ActionName("Get")]
        public async Task<IActionResult> Get(CodeToInjectSearchModel searchModel)
        {
            
            var data = (await _codeInjectorService.GetAllAsync(searchModel));

            var model = new CodeToInjectListModel().PrepareToGrid(searchModel, data, () =>
            {
                return data.Select(r => r.ToModel<CodeToInjectDTO>());
            });

            return Json(model);
        }


        [AuthorizeAdmin]
        [Area(AreaNames.Admin)]
        [HttpPost, ActionName("Remove")]
        public async Task<IActionResult> Remove(CodeToInjectDTO model)
        {
            if (!ModelState.IsValid)
                return ErrorJson(ModelState.SerializeErrors());

            await _codeInjectorService.DeleteAsync(model.ToEntity<CodeToInject>());

            
            //string errors = "";
            //string errors = await _codeInjectorService.InsertAsync(model);
            //if (!errors.IsNullOrEmpty())
            //    return ErrorJson($"{errors}");

            return Json(new { Result = true });
        }

        [AuthorizeAdmin]
        [Area(AreaNames.Admin)]
        [HttpPost, ActionName("Add")]
        public async Task<IActionResult> Add(CodeToInjectDTO model)
        {
            if (!ModelState.IsValid)
                return ErrorJson(ModelState.SerializeErrors());

            await _codeInjectorService.InsertAsync(model.ToEntity<CodeToInject>());
            string errors = "";
            //string errors = await _codeInjectorService.InsertAsync(model);
            //if (!errors.IsNullOrEmpty())
            //    return ErrorJson($"{errors}");

            return Json(new { Result = errors.IsNullOrEmpty() });
        }

        [AuthorizeAdmin]
        [Area(AreaNames.Admin)]
        public async Task<IActionResult> Configure()
        {
            ConfigurationModel model = new ConfigurationModel();
            await PrepareModelAsync(model);
            return View("~/Plugins/Misc.CodeInjector/Views/Configure.cshtml", model); 
        }

        [AuthorizeAdmin]
        [Area(AreaNames.Admin)]
        [HttpPost, ActionName("Configure")]
        public async Task<IActionResult> Configure(CodeInjectorSettings model)
        {
            if (!ModelState.IsValid)
            {
                _notificationService.ErrorNotification("Error");
                return await Configure();
            }
                

            var storeId = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            await _settingService.SaveSettingAsync(model, storeId);
            await _settingService.ClearCacheAsync();

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));

            return await Configure();
        }


        #endregion
    }
}