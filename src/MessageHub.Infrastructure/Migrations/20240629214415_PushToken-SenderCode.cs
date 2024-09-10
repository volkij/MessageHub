﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MessageHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PushTokenSenderCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SenderCode",
                schema: "messages",
                table: "PushTokens",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SenderCode",
                schema: "messages",
                table: "PushTokens");
        }
    }
}
