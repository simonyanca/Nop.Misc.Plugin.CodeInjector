using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Misc.CodeInjector.Models;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Misc.CodeInjector.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class CodeInjectorController : BasePluginController
    {

        #region Fields
        


        #endregion

        #region Ctor

        public CodeInjectorController()
        {
            
        }

        #endregion


        #region Methods


        [AuthorizeAdmin]
        [Area(AreaNames.Admin)]
        public IActionResult Configure()
        {
            ConfigurationModel model = new ConfigurationModel()
            {
                
            };

            
            return View("~/Plugins/Misc.CodeInjector/Views/Configure.cshtml", model); 
        }

        [AuthorizeAdmin]
        [Area(AreaNames.Admin)]
        [HttpPost, ActionName("Configure")]
        [FormValueRequired("save")]
        public IActionResult Configure(ConfigurationModel model)
        {
            if (!ModelState.IsValid)
                return  Configure();

            return Configure();
        }

        #endregion
    }
}