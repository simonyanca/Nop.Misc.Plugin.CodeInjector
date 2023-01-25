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
using Nop.Web.Framework.Infrastructure;

namespace Nop.Plugin.Misc.CodeInjector
{   
    public class CodeInjector : BasePlugin, IMiscPlugin, IWidgetPlugin
    {
        private readonly IWebHelper _webHelper;
        private readonly WidgetSettings _widgetSettings;
        private readonly ISettingService _settingService;
        private readonly ICodeInjectorService _codeInjectorService;

        public bool HideInWidgetList => true;

        public CodeInjector(  IWebHelper webHelper, WidgetSettings widgetSettings, ISettingService settingService, ICodeInjectorService codeInjectorService)
        {
            _webHelper = webHelper;
            _widgetSettings = widgetSettings;
            _settingService = settingService;
            _codeInjectorService = codeInjectorService;
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
            //locales
            //await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
            //{
            //    ["Plugins.Misc.CodeInjector.Fields.PhoneNumber"] = "Phone number"
            //});

            if (!_widgetSettings.ActiveWidgetSystemNames.Contains(CodeInjectorDefaults.SystemName))
            {
                _widgetSettings.ActiveWidgetSystemNames.Add(CodeInjectorDefaults.SystemName);
                await _settingService.SaveSettingAsync(_widgetSettings);
            }

            await base.InstallAsync();
        }

        public override async Task UninstallAsync()
        {
            await base.UninstallAsync();
        }
    }
}