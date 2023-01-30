
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.CodeInjector.Models
{
    /// <summary>
    /// Represents a tax transaction log list model
    /// </summary>
    public record CodeToInjectListModel : BasePagedListModel<CodeToInjectDTO>    
    {
    }
}