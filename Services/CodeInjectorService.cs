
using Nop.Core;
using Nop.Data;
using Nop.Core.Caching;
using Nop.Plugin.Misc.CodeInjector.Services;
using System.Threading.Tasks;
using Nop.Plugin.Misc.CodeInjector.Models;
using System.Collections.Generic;
using System.Linq;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Microsoft.IdentityModel.Tokens;

namespace Nop.Plugin.Misc.CodeInjector.Services
{
    public class CodeInjectorService : ICodeInjectorService
    {
        private readonly IRepository<CodeToInject> _codeToInjectRepository;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly IStoreContext _storeContext;

        public CodeInjectorService( IRepository<CodeToInject> codeToInjectRepository, IStaticCacheManager staticCacheManager, IStoreContext storeContext)
        {
            _codeToInjectRepository = codeToInjectRepository;
            _staticCacheManager = staticCacheManager;
            _storeContext = storeContext;
        }

        public async Task DeleteAsync(CodeToInjectDTO item)
        {
            var ent = _codeToInjectRepository.GetById(item.Id);
            await _codeToInjectRepository.DeleteAsync(ent);
        }

        private async Task<Dictionary<string, CodeToInjectDTO[]>> GetZonesDictionary()
        {
            var store = await _storeContext.GetCurrentStoreAsync();
            var qry = _codeToInjectRepository.Table.Where(r => r.StoreId == store.Id);
            Dictionary<string, CodeToInjectDTO[]> dic = qry.GroupBy(r => r.Zone, r => r)
                .ToDictionary(r => r.Key, r => r.Select(rr => rr.ToModel<CodeToInjectDTO>())
                .ToArray());
            return dic;
        }

        public async Task<string[]> GetActiveZones()
        {
            var data = await GetZonesDictionary();
            return data.Keys.ToArray();
        }

        public async Task<IPagedList<CodeToInjectDTO>> GetAllAsync(CodeToInjectSearchModel searchModel)
        {
            var store = await _storeContext.GetCurrentStoreAsync();
            var qry = _codeToInjectRepository.Table.Where(r => r.StoreId == store.Id);

            if (!searchModel.Name.IsNullOrEmpty()) qry = qry.Where(r => r.Name.Contains(searchModel.Name));
            if (!searchModel.Zone .IsNullOrEmpty()) qry = qry.Where(r => r.Zone.Contains(searchModel.Zone));
            if (!searchModel.Code.IsNullOrEmpty()) qry = qry.Where(r => r.Code.Contains(searchModel.Code));

            string colName = searchModel.Columns[searchModel.Order[0].Column].Data;

            if (colName == nameof(CodeToInjectDTO.Name)) qry = searchModel.Order[0].Dir == "asc" ? qry.OrderBy(r => r.Name) : qry.OrderByDescending(r => r.Name);
            else if (colName == nameof(CodeToInjectDTO.Zone)) qry = searchModel.Order[0].Dir == "asc" ? qry.OrderBy(r => r.Zone) : qry.OrderByDescending(r => r.Zone);

            var records = await qry.Select(r => r.ToModel<CodeToInjectDTO>()).ToListAsync();
            var paged = new PagedList<CodeToInjectDTO>(records, searchModel.Page - 1, searchModel.PageSize);

            return paged;
        }

        public async Task<string[]> GetCodeToInject(string zone)
        {
            var data = await GetZonesDictionary();
            return data[zone].Select(r => r.Code).ToArray();
        }

        public async Task InsertAsync(CodeToInjectDTO dto)
        {
            var ent = dto.ToEntity<CodeToInject>();
            await _codeToInjectRepository.InsertAsync(ent);
        }
    }
}
