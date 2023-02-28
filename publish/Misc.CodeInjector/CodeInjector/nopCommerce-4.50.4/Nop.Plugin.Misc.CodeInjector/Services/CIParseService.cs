

using Nop.Core;
using System.Threading.Tasks;
using Nop.Services.Configuration;
using System.Text.Json;
using RazorEngine;
using RazorEngine.Templating;
using Nop.Services.Logging;
using System;
using Nop.Plugin.Misc.CodeInjector.Models;
using LinqToDB.Common;
using Nop.Core.Caching;
using Nop.Core.Domain.Stores;
using System.Collections.Generic;

namespace Nop.Plugin.Misc.CodeInjector.Services
{
    public class CIParseService 
    {
        private readonly ICodeInjectorService _service;
        private readonly IStoreContext _storeContext;
        private readonly ISettingService _settingService;
		

		public CIParseService(ICodeInjectorService service, IStoreContext storeContext, ISettingService settingService)
        {
            _service = service;
            _settingService = settingService;
            _storeContext = storeContext;
		}

  

        public async Task<string> GetHtml(string widgetZone, object additionalData)
        {
			//load settings for active store scope
			var store = await _storeContext.GetCurrentStoreAsync();
			var settings = await _settingService.LoadSettingAsync<CodeInjectorSettings>(store.Id);
			string html = string.Empty;

			if(settings.ViewAllZones || settings.Debug)
			{
				string jsonString = string.Empty;
				if (settings.Debug)
				{
					jsonString = JsonSerializer.Serialize(additionalData);
					jsonString = Newtonsoft.Json.Linq.JToken.Parse(jsonString)
									.ToString(Newtonsoft.Json.Formatting.Indented)
									.Replace("\r\n", "<br>");
				}

				html =	$"<div style=\"background-color:#02A9FC; color:white; border:1px solid black\" >" +
							$"<strong>Zone name:</strong> {widgetZone}<br>" +
							(additionalData == null? "" : $"<div><strong>Parameters:</strong><br>{jsonString}</div>") +
						$"</div>";
				return html;
			}
			
			CodeToInjectDTO[] template =  await _service.GetCodeToInject(widgetZone);
				 
			foreach (var i in template)
			{
				if (i.RenderType == Domain.RenderTypeEnum.RAW)
					html += i.Code;
				else
				{
					string keyt = $"CodeInject-{i.Id}-{i.CacheKey}";
					if (!Engine.Razor.IsTemplateCached(keyt, null))
						html += Engine.Razor.RunCompile(i.Code, keyt, null, additionalData, null);
					else
						html += Engine.Razor.Run(keyt, null, additionalData, null);
				}
			}

			return html;
		}
    }
}
