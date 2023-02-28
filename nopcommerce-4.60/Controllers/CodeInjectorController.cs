
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Misc.CodeInjector.Models;
using Nop.Plugin.Misc.CodeInjector.Services;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Plugins;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Models.Extensions;
using Nop.Web.Framework.Mvc.Filters;

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
		private readonly IPluginService _pluginService;
		private readonly IPermissionService _permissionService;
		#endregion

		#region Ctor

		public CodeInjectorController(ICodeInjectorService codeInjectorService, IPermissionService permissionService, IPluginService pluginService, ILocalizationService localizationService, IStoreContext storeContext, ISettingService settingService, INotificationService notificationService)
        {
            _codeInjectorService = codeInjectorService;
            _storeContext = storeContext;
            _settingService = settingService;
            _notificationService = notificationService;
            _localizationService = localizationService;
			_pluginService = pluginService;
			_permissionService = permissionService;
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
		[HttpGet, ActionName("SettingsForm")]
		public async Task<IActionResult> SettingsForm()
		{
			var storeId = await _storeContext.GetActiveStoreScopeConfigurationAsync();
			CodeInjectorSettings model = await _settingService.LoadSettingAsync<CodeInjectorSettings>(storeId);
			return View("~/Plugins/Misc.CodeInjector/Views/SettingsForm.cshtml", model);
		}

        [AuthorizeAdmin]
        [Area(AreaNames.Admin)]
        public IActionResult Configure()
        {
			CodeToInjectSearchModel model = new CodeToInjectSearchModel();
            return View("~/Plugins/Misc.CodeInjector/Views/Configure.cshtml", model); 
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
		public async Task<IActionResult> Remove(int id)
		{
			await _codeInjectorService.DeleteAsync(id);
			return Json(new { Result = true });
		}

		[AuthorizeAdmin]
		[Area(AreaNames.Admin)]
		[HttpGet, ActionName("Add")]
		public IActionResult Add()
		{
			CodeToInjectDTO model = new CodeToInjectDTO();
			return View(model);
		}

		private IActionResult View(CodeToInjectDTO model)
		{
			return View("~/Plugins/Misc.CodeInjector/Views/AddUpdateISC.cshtml", model);
		}

		[AuthorizeAdmin]
		[Area(AreaNames.Admin)]
		[HttpPost, ActionName("SettingsForm")]
		public async Task<IActionResult> SettingsForm(CodeInjectorSettings model)
		{
			if (!ModelState.IsValid)
			{
				_notificationService.ErrorNotification("Error");
				return await SettingsForm();
			}


			var storeId = await _storeContext.GetActiveStoreScopeConfigurationAsync();
			await _settingService.SaveSettingAsync(model, storeId);
			await _settingService.ClearCacheAsync();

			_notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));

			return await SettingsForm();
		}

		[AuthorizeAdmin]
		[Area(AreaNames.Admin)]
		[HttpPost, ActionName("Save")]
		public async Task<IActionResult> Save(CodeToInjectDTO model)
		{
			if (!ModelState.IsValid)
			{
				_notificationService.ErrorNotification("Error");
				return View("~/Plugins/Misc.CodeInjector/Views/AddUpdateISC.cshtml", model);
			}

			CodeToInject ent = model.ToEntity<CodeToInject>();
			if (ent.Id == 0)
				await _codeInjectorService.InsertAsync(ent);
			else
				await _codeInjectorService.Update(ent);

			//Force call GetActiveZones
			var storeId = await _storeContext.GetActiveStoreScopeConfigurationAsync();
			CodeInjectorSettings model2 = await _settingService.LoadSettingAsync<CodeInjectorSettings>(storeId);
			await _settingService.SaveSettingAsync(model2, storeId);
			await _settingService.ClearCacheAsync();
			

			_notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));

			return RedirectToAction("Configure");
		}

		[AuthorizeAdmin]
		[Area(AreaNames.Admin)]
		[HttpGet]
		public async Task<IActionResult> Update(int id)
		{
			CodeToInjectDTO model = await _codeInjectorService.GetById(id);
			return View(model);
		}

		#endregion
	}
}