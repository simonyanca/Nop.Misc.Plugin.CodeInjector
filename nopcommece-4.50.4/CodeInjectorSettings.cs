using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using Nop.Core.Configuration;

namespace Nop.Plugin.Misc.CodeInjector
{
    /// <summary>
    /// Represents a plugin settings
    /// </summary>
    public class CodeInjectorSettings : ISettings
    {
        public bool Debug { get; set; }
    }
}