using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace BlazorMovies.Server.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "genres",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_genres", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "movies",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: false),
                    summary = table.Column<string>(type: "text", nullable: true),
                    intheaters = table.Column<bool>(type: "boolean", nullable: false),
                    trailer = table.Column<string>(type: "text", nullable: true),
                    releasedate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    poster = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_movies", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "people",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    biography = table.Column<string>(type: "text", nullable: true),
                    picture = table.Column<string>(type: "text", nullable: true),
                    dateofbirth = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_people", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "moviesgenres",
                schema: "public",
                columns: table => new
                {
                    movieid = table.Column<int>(type: "integer", nullable: false),
                    genresid = table.Column<int>(type: "integer", nullable: false),
                    genreid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_moviesgenres", x => new { x.movieid, x.genresid });
                    table.ForeignKey(
                        name: "fk_moviesgenres_genres_genreid",
                        column: x => x.genreid,
                        principalSchema: "public",
                        principalTable: "genres",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_moviesgenres_movies_movieid",
                        column: x => x.movieid,
                        principalSchema: "public",
                        principalTable: "movies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "moviesactors",
                schema: "public",
                columns: table => new
                {
                    personid = table.Column<int>(type: "integer", nullable: false),
                    movieid = table.Column<int>(type: "integer", nullable: false),
                    character = table.Column<string>(type: "text", nullable: true),
                    order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_moviesactors", x => new { x.movieid, x.personid });
                    table.ForeignKey(
                        name: "fk_moviesactors_movies_movieid",
                        column: x => x.movieid,
                        principalSchema: "public",
                        principalTable: "movies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_moviesactors_people_personid",
                        column: x => x.personid,
                        principalSchema: "public",
                        principalTable: "people",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_moviesactors_personid",
                schema: "public",
                table: "moviesactors",
                column: "personid");

            migrationBuilder.CreateIndex(
                name: "ix_moviesgenres_genreid",
                schema: "public",
                table: "moviesgenres",
                column: "genreid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "moviesactors",
                schema: "public");

            migrationBuilder.DropTable(
                name: "moviesgenres",
                schema: "public");

            migrationBuilder.DropTable(
                name: "people",
                schema: "public");

            migrationBuilder.DropTable(
                name: "genres",
                schema: "public");

            migrationBuilder.DropTable(
                name: "movies",
                schema: "public");
        }
    }
}
