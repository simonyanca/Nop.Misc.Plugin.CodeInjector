

using Nop.Core;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using System.Text.Json;
using System.Text.RegularExpressions;
using RazorEngine;
using RazorEngine.Templating; 

namespace Nop.Plugin.Misc.CodeInjector.Services
{
    public class CIParseService 
    {
        private readonly ICodeInjectorService _service;
        private readonly ILocalizationService _localizationService;
        private readonly IStoreContext _storeContext;
        private readonly ISettingService _settingService;

        public CIParseService(ICodeInjectorService service, ILocalizationService localizationService, IStoreContext storeContext, ISettingService settingService)
            
        {
            _service = service;
            _localizationService = localizationService;
            _settingService = settingService;
            _storeContext = storeContext;
        }

        //string widgetZone, object additionalData)

        private static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }

        public async Task<string> GetHtml(string widgetZone, object additionalData)
        {
            //load settings for active store scope
            var storeId = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var settings = await _settingService.LoadSettingAsync<CodeInjectorSettings>(storeId);
            string jsonString = JsonSerializer.Serialize(additionalData);

            string html = $"<strong>Zone name: {widgetZone}</strong><br>{jsonString}";
            if (settings.Debug)
                return html;

            
            string template = await _service.GetCodeToInject(widgetZone);
            string key = template;
            if (template.IsNullOrEmpty())
                return template;

            if (!Engine.Razor.IsTemplateCached(key, null))
                html = Engine.Razor.RunCompile(template, key, null, additionalData, null);
            else
                html = Engine.Razor.Run(key, null, additionalData, null);
            
            

            return html;
        }
    }
}
