
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Nop.Core;
using Nop.Plugin.Misc.CodeInjector.Models;
using Nop.Web.Framework.Infrastructure;

namespace Nop.Plugin.Misc.CodeInjector.Services
{
    public interface ICodeInjectorService
    {
        Task<string[]> GetCodeToInject(string zone);

        Task<string[]> GetActiveZones();

        Task<IPagedList<CodeToInjectDTO>> GetAllAsync(CodeToInjectSearchModel searchModel);

        Task DeleteAsync(CodeToInjectDTO ent);

        Task InsertAsync(CodeToInjectDTO ent);

        
    }
}