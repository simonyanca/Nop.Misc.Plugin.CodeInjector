using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core;
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

		[Required]
        public string Code { get; set; }

        public int Order { get; set; } = 0;
    }


}
