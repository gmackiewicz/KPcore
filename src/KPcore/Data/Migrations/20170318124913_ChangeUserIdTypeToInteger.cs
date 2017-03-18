using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace KPcore.Data.Migrations
{
    public partial class ChangeUserIdTypeToInteger : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex("IX_TopicEntries_AuthorId", "TopicEntries");
            migrationBuilder.DropForeignKey("FK_TopicEntries_AspNetUsers_AuthorId", "TopicEntries");

            migrationBuilder.DropIndex("IX_Topics_TeacherId", "Topics");
            migrationBuilder.DropForeignKey("FK_Topics_AspNetUsers_TeacherId", "Topics");

            migrationBuilder.DropPrimaryKey("PK_StudentGroups", "StudentGroups");
            migrationBuilder.DropIndex("IX_StudentGroups_StudentId", "StudentGroups");
            migrationBuilder.DropForeignKey("FK_StudentGroups_AspNetUsers_StudentId", "StudentGroups");

            migrationBuilder.DropIndex("IX_GroupComments_AuthorId", "GroupComments");
            migrationBuilder.DropForeignKey("FK_GroupComments_AspNetUsers_AuthorId", "GroupComments");


            migrationBuilder.DropForeignKey("FK_AspNetUserRoles_AspNetUsers_UserId", "AspNetUserRoles");

            migrationBuilder.DropPrimaryKey("PK_AspNetUserTokens", "AspNetUserTokens");

            migrationBuilder.DropPrimaryKey("PK_AspNetUserRoles", "AspNetUserRoles");
            migrationBuilder.DropIndex("IX_AspNetUserRoles_RoleId", "AspNetUserRoles");
            migrationBuilder.DropForeignKey("FK_AspNetUserRoles_AspNetRoles_RoleId", "AspNetUserRoles");
            migrationBuilder.DropIndex("IX_AspNetUserRoles_UserId", "AspNetUserRoles");

            migrationBuilder.DropIndex("IX_AspNetUserLogins_UserId", "AspNetUserLogins");
            migrationBuilder.DropForeignKey("FK_AspNetUserLogins_AspNetUsers_UserId", "AspNetUserLogins");

            migrationBuilder.DropIndex("IX_AspNetUserClaims_UserId", "AspNetUserClaims");
            migrationBuilder.DropForeignKey("FK_AspNetUserClaims_AspNetUsers_UserId", "AspNetUserClaims");

            migrationBuilder.DropIndex("IX_AspNetRoleClaims_RoleId", "AspNetRoleClaims");
            migrationBuilder.DropForeignKey("FK_AspNetRoleClaims_AspNetRoles_RoleId", "AspNetRoleClaims");

            migrationBuilder.DropPrimaryKey("PK_AspNetRoles", "AspNetRoles");

            migrationBuilder.DropPrimaryKey("PK_AspNetUsers", "AspNetUsers");

            #region Alters

            migrationBuilder.AlterColumn<int>(
                name: "AuthorId",
                table: "TopicEntries",
                nullable: false);

            migrationBuilder.AlterColumn<int>(
                name: "TeacherId",
                table: "Topics",
                nullable: false);

            migrationBuilder.AlterColumn<int>(
                name: "StudentId",
                table: "StudentGroups",
                nullable: false);

            migrationBuilder.AlterColumn<int>(
                name: "AuthorId",
                table: "GroupComments",
                nullable: false);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "AspNetUserTokens",
                nullable: false);

            migrationBuilder.AlterColumn<int>(
                name: "RoleId",
                table: "AspNetUserRoles",
                nullable: false);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "AspNetUserRoles",
                nullable: false);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "AspNetUserLogins",
                nullable: false);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "AspNetUserClaims",
                nullable: false);

            migrationBuilder.AlterColumn<int>(
                name: "RoleId",
                table: "AspNetRoleClaims",
                nullable: false);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "AspNetRoles",
                nullable: false)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "AspNetUsers",
                nullable: false)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            #endregion

            // AspNetUsers AspNetRoles
            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetRoles",
                table: "AspNetRoles",
                column: "Id");

            // AspNetRoleClaims
            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            // AspNetUserClaims
            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            // AspNetUserLogins
            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            // AspNetUserRoles
            migrationBuilder.AddPrimaryKey(
                name: "PK_AspnetUserRoles",
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_UserId",
                table: "AspNetUserRoles",
                column: "UserId");

            // AspNetUserTokens
            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserTokens",
                table: "AspNetUserTokens",
                columns: new[] { "UserId", "LoginProvider", "Name" });

            // AspNetUserRoles
            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            // GroupComments
            migrationBuilder.CreateIndex(
                name: "IX_GroupComments_AuthorId",
                table: "GroupComments",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupComments_AspNetUsers_AuthorId",
                table: "GroupComments",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            // StudentGroups
            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentGroups",
                table: "StudentGroups",
                columns: new[] { "StudentId", "GroupId" }
            );

            migrationBuilder.CreateIndex(
                name: "IX_StudentGroups_StudentId",
                table: "StudentGroups",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentGroups_AspNetUsers_StudentId",
                table: "StudentGroups",
                column: "StudentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            // Topics
            migrationBuilder.AddForeignKey(
                name: "FK_Topics_AspNetUsers_TeacherId",
                table: "Topics",
                column: "TeacherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            // TopicEntries
            migrationBuilder.AddForeignKey(
                name: "FK_TopicEntries_AspNetUsers_AuthorId",
                table: "TopicEntries",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.CreateIndex(
                name: "IX_TopicEntries_AuthorId",
                table: "TopicEntries",
                column: "AuthorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AuthorId",
                table: "TopicEntries",
                maxLength: 450,
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "TeacherId",
                table: "Topics",
                maxLength: 450,
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "StudentId",
                table: "StudentGroups",
                maxLength: 450,
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "AuthorId",
                table: "GroupComments",
                maxLength: 450,
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AspNetUsers",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AspNetUserTokens",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "RoleId",
                table: "AspNetUserRoles",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AspNetUserRoles",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AspNetUserLogins",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AspNetUserClaims",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "RoleId",
                table: "AspNetRoleClaims",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AspNetRoles",
                nullable: false);
        }
    }
}
