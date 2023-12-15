using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoolMate.Migrations
{
    /// <inheritdoc />
    public partial class update_shoporder_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "shop_order",
                type: "varchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "shop_order",
                type: "varchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "phone",
                table: "shop_order",
                type: "varchar(10)",
                maxLength: 10,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "email",
                table: "shop_order");

            migrationBuilder.DropColumn(
                name: "name",
                table: "shop_order");

            migrationBuilder.DropColumn(
                name: "phone",
                table: "shop_order");
        }
    }
}
