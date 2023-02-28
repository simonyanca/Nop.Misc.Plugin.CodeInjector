

using Nop.Core;
using Nop.Data;
using System.Threading.Tasks;
using Nop.Plugin.Misc.CodeInjector.Models;
using System.Collections.Generic;
using System.Linq;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using RazorEngine;
using System;
using Nop.Core.Caching;
using Autofac.Core;
using Nop.Services.Configuration;
using Nop.Web.Framework.Infrastructure;

namespace Nop.Plugin.Misc.CodeInjector.Services
{
    public class CodeInjectorService : ICodeInjectorService
    {
        private readonly IRepository<CodeToInject> _codeToInjectRepository;
        private readonly IStoreContext _storeContext;
		private readonly IStaticCacheManager _staticCacheManager;
        private readonly ISettingService _settingService;

		public CodeInjectorService(IRepository<CodeToInject> codeToInjectRepository, IStoreContext storeContext, IStaticCacheManager staticCacheManager,ISettingService settingService)
        {
            _codeToInjectRepository = codeToInjectRepository;
            _storeContext = storeContext;
			_staticCacheManager = staticCacheManager;
			_settingService = settingService;
		}

		public async Task DeleteAsync(int id)
		{
			var ent = await _codeToInjectRepository.GetByIdAsync(id);
			await _codeToInjectRepository.DeleteAsync(ent);
		}

		public async Task DeleteAsync(CodeToInject item)
        {
            await DeleteAsync(item.Id);
		}

        private async Task<Dictionary<string, List<CodeToInject>>> GetZonesDictionary()
        {
			var store = await _storeContext.GetCurrentStoreAsync();
			CacheKey key = _staticCacheManager.PrepareKeyForDefaultCache(CodeInjectorDefaults.WidgetZoneHtmlCacheKey, store);
			Dictionary<string, List<CodeToInject>>  ret = await _staticCacheManager.GetAsync<Dictionary<string, List<CodeToInject>>>(key, () =>
            {
				var qry = _codeToInjectRepository.Table.Where(r => r.StoreId == store.Id);
				if (!qry.Any())
					return new Dictionary<string, List<CodeToInject>>();

				Dictionary<string, List<CodeToInject>> dic = new Dictionary<string, List<CodeToInject>>();
				foreach (var i in qry)
				{
					if (!dic.ContainsKey(i.Zone))
						dic[i.Zone] = new List<CodeToInject>();
					dic[i.Zone].Add(i);
					dic[i.Zone] = dic[i.Zone].OrderBy(r => r.Order).ToList();
				}

				return dic;
			});

            return ret;
        }

		public async Task<string[]> GetActiveZones()
        {
            //load settings for active store scope
            var store = await _storeContext.GetCurrentStoreAsync();
            var settings = await _settingService.LoadSettingAsync<CodeInjectorSettings>(store.Id);

            if (settings.ViewAllZones)
                return typeof(PublicWidgetZones).GetProperties().Select(r => r.GetValue(r.Name).ToString()).ToArray();

			var data = await GetZonesDictionary();
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

            if (colName == nameof(CodeToInjectDTO.Zone))
                qry = searchModel.Order[0].Dir == "asc" ? qry.OrderBy(r => r.Zone).ThenBy(r => r.Order) : qry.OrderByDescending(r => r.Zone).ThenBy(r => r.Order);
            else if (colName == nameof(CodeToInjectDTO.Name))
                qry = searchModel.Order[0].Dir == "asc" ? qry.OrderBy(r => r.Name).ThenBy(r => r.Order) : qry.OrderByDescending(r => r.Name).ThenBy(r => r.Order);
            else
                qry = qry.OrderBy(r => r.Zone).ThenBy(r => r.Order);

			var records = await qry.ToListAsync();
            var paged = new PagedList<CodeToInject>(records, searchModel.Page - 1, searchModel.PageSize);

            return paged;
        }

        public async Task<CodeToInjectDTO[]> GetCodeToInject(string zone)
        {
            var data = await GetZonesDictionary();
            List<CodeToInject> items = new List<CodeToInject>();
            data.TryGetValue(zone, out items);
            if (items == null) return new CodeToInjectDTO[0];
			return items.Select(r => r.ToModel<CodeToInjectDTO>()).ToArray();
        }

		public async Task<CodeToInjectDTO> GetById(int id)
		{
			var store = await _storeContext.GetCurrentStoreAsync();
			var item = await _codeToInjectRepository.Table.Where(r => r.StoreId == store.Id && r.Id == id).FirstOrDefaultAsync();
            if (item == null)
                return null;
			return item.ToModel<CodeToInjectDTO>();
		}

		public async Task InsertAsync(CodeToInject ent)
        {
            var store = await _storeContext.GetCurrentStoreAsync();
            ent.StoreId = store.Id;
			ent.CacheKey = DateTime.Now.Ticks;
			await _codeToInjectRepository.InsertAsync(ent);
        }

        public async Task Update(CodeToInject ent)
        {
			var store = await _storeContext.GetCurrentStoreAsync();
			ent.StoreId = store.Id;
            ent.CacheKey = DateTime.Now.Ticks;
			await _codeToInjectRepository.UpdateAsync(ent);
        }
    }
}
