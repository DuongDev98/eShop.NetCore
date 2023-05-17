using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace eShop.Data.Migrations
{
    /// <inheritdoc />
    public partial class add_table_slide : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isFeatured",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Slides",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Slides", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("f611bbfd-a34d-4f71-bdaf-1d373f8cb891"),
                column: "ConcurrencyStamp",
                value: "7e3a40c6-af7f-4725-ac41-8b2829628aec");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("f41f21f2-fe66-4df9-a2bc-cfe970f45479"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "6539e8a3-1678-4cb6-8377-5a49f268a5b1", "AQAAAAEAACcQAAAAENUHWI0HxHrYT8c/IUiltWeo9hyiylBSvv9goYLKYM7QyUaXSBu/sBmC/eD8l8N6dg==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateCreated", "isFeatured" },
                values: new object[] { new DateTime(2023, 5, 17, 0, 1, 6, 966, DateTimeKind.Local).AddTicks(7043), false });

            migrationBuilder.InsertData(
                table: "Slides",
                columns: new[] { "Id", "Description", "Image", "Name", "Url" },
                values: new object[,]
                {
                    { 1, "Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.", "/themes/images/carousel/1.png", "Second Thumbnail label", "#" },
                    { 2, "Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.", "/themes/images/carousel/2.png", "Second Thumbnail label", "#" },
                    { 3, "Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.", "/themes/images/carousel/3.png", "Second Thumbnail label", "#" },
                    { 4, "Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.", "/themes/images/carousel/4.png", "Second Thumbnail label", "#" },
                    { 5, "Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.", "/themes/images/carousel/5.png", "Second Thumbnail label", "#" },
                    { 6, "Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.", "/themes/images/carousel/6.png", "Second Thumbnail label", "#" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Slides");

            migrationBuilder.DropColumn(
                name: "isFeatured",
                table: "Products");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("f611bbfd-a34d-4f71-bdaf-1d373f8cb891"),
                column: "ConcurrencyStamp",
                value: "cfee47a5-2e00-4cc2-82df-46c086455b22");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("f41f21f2-fe66-4df9-a2bc-cfe970f45479"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "cc8dd676-92c2-4d10-b61e-bb88e9780606", "AQAAAAEAACcQAAAAEP0Wgkg5Bh1tApXGZxs5PcsmGIJJq4E0mVQkkkdUffI3DgLX1G9Mj+CTqjA1tsKeag==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 5, 16, 19, 18, 16, 260, DateTimeKind.Local).AddTicks(1339));
        }
    }
}
