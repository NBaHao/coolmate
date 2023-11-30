using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace CoolMate.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "address",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    address_line = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    province = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    district = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    commune = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "product_category",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    parent_category_id = table.Column<int>(type: "int", nullable: true),
                    category_name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    slug = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(type: "longtext", nullable: true),
                    ClaimType = table.Column<string>(type: "longtext", nullable: true),
                    ClaimValue = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: true),
                    NormalizedName = table.Column<string>(type: "longtext", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "site_user",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false),
                    name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "longtext", nullable: true),
                    phone_number = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    gender = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: true),
                    height = table.Column<int>(type: "int", nullable: true),
                    weight = table.Column<int>(type: "int", nullable: true),
                    birthday = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true),
                    UserName = table.Column<string>(type: "longtext", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "longtext", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "longtext", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PasswordHash = table.Column<string>(type: "longtext", nullable: true),
                    SecurityStamp = table.Column<string>(type: "longtext", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "longtext", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_site_user", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(type: "longtext", nullable: true),
                    ClaimType = table.Column<string>(type: "longtext", nullable: true),
                    ClaimValue = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "longtext", nullable: false),
                    ProviderKey = table.Column<string>(type: "longtext", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "longtext", nullable: true),
                    UserId = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false),
                    RoleId = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "longtext", nullable: true),
                    LoginProvider = table.Column<string>(type: "longtext", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    Value = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "product",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    category_id = table.Column<int>(type: "int", nullable: true),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    description = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true),
                    price_int = table.Column<int>(type: "int", nullable: true),
                    price_str = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "fk_category",
                        column: x => x.category_id,
                        principalTable: "product_category",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "shop_order",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<string>(type: "varchar(255)", nullable: true),
                    order_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    payment_method = table.Column<int>(type: "int", nullable: true),
                    shipping_address = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    shipping_method = table.Column<int>(type: "int", nullable: true),
                    order_total = table.Column<int>(type: "int", nullable: true),
                    order_status = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "fk_shoporder_user",
                        column: x => x.user_id,
                        principalTable: "site_user",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "shopping_cart",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<string>(type: "varchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "fk_shopcart_user",
                        column: x => x.user_id,
                        principalTable: "site_user",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "user_address",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<string>(type: "varchar(255)", nullable: true),
                    address_id = table.Column<int>(type: "int", nullable: true),
                    name = table.Column<string>(type: "longtext", nullable: true),
                    phone_number = table.Column<string>(type: "longtext", nullable: true),
                    is_default = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                    table.ForeignKey(
                        name: "fk_useradd_address",
                        column: x => x.address_id,
                        principalTable: "address",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_useradd_user",
                        column: x => x.user_id,
                        principalTable: "site_user",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "product_item",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    product_id = table.Column<int>(type: "int", nullable: true),
                    size = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true),
                    color = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    color_image = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true),
                    qty_in_stock = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "fk_proditem_product",
                        column: x => x.product_id,
                        principalTable: "product",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "order_line",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    product_item_id = table.Column<int>(type: "int", nullable: true),
                    order_id = table.Column<int>(type: "int", nullable: true),
                    qty = table.Column<int>(type: "int", nullable: true),
                    price = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "fk_orderline_order",
                        column: x => x.order_id,
                        principalTable: "shop_order",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_orderline_proditem",
                        column: x => x.product_item_id,
                        principalTable: "product_item",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "product_item_image",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    product_item_id = table.Column<int>(type: "int", nullable: true),
                    url = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "fk_product_item_id",
                        column: x => x.product_item_id,
                        principalTable: "product_item",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "shopping_cart_item",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    cart_id = table.Column<int>(type: "int", nullable: true),
                    product_item_id = table.Column<int>(type: "int", nullable: true),
                    qty = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "fk_shopcartitem_proditem",
                        column: x => x.product_item_id,
                        principalTable: "product_item",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_shopcartitem_shopcart",
                        column: x => x.cart_id,
                        principalTable: "shopping_cart",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "user_review",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<string>(type: "varchar(255)", nullable: true),
                    ordered_product_id = table.Column<int>(type: "int", nullable: true),
                    rating_value = table.Column<int>(type: "int", nullable: true),
                    comment = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "fk_review_product",
                        column: x => x.ordered_product_id,
                        principalTable: "product_item",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_review_user",
                        column: x => x.user_id,
                        principalTable: "site_user",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "fk_orderline_order",
                table: "order_line",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "fk_orderline_proditem",
                table: "order_line",
                column: "product_item_id");

            migrationBuilder.CreateIndex(
                name: "fk_category",
                table: "product",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "fk_proditem_product",
                table: "product_item",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "fk_product_item_id",
                table: "product_item_image",
                column: "product_item_id");

            migrationBuilder.CreateIndex(
                name: "fk_shoporder_paymethod",
                table: "shop_order",
                column: "payment_method");

            migrationBuilder.CreateIndex(
                name: "fk_shoporder_shipmethod",
                table: "shop_order",
                column: "shipping_method");

            migrationBuilder.CreateIndex(
                name: "fk_shoporder_status",
                table: "shop_order",
                column: "order_status");

            migrationBuilder.CreateIndex(
                name: "fk_shoporder_user",
                table: "shop_order",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "fk_shopcart_user",
                table: "shopping_cart",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "fk_shopcartitem_proditem",
                table: "shopping_cart_item",
                column: "product_item_id");

            migrationBuilder.CreateIndex(
                name: "fk_shopcartitem_shopcart",
                table: "shopping_cart_item",
                column: "cart_id");

            migrationBuilder.CreateIndex(
                name: "fk_useradd_address",
                table: "user_address",
                column: "address_id");

            migrationBuilder.CreateIndex(
                name: "fk_useradd_user",
                table: "user_address",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "fk_review_product",
                table: "user_review",
                column: "ordered_product_id");

            migrationBuilder.CreateIndex(
                name: "fk_review_user",
                table: "user_review",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "order_line");

            migrationBuilder.DropTable(
                name: "product_item_image");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "shopping_cart_item");

            migrationBuilder.DropTable(
                name: "user_address");

            migrationBuilder.DropTable(
                name: "user_review");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "shop_order");

            migrationBuilder.DropTable(
                name: "shopping_cart");

            migrationBuilder.DropTable(
                name: "address");

            migrationBuilder.DropTable(
                name: "product_item");

            migrationBuilder.DropTable(
                name: "site_user");

            migrationBuilder.DropTable(
                name: "product");

            migrationBuilder.DropTable(
                name: "product_category");
        }
    }
}
