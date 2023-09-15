﻿using FluentMigrator;

namespace CSD.Migrations.Year2023
{
    [Migration(202309151459)]
    public class Migration_202309151459 : Migration
    {
        public override void Up()
        {
            Create.Table("users")
                .WithColumn("id").AsInt64().NotNullable().PrimaryKey().Identity()
                .WithColumn("create_date").AsDateTimeOffset().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
                .WithColumn("update_date").AsDateTimeOffset().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
                .WithColumn("first_name").AsString(128).NotNullable()
                .WithColumn("last_name").AsString(128).NotNullable()
                .WithColumn("paternal_name").AsString(128).Nullable()
                .WithColumn("role").AsInt16().NotNullable().WithDefaultValue(0)
                .WithColumn("login").AsString(128).NotNullable().Unique()
                .WithColumn("password").AsString(256).NotNullable()
                .WithColumn("password_salt").AsString(256).NotNullable();

            Create.Table("scenes")
                .WithColumn("id").AsInt64().NotNullable().PrimaryKey().Identity()
                .WithColumn("create_date").AsDateTimeOffset().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
                .WithColumn("update_date").AsDateTimeOffset().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
                .WithColumn("name").AsString(256).NotNullable().Unique()
                .WithColumn("path").AsString(512).NotNullable();

            Create.Table("user_scenes")
                .WithColumn("id").AsInt64().NotNullable().PrimaryKey().Identity()
                .WithColumn("create_date").AsDateTimeOffset().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
                .WithColumn("update_date").AsDateTimeOffset().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
                .WithColumn("user_id").AsInt64().NotNullable().ForeignKey("users", "id")
                .WithColumn("scene_id").AsInt64().NotNullable().ForeignKey("scenes", "id");

            Create.Table("comments")
                .WithColumn("id").AsInt64().NotNullable().PrimaryKey().Identity()
                .WithColumn("create_date").AsDateTimeOffset().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
                .WithColumn("update_date").AsDateTimeOffset().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
                .WithColumn("user_id").AsInt64().NotNullable().ForeignKey("users", "id")
                .WithColumn("scene_id").AsInt64().NotNullable().ForeignKey("scenes", "id")
                .WithColumn("path_to_audio").AsString(512).NotNullable()
                .WithColumn("path_to_photo").AsString(512).NotNullable()
                .WithColumn("path_to_text").AsString(512).NotNullable();
        }

        public override void Down()
        {
            Delete.Table("users");
            Delete.Table("scenes");
            Delete.Table("comments");
            Delete.Table("user_scenes");
        }
    }
}
