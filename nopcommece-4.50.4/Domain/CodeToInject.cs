
using System.ComponentModel.DataAnnotations;
using Nop.Core;
using Nop.Plugin.Misc.CodeInjector.Domain;

namespace Nop.Plugin.Misc.CodeInjector.Services
{
    public class CodeToInject: BaseEntity
    {
        [Required]
        public int StoreId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Zone { get; set; }

        [Required]
        public string Code { get; set; }

        public int Order { get; set; }

		[Required]
		public int RenderType { get; set; }

        public long CacheKey { get; set; }

	}


}
