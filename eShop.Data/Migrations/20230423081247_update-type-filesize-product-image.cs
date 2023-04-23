using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShop.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatetypefilesizeproductimage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "FileSize",
                table: "ProductImages",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("f611bbfd-a34d-4f71-bdaf-1d373f8cb891"),
                column: "ConcurrencyStamp",
                value: "51d7dcea-65a3-4b40-9601-2f989d03eb1a");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("f41f21f2-fe66-4df9-a2bc-cfe970f45479"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "bcc3928d-6b8a-4d25-97d9-8518eb7efc9b", "AQAAAAEAACcQAAAAEOonOe49QuG11qhsEF+viaFbRj33dUw31uf0ee8CLxxGZf/IgiXxsOHQr56ZEBvsMA==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 4, 23, 15, 12, 47, 329, DateTimeKind.Local).AddTicks(8585));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "FileSize",
                table: "ProductImages",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

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
        }
    }
}
