using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace KPcore.Data.Migrations
{
    public partial class AddGroupCommentModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GroupComments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AuthorId = table.Column<string>(maxLength: 450, nullable: false),
                    Content = table.Column<string>(maxLength: 500, nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    GroupId = table.Column<int>(nullable: false),
                    ModificationDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupComments_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupComments_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupComments_AuthorId",
                table: "GroupComments",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupComments_GroupId",
                table: "GroupComments",
                column: "GroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupComments");
        }
    }
}
