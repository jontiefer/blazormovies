﻿// <auto-generated />
using System;
using BlazorMovies.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace BlazorMovies.Server.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("public")
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("BlazorMovies.Shared.Entities.Genre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_genres");

                    b.ToTable("genres");
                });

            modelBuilder.Entity("BlazorMovies.Shared.Entities.Movie", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .UseIdentityByDefaultColumn();

                    b.Property<bool>("InTheaters")
                        .HasColumnType("boolean")
                        .HasColumnName("intheaters");

                    b.Property<string>("Poster")
                        .HasColumnType("text")
                        .HasColumnName("poster");

                    b.Property<DateTime?>("ReleaseDate")
                        .IsRequired()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("releasedate");

                    b.Property<string>("Summary")
                        .HasColumnType("text")
                        .HasColumnName("summary");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.Property<string>("Trailer")
                        .HasColumnType("text")
                        .HasColumnName("trailer");

                    b.HasKey("Id")
                        .HasName("pk_movies");

                    b.ToTable("movies");
                });

            modelBuilder.Entity("BlazorMovies.Shared.Entities.MoviesActors", b =>
                {
                    b.Property<int>("MovieId")
                        .HasColumnType("integer")
                        .HasColumnName("movieid");

                    b.Property<int>("PersonId")
                        .HasColumnType("integer")
                        .HasColumnName("personid");

                    b.Property<string>("Character")
                        .HasColumnType("text")
                        .HasColumnName("character");

                    b.Property<int>("Order")
                        .HasColumnType("integer")
                        .HasColumnName("order");

                    b.HasKey("MovieId", "PersonId")
                        .HasName("pk_moviesactors");

                    b.HasIndex("PersonId")
                        .HasDatabaseName("ix_moviesactors_personid");

                    b.ToTable("moviesactors");
                });

            modelBuilder.Entity("BlazorMovies.Shared.Entities.MoviesGenres", b =>
                {
                    b.Property<int>("MovieId")
                        .HasColumnType("integer")
                        .HasColumnName("movieid");

                    b.Property<int>("GenreId")
                        .HasColumnType("integer")
                        .HasColumnName("genreid");

                    b.HasKey("MovieId", "GenreId")
                        .HasName("pk_moviesgenres");

                    b.HasIndex("GenreId")
                        .HasDatabaseName("ix_moviesgenres_genreid");

                    b.ToTable("moviesgenres");
                });

            modelBuilder.Entity("BlazorMovies.Shared.Entities.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("Biography")
                        .HasColumnType("text")
                        .HasColumnName("biography");

                    b.Property<DateTime?>("DateOfBirth")
                        .IsRequired()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("dateofbirth");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("Picture")
                        .HasColumnType("text")
                        .HasColumnName("picture");

                    b.HasKey("Id")
                        .HasName("pk_people");

                    b.ToTable("people");
                });

            modelBuilder.Entity("BlazorMovies.Shared.Entities.MoviesActors", b =>
                {
                    b.HasOne("BlazorMovies.Shared.Entities.Movie", "Movie")
                        .WithMany("MoviesActors")
                        .HasForeignKey("MovieId")
                        .HasConstraintName("fk_moviesactors_movies_movieid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BlazorMovies.Shared.Entities.Person", "Person")
                        .WithMany("MoviesActors")
                        .HasForeignKey("PersonId")
                        .HasConstraintName("fk_moviesactors_people_personid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Movie");

                    b.Navigation("Person");
                });

            modelBuilder.Entity("BlazorMovies.Shared.Entities.MoviesGenres", b =>
                {
                    b.HasOne("BlazorMovies.Shared.Entities.Genre", "Genre")
                        .WithMany("MoviesGenres")
                        .HasForeignKey("GenreId")
                        .HasConstraintName("fk_moviesgenres_genres_genreid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BlazorMovies.Shared.Entities.Movie", "Movie")
                        .WithMany("MoviesGenres")
                        .HasForeignKey("MovieId")
                        .HasConstraintName("fk_moviesgenres_movies_movieid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Genre");

                    b.Navigation("Movie");
                });

            modelBuilder.Entity("BlazorMovies.Shared.Entities.Genre", b =>
                {
                    b.Navigation("MoviesGenres");
                });

            modelBuilder.Entity("BlazorMovies.Shared.Entities.Movie", b =>
                {
                    b.Navigation("MoviesActors");

                    b.Navigation("MoviesGenres");
                });

            modelBuilder.Entity("BlazorMovies.Shared.Entities.Person", b =>
                {
                    b.Navigation("MoviesActors");
                });
#pragma warning restore 612, 618
        }
    }
}
