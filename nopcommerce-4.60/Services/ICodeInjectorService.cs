

using System.Threading.Tasks;
using Nop.Core;
using Nop.Plugin.Misc.CodeInjector.Models;

namespace Nop.Plugin.Misc.CodeInjector.Services
{
    public interface ICodeInjectorService
    {
        Task<string> GetCodeToInject(string zone);

        Task<string[]> GetActiveZones();

        Task<IPagedList<CodeToInject>> GetAllAsync(CodeToInjectSearchModel searchModel);

        Task DeleteAsync(CodeToInject ent);

        Task InsertAsync(CodeToInject ent);


    }
}