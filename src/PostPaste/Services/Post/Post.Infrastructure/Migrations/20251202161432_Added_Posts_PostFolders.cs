using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Post.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Added_Posts_PostFolders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostCategories_PostFolders_PostId",
                table: "PostCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_PostFolders_PostFolderEntity_FolderId",
                table: "PostFolders");

            migrationBuilder.DropTable(
                name: "PostFolderEntity");

            migrationBuilder.DropIndex(
                name: "IX_PostFolders_FolderId",
                table: "PostFolders");

            migrationBuilder.DropColumn(
                name: "BlobKey",
                table: "PostFolders");

            migrationBuilder.DropColumn(
                name: "ExpirationDate",
                table: "PostFolders");

            migrationBuilder.DropColumn(
                name: "FolderId",
                table: "PostFolders");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "PostFolders");

            migrationBuilder.DropColumn(
                name: "PasswordSalt",
                table: "PostFolders");

            migrationBuilder.DropColumn(
                name: "ShortCode",
                table: "PostFolders");

            migrationBuilder.DropColumn(
                name: "Tags",
                table: "PostFolders");

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Tags = table.Column<IReadOnlyCollection<string>>(type: "jsonb", nullable: false, defaultValue: new string[0]),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    PasswordSalt = table.Column<string>(type: "text", nullable: true),
                    ExpirationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ShortCode = table.Column<string>(type: "text", nullable: false),
                    BlobKey = table.Column<string>(type: "text", nullable: false),
                    OwnerId = table.Column<int>(type: "integer", nullable: false),
                    FolderId = table.Column<int>(type: "integer", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_PostFolders_FolderId",
                        column: x => x.FolderId,
                        principalTable: "PostFolders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Posts_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_FolderId",
                table: "Posts",
                column: "FolderId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_IsDeleted",
                table: "Posts",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_OwnerId",
                table: "Posts",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostCategories_Posts_PostId",
                table: "PostCategories",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostCategories_Posts_PostId",
                table: "PostCategories");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.AddColumn<string>(
                name: "BlobKey",
                table: "PostFolders",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpirationDate",
                table: "PostFolders",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FolderId",
                table: "PostFolders",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "PostFolders",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasswordSalt",
                table: "PostFolders",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShortCode",
                table: "PostFolders",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<IReadOnlyCollection<string>>(
                name: "Tags",
                table: "PostFolders",
                type: "jsonb",
                nullable: false,
                defaultValue: new string[0]);

            migrationBuilder.CreateTable(
                name: "PostFolderEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    OwnerId = table.Column<int>(type: "integer", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostFolderEntity", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostFolders_FolderId",
                table: "PostFolders",
                column: "FolderId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostCategories_PostFolders_PostId",
                table: "PostCategories",
                column: "PostId",
                principalTable: "PostFolders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostFolders_PostFolderEntity_FolderId",
                table: "PostFolders",
                column: "FolderId",
                principalTable: "PostFolderEntity",
                principalColumn: "Id");
        }
    }
}
