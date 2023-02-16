using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.CodeInjector.Models
{
    public record CodeToInjectDTO: BaseNopEntityModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Zone { get; set; }

        [Required]
        public string Code { get; set; }

        public int Order { get; set; } = 0;
    }


}
