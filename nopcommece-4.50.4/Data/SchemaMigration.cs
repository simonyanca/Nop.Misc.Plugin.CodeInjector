using FluentMigrator;
using Nop.Data.Extensions;
using Nop.Data.Migrations;
using Nop.Plugin.Misc.CodeInjector.Services;

namespace Nop.Plugin.Misc.CodeInjector.Data
{
    [NopMigration("2026/01/10 09:09:17", "Misc.CodeInjector base schema", MigrationProcessType.Installation)]
    public class SchemaMigration : AutoReversingMigration
    {
        #region Methods

        /// <summary>
        /// Collect the UP migration expressions
        /// </summary>
        public override void Up()
        {
            Create.TableFor<CodeToInject>();
        }

        #endregion
    }
}