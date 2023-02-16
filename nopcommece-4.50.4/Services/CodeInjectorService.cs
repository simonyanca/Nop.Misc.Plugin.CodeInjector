

using Nop.Core;
using Nop.Data;
using System.Threading.Tasks;
using Nop.Plugin.Misc.CodeInjector.Models;
using System.Collections.Generic;
using System.Linq;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Microsoft.IdentityModel.Tokens;
using System;
using Nop.Web.Framework.Infrastructure;
using Nop.Plugin.Misc.CodeInjector.Extensions;
using System.Reflection;

namespace Nop.Plugin.Misc.CodeInjector.Services
{
    public class CodeInjectorService : ICodeInjectorService
    {
        private readonly IRepository<CodeToInject> _codeToInjectRepository;
        private readonly IStoreContext _storeContext;

        public CodeInjectorService(IRepository<CodeToInject> codeToInjectRepository, IStoreContext storeContext)
        {
            _codeToInjectRepository = codeToInjectRepository;
            _storeContext = storeContext;
        }

        public async Task DeleteAsync(CodeToInject item)
        {
            var ent = await _codeToInjectRepository.GetByIdAsync(item.Id);
            await _codeToInjectRepository.DeleteAsync(ent);
        }

        private async Task<Dictionary<string, List<CodeToInject>>> GetZonesDictionary()
        {

            var store = await _storeContext.GetCurrentStoreAsync();
            var qry = _codeToInjectRepository.Table.Where(r => r.StoreId == store.Id);
            if (!qry.Any())
                return new Dictionary<string, List<CodeToInject>>();

            Dictionary<string, List<CodeToInject>> dic = new Dictionary<string, List<CodeToInject>>();
            foreach(var i in qry)
            {
                if (!dic.ContainsKey(i.Zone))
                    dic[i.Zone] = new List<CodeToInject>();
                dic[i.Zone].Add(i);
                dic[i.Zone] = dic[i.Zone].OrderBy(r => r.Order).ToList();
            }
            
            return dic;
        }

      

        public async Task<string[]> GetActiveZones()
        {
            var data = await GetZonesDictionary();
            ////PublicWidgetZones
            return data.Keys.ToArray();
        }

        public async Task<IPagedList<CodeToInject>> GetAllAsync(CodeToInjectSearchModel searchModel)
        {
            var store = await _storeContext.GetCurrentStoreAsync();
            var qry = _codeToInjectRepository.Table.Where(r => r.StoreId == store.Id);

            if (!string.IsNullOrEmpty(searchModel.Name))
                qry = qry.Where(r => r.Name.Contains(searchModel.Name));
            if (!string.IsNullOrEmpty(searchModel.Zone))
                qry = qry.Where(r => r.Zone.Contains(searchModel.Zone));
            if (!string.IsNullOrEmpty(searchModel.Code))
                qry = qry.Where(r => r.Code.Contains(searchModel.Code));

            string colName = searchModel.Columns[searchModel.Order[0].Column].Data;

            if (colName == nameof(CodeToInjectDTO.Name))
                qry = searchModel.Order[0].Dir == "asc" ? qry.OrderBy(r => r.Name) : qry.OrderByDescending(r => r.Name);
            else if (colName == nameof(CodeToInjectDTO.Zone))
                qry = searchModel.Order[0].Dir == "asc" ? qry.OrderBy(r => r.Zone) : qry.OrderByDescending(r => r.Zone);

            var records = await qry.ToListAsync();
            var paged = new PagedList<CodeToInject>(records, searchModel.Page - 1, searchModel.PageSize);

            return paged;
        }

        public async Task<string> GetCodeToInject(string zone)
        {
            var data = await GetZonesDictionary();
            return string.Join("", data[zone].Select(r => r.Code).ToArray());
        }

        public async Task InsertAsync(CodeToInject ent)
        {
            var store = await _storeContext.GetCurrentStoreAsync();
            ent.StoreId = store.Id;
            await _codeToInjectRepository.InsertAsync(ent);
        }
    }
}
