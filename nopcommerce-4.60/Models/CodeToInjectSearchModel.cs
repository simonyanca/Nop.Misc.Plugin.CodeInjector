
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.CodeInjector.Models
{
    public record CodeToInjectSearchModel : BaseSearchModel
    {
        public string Name { get; set; }
        public string Zone { get; set; }
        public string Code { get; set; }

        public ColumnOptions[] Columns { get; set; }
        public Order[] Order { get; set; }
    }

    public class ColumnOptions
    {
        public string Data { get; set; }
        public string Name { get; set; }

        public bool Searchable { get; set; }

        public bool Orderable { get; set; }

    }

    public class Order
    {
        public int Column { get; set; }
        public string Dir { get; set; }
    }

}
