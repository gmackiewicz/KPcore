using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KPcore.Data.Migrations
{
    public partial class CreationDatesCanBeNull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDate",
                table: "TopicEntries",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "NotificationDate",
                table: "Notifications",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDate",
                table: "GroupComments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDate",
                table: "TopicEntries",
                nullable: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "NotificationDate",
                table: "Notifications",
                nullable: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDate",
                table: "GroupComments",
                nullable: false);
        }
    }
}
