using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KPcore.Data.Migrations
{
    public partial class OnTopicDeleteSetNullInGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Topics_TopicId",
                table: "Groups");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Topics_TopicId",
                table: "Groups",
                column: "TopicId",
                principalTable: "Topics",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Topics_TopicId",
                table: "Groups");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Topics_TopicId",
                table: "Groups",
                column: "TopicId",
                principalTable: "Topics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
