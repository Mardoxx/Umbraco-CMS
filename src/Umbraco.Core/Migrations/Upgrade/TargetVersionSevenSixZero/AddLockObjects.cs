﻿using Umbraco.Core.Models.Rdbms;

namespace Umbraco.Core.Migrations.Upgrade.TargetVersionSevenSixZero
{
    [Migration("7.6.0", 101, Constants.System.UmbracoMigrationName)]
    public class AddLockObjects : MigrationBase
    {
        public AddLockObjects(IMigrationContext context)
            : base(context)
        { }

        public override void Up()
        {
            EnsureLockObject(Constants.Locks.Servers, "Servers");
        }

        public override void Down()
        {
            // not implemented
        }

        private void EnsureLockObject(int id, string name)
        {
            Execute.Code(context =>
            {
                var db = context.Database;
                var exists = db.Exists<LockDto>(id);
                if (exists) return string.Empty;
                // be safe: delete old umbracoNode lock objects if any
                db.Execute("DELETE FROM umbracoNode WHERE id=@id;", new { id });
                // then create umbracoLock object
                db.Execute("INSERT umbracoLock (id, name, value) VALUES (@id, @name, 1);", new { id, name });
                return string.Empty;
            });
        }
    }
}