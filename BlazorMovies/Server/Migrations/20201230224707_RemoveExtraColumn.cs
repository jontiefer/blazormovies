using Microsoft.EntityFrameworkCore.Migrations;

namespace BlazorMovies.Server.Migrations
{
    public partial class RemoveExtraColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_moviesgenres_genres_genreid",
                schema: "public",
                table: "moviesgenres");

            migrationBuilder.DropPrimaryKey(
                name: "pk_moviesgenres",
                schema: "public",
                table: "moviesgenres");

            migrationBuilder.DropColumn(
                name: "genresid",
                schema: "public",
                table: "moviesgenres");

            migrationBuilder.AlterColumn<int>(
                name: "genreid",
                schema: "public",
                table: "moviesgenres",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "pk_moviesgenres",
                schema: "public",
                table: "moviesgenres",
                columns: new[] { "movieid", "genreid" });

            migrationBuilder.AddForeignKey(
                name: "fk_moviesgenres_genres_genreid",
                schema: "public",
                table: "moviesgenres",
                column: "genreid",
                principalSchema: "public",
                principalTable: "genres",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_moviesgenres_genres_genreid",
                schema: "public",
                table: "moviesgenres");

            migrationBuilder.DropPrimaryKey(
                name: "pk_moviesgenres",
                schema: "public",
                table: "moviesgenres");

            migrationBuilder.AlterColumn<int>(
                name: "genreid",
                schema: "public",
                table: "moviesgenres",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "genresid",
                schema: "public",
                table: "moviesgenres",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "pk_moviesgenres",
                schema: "public",
                table: "moviesgenres",
                columns: new[] { "movieid", "genresid" });

            migrationBuilder.AddForeignKey(
                name: "fk_moviesgenres_genres_genreid",
                schema: "public",
                table: "moviesgenres",
                column: "genreid",
                principalSchema: "public",
                principalTable: "genres",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
