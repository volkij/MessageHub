using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MessageHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DateRead : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ReadDate",
                schema: "messages",
                table: "Messages",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReadDate",
                schema: "messages",
                table: "Messages");
        }
    }
}
