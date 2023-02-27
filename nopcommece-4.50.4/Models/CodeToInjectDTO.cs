using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core;
using Nop.Plugin.Misc.CodeInjector.Domain;
using Nop.Web.Framework.Infrastructure;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.CodeInjector.Models
{
    public record CodeToInjectDTO: BaseNopEntityModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Zone { get; set; }

		public IList<SelectListItem> Zones = typeof(PublicWidgetZones).GetProperties().Select(r => new SelectListItem()
		{
			Text = r.Name,
			Value = r.GetValue(r.Name).ToString()
		}).ToArray();

		public IList<SelectListItem> RenderTypes = Enum.GetNames(typeof(RenderTypeEnum)).Select(r => new SelectListItem()
		{
			Text = r,
			Value = ((int)Enum.Parse<RenderTypeEnum>(r)).ToString()
		}).ToArray();

		[Required]
        public string Code { get; set; }

		public string CodeShort 
		{ 
			get
			{
				return string.Join("", Code.Take(30)) + "...";
			}
		}

		public int Order { get; set; } = 0;

		public long CacheKey { get; set; }

		public RenderTypeEnum RenderType { get; set; }

		public string RenderTypeName { get
			{
				return RenderType.ToString();
			} 
		}
	}


}
