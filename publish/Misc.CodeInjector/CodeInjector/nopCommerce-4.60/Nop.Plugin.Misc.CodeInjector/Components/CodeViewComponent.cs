

using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Nop.Web.Framework.Components;
using Nop.Plugin.Misc.CodeInjector.Services;
using System;
using Nop.Services.Logging;
using Nop.Core.Caching;

namespace Nop.Plugin.Misc.CodeInjector.Components
{
	[ViewComponent(Name = CodeInjectorDefaults.VIEW_COMPONENT_NAME)]
	/// <summary>
	/// Represents the view component to display logo
	/// </summary>
	public class CodeViewComponent : NopViewComponent
    {
        #region Fields
        private readonly CIParseService _parserService;
		
		private readonly ILogger _logger;

		#endregion

		#region Ctor

		public CodeViewComponent(CIParseService parserService ,ILogger logger)
        {
            _parserService = parserService;
            _logger = logger;
        }

        #endregion

        #region Methods

     
        public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
        {
			string html = string.Empty;
			try
            {
				html = await _parserService.GetHtml(widgetZone, additionalData);
			}catch(Exception ex)
            {
                await _logger.ErrorAsync($"Error CodeInjectionPlugin parsing {widgetZone}", ex);
				html = $"<div style=\"background-color:#f44336; color:white\" >Error zone {widgetZone}</div>";
			}
			return await Task.FromResult(new HtmlContentViewComponentResult(new HtmlString(html)));
		}

        #endregion
    }
}