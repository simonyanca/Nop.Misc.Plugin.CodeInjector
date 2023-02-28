
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Plugin.Misc.CodeInjector.Extensions;
using Nop.Web.Framework.Infrastructure;
using Nop.Web.Framework.Models;


namespace Nop.Plugin.Misc.CodeInjector.Models
{
    /// <summary>
    /// Represents plugin configuration model
    /// </summary>
    public record ConfigurationModel : BaseNopModel
    {
        public ConfigurationModel()
        {
            AddModel = new CodeToInjectDTO();
            SearchModel = new CodeToInjectSearchModel();
            Settings = new CodeInjectorSettings();
        }

      

        public CodeToInjectDTO AddModel { get; set; }

        public CodeToInjectSearchModel SearchModel { get; set; }

        public CodeInjectorSettings Settings { get; set; }
    }
}