using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalogAPI.Migrations
{
    /// <inheritdoc />
    public partial class PopulateProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "INSERT INTO Products VALUES (1, 'Coca-Cola Diet', 'Soda 350ml', 5.45, 'cocacola.jpg', 50, now(), 1)"
            );
            migrationBuilder.Sql(
                "INSERT INTO Products VALUES (2, 'Tuna Sandwich', 'Tuna Sandwich with Mayo', 8.40, 'tuna.jpg', 10, now(), 2)"
            );
            migrationBuilder.Sql(
                "INSERT INTO Products VALUES (3, 'Pudding 100g', 'Condensed milk pudding 100g', 6.75, 'pudding.jpg', 20, now(), 3)"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Products");
        }
    }
}
