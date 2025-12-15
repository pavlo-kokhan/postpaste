using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Post.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemovedPostShortCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShortCode",
                table: "Posts");

            migrationBuilder.AlterColumn<IReadOnlyCollection<string>>(
                name: "Tags",
                table: "Posts",
                type: "jsonb",
                nullable: false,
                defaultValue: new string[0],
                oldClrType: typeof(IReadOnlyCollection<string>),
                oldType: "jsonb",
                oldDefaultValue: new string[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<IReadOnlyCollection<string>>(
                name: "Tags",
                table: "Posts",
                type: "jsonb",
                nullable: false,
                defaultValue: new string[0],
                oldClrType: typeof(IReadOnlyCollection<string>),
                oldType: "jsonb",
                oldDefaultValue: new string[0]);

            migrationBuilder.AddColumn<string>(
                name: "ShortCode",
                table: "Posts",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
