using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShop.Data.Migrations
{
    /// <inheritdoc />
    public partial class addProductImageTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 4, 22, 16, 10, 46, 668, DateTimeKind.Local).AddTicks(864));

            migrationBuilder.CreateTable(
                name: "ProductImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Caption = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    FileSize = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImages_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("f611bbfd-a34d-4f71-bdaf-1d373f8cb891"),
                column: "ConcurrencyStamp",
                value: "677e2dfe-b64c-4943-877d-11c370aa29b1");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("f41f21f2-fe66-4df9-a2bc-cfe970f45479"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "acbdf3c1-5408-4ed8-9a6d-30ab90002a32", "AQAAAAEAACcQAAAAEKLb+c4yaxsE0JWdXYuReCV2Q0NoLoyboTQFlAwF5OIwi1ubOEclELzNjnSymVgHfA==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 4, 23, 13, 41, 53, 655, DateTimeKind.Local).AddTicks(6046));

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductId",
                table: "ProductImages",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductImages");

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 4, 22, 16, 10, 46, 668, DateTimeKind.Local).AddTicks(864),
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("f611bbfd-a34d-4f71-bdaf-1d373f8cb891"),
                column: "ConcurrencyStamp",
                value: "fb28e79d-7010-424b-ab2a-5ec04932d6cc");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("f41f21f2-fe66-4df9-a2bc-cfe970f45479"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "f925ed41-b99a-401d-9c03-c7478d7cc302", "AQAAAAEAACcQAAAAEDsZqzTkmakqgXEjLMCZVNy0wi87ggYvtsFIB/S+a28gkkDfmtaFlWchzQszhh+ngw==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 4, 22, 16, 10, 46, 671, DateTimeKind.Local).AddTicks(8815));
        }
    }
}
