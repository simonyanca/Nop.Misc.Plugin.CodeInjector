using Nop.Core;
using Nop.Core.Caching;

namespace Nop.Plugin.Misc.CodeInjector
{
    /// <summary>
    /// Represents plugin constants
    /// </summary>
    public static class CodeInjectorDefaults
    {
        public static string SystemName => "Misc.CodeInjector";

		public const string VIEW_COMPONENT_NAME = "CodeView";

		public static CacheKey WidgetZoneHtmlCacheKey => new("Nop.Plugin.Misc.CodeInjector.WidgetZoneHtmlKey");

	}
}