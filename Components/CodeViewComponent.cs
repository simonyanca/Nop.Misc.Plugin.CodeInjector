

using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Nop.Web.Framework.Components;
using Nop.Plugin.Misc.CodeInjector.Services;
using Nop.Services.Localization;
using Nop.Core;
using Nop.Services.Configuration;
using System.Text.Json;

namespace Nop.Plugin.Misc.CodeInjector.Components
{
    /// <summary>
    /// Represents the view component to display logo
    /// </summary>
    public class CodeViewComponent : NopViewComponent
    {
        #region Fields
        private readonly CIParseService _parserService;


        #endregion

        #region Ctor

        public CodeViewComponent(CIParseService parserService)
        {
            _parserService = parserService;
        }

        #endregion

        #region Methods

     

        /// <summary>
        /// Invoke view component
        /// </summary>
        /// <param name="widgetZone">Widget zone name</param>
        /// <param name="additionalData">Additional data</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the view component result
        /// </returns>
        public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
        {
            string html =  await _parserService.GetHtml(widgetZone, additionalData);
            return await Task.FromResult(new HtmlContentViewComponentResult(new HtmlString(html)));
        }

        #endregion
    }
}