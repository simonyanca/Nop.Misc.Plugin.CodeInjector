
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Domain.Cms;
using Nop.Plugin.Misc.CodeInjector.Components;
using Nop.Plugin.Misc.CodeInjector.Services;
using Nop.Services.Cms;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Plugins;

namespace Nop.Plugin.Misc.CodeInjector
{   
    public class CodeInjector : BasePlugin, IMiscPlugin, IWidgetPlugin
    { 
        private readonly IWebHelper _webHelper;
        private readonly WidgetSettings _widgetSettings;
        private readonly ISettingService _settingService;
        private readonly ICodeInjectorService _codeInjectorService;
		private readonly ILocalizationService _localizationService;

		public bool HideInWidgetList => true;

        public CodeInjector(  IWebHelper webHelper, WidgetSettings widgetSettings, ISettingService settingService, ICodeInjectorService codeInjectorService, ILocalizationService localizationService)
        {
            _webHelper = webHelper;
            _widgetSettings = widgetSettings;
            _settingService = settingService;
            _codeInjectorService = codeInjectorService;
            _localizationService = localizationService;

			
		}

        public Type GetWidgetViewComponent(string widgetZone)
        {
            if (widgetZone == null)
                throw new ArgumentNullException(nameof(widgetZone));

            string[] zones = _codeInjectorService.GetActiveZones().Result;
            if (zones.Contains(widgetZone))
                return typeof(CodeViewComponent);

            return null;
        }

        public async Task<IList<string>> GetWidgetZonesAsync()
        {
            string[] zones = await _codeInjectorService.GetActiveZones();
            return zones.ToList();
        }

        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/CodeInjector/Configure";
        }

        public override async Task InstallAsync()
        {
            if (!_widgetSettings.ActiveWidgetSystemNames.Contains(CodeInjectorDefaults.SystemName))
            {
                _widgetSettings.ActiveWidgetSystemNames.Add(CodeInjectorDefaults.SystemName);
                await _settingService.SaveSettingAsync(_widgetSettings);
            }

            await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
            {
                ["Nop.Plugin.Misc.CodeInjector.EditSettings"] = "Edit Settings",
            });

			await base.InstallAsync();
        }

        public override async Task UninstallAsync()
        {
            await base.UninstallAsync();
        }

		public string GetWidgetViewComponentName(string widgetZone)
		{
            return CodeInjectorDefaults.VIEW_COMPONENT_NAME;
		}
	}
}