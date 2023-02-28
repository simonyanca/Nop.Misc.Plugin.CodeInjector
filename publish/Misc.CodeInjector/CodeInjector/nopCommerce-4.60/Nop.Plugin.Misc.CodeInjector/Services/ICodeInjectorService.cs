

using System.Threading.Tasks;
using Nop.Core;
using Nop.Plugin.Misc.CodeInjector.Models;

namespace Nop.Plugin.Misc.CodeInjector.Services
{
    public interface ICodeInjectorService
    {
        Task<CodeToInjectDTO[]> GetCodeToInject(string zone);

        Task<string[]> GetActiveZones();

        Task<IPagedList<CodeToInject>> GetAllAsync(CodeToInjectSearchModel searchModel);

        Task<CodeToInjectDTO> GetById(int id);

        Task DeleteAsync(CodeToInject ent);
		Task DeleteAsync(int id);

		Task InsertAsync(CodeToInject ent);

        Task Update(CodeToInject ent);
	}
}