using FluentMigrator;

namespace CSD.Migrations.Year2023
{
    [Migration(202309151459)]
    public class Migration_202309151459 : Migration
    {
        public override void Up()
        {
            Create.Table("scenes")
                .WithColumn("id").AsInt64().NotNullable().PrimaryKey().Identity()
                .WithColumn("create_date").AsDateTimeOffset().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
                .WithColumn("update_date").AsDateTimeOffset().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
                .WithColumn("name").AsString(256).NotNullable().Unique()
                .WithColumn("filename").AsString(256).NotNullable();

            Create.Table("users")
                .WithColumn("id").AsInt64().NotNullable().PrimaryKey().Identity()
                .WithColumn("create_date").AsDateTimeOffset().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
                .WithColumn("update_date").AsDateTimeOffset().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
                .WithColumn("first_name").AsString(128).NotNullable()
                .WithColumn("last_name").AsString(128).NotNullable()
                .WithColumn("paternal_name").AsString(128).Nullable()
                .WithColumn("scene_id").AsInt64().Nullable().ForeignKey("scenes", "id")
                .WithColumn("role").AsInt16().NotNullable().WithDefaultValue(0)
                .WithColumn("login").AsString(128).NotNullable().Unique()
                .WithColumn("password").AsString(256).NotNullable()
                .WithColumn("password_salt").AsString(256).NotNullable();

            Create.Table("comments")
                .WithColumn("id").AsInt64().NotNullable().PrimaryKey().Identity()
                .WithColumn("create_date").AsDateTimeOffset().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
                .WithColumn("update_date").AsDateTimeOffset().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
                .WithColumn("user_id").AsInt64().NotNullable().ForeignKey("users", "id")
                .WithColumn("scene_id").AsInt64().NotNullable().ForeignKey("scenes", "id")
                .WithColumn("audio_filename").AsString(256).NotNullable()
                .WithColumn("photo_filename").AsString(256).NotNullable()
                .WithColumn("text_filename").AsString(256).NotNullable();
        }

        public override void Down()
        {
            Delete.Table("comments");
            Delete.Table("users");
            Delete.Table("scenes");
        }
    }
}
