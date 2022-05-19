﻿// <auto-generated />
using System;
using Kino;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Kino.Migrations
{
    [DbContext(typeof(KinoDbContext))]
    partial class KinoDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Kino.Features.Claim", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    b.Property<string>("Type")
                        .HasColumnType("text")
                        .HasColumnName("type");

                    b.HasKey("UserId", "Type")
                        .HasName("pk_claims");

                    b.ToTable("claims", (string)null);
                });

            modelBuilder.Entity("Kino.Features.Films.Film", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<TimeSpan>("Duration")
                        .HasColumnType("interval")
                        .HasColumnName("duration");

                    b.Property<string[]>("Genres")
                        .IsRequired()
                        .HasColumnType("text[]")
                        .HasColumnName("genres");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("image_url");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.HasKey("Id")
                        .HasName("pk_films");

                    b.ToTable("films", (string)null);
                });

            modelBuilder.Entity("Kino.Features.Hall", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Capacity")
                        .HasColumnType("integer")
                        .HasColumnName("capacity");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_halls");

                    b.ToTable("halls", (string)null);
                });

            modelBuilder.Entity("Kino.Features.Reviews.Review", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<int>("FilmId")
                        .HasColumnType("integer")
                        .HasColumnName("film_id");

                    b.Property<int>("Score")
                        .HasColumnType("integer")
                        .HasColumnName("score");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_reviews");

                    b.HasIndex("FilmId")
                        .HasDatabaseName("ix_reviews_film_id");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_reviews_user_id");

                    b.ToTable("reviews", (string)null);
                });

            modelBuilder.Entity("Kino.Features.Screenings.Screening", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<decimal>("BasePrice")
                        .HasColumnType("numeric")
                        .HasColumnName("base_price");

                    b.Property<int>("FilmId")
                        .HasColumnType("integer")
                        .HasColumnName("film_id");

                    b.Property<int>("HallId")
                        .HasColumnType("integer")
                        .HasColumnName("hall_id");

                    b.Property<DateTimeOffset>("StartAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("start_at");

                    b.HasKey("Id")
                        .HasName("pk_screenings");

                    b.HasIndex("FilmId")
                        .HasDatabaseName("ix_screenings_film_id");

                    b.HasIndex("HallId")
                        .HasDatabaseName("ix_screenings_hall_id");

                    b.ToTable("screenings", (string)null);
                });

            modelBuilder.Entity("Kino.Features.Seat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("HallId")
                        .HasColumnType("integer")
                        .HasColumnName("hall_id");

                    b.Property<int>("Number")
                        .HasColumnType("integer")
                        .HasColumnName("number");

                    b.Property<decimal>("PriceCoefficient")
                        .HasColumnType("numeric")
                        .HasColumnName("price_coefficient");

                    b.Property<int>("Row")
                        .HasColumnType("integer")
                        .HasColumnName("row");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("type");

                    b.HasKey("Id")
                        .HasName("pk_seats");

                    b.HasIndex("HallId")
                        .HasDatabaseName("ix_seats_hall_id");

                    b.HasIndex("Number", "Row", "HallId")
                        .IsUnique()
                        .HasDatabaseName("ix_seats_number_row_hall_id");

                    b.ToTable("seats", (string)null);
                });

            modelBuilder.Entity("Kino.Features.Tickets.Ticket", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ScreeningId")
                        .HasColumnType("integer")
                        .HasColumnName("screening_id");

                    b.Property<int>("SeatId")
                        .HasColumnType("integer")
                        .HasColumnName("seat_id");

                    b.HasKey("Id")
                        .HasName("pk_tickets");

                    b.HasIndex("ScreeningId")
                        .HasDatabaseName("ix_tickets_screening_id");

                    b.HasIndex("SeatId", "ScreeningId")
                        .IsUnique()
                        .HasDatabaseName("ix_tickets_seat_id_screening_id");

                    b.ToTable("tickets", (string)null);
                });

            modelBuilder.Entity("Kino.Features.Users.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("bytea")
                        .HasColumnName("password_hash");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("bytea")
                        .HasColumnName("password_salt");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasDatabaseName("ix_users_name");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("Kino.Features.Claim", b =>
                {
                    b.HasOne("Kino.Features.Users.User", "User")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_claims_users_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Kino.Features.Reviews.Review", b =>
                {
                    b.HasOne("Kino.Features.Films.Film", "Film")
                        .WithMany("Reviews")
                        .HasForeignKey("FilmId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_reviews_films_film_id");

                    b.HasOne("Kino.Features.Users.User", "User")
                        .WithMany("Reviews")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_reviews_users_user_id");

                    b.Navigation("Film");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Kino.Features.Screenings.Screening", b =>
                {
                    b.HasOne("Kino.Features.Films.Film", "Film")
                        .WithMany("Screenings")
                        .HasForeignKey("FilmId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_screenings_films_film_id");

                    b.HasOne("Kino.Features.Hall", "Hall")
                        .WithMany("Screenings")
                        .HasForeignKey("HallId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_screenings_halls_hall_id");

                    b.Navigation("Film");

                    b.Navigation("Hall");
                });

            modelBuilder.Entity("Kino.Features.Seat", b =>
                {
                    b.HasOne("Kino.Features.Hall", "Hall")
                        .WithMany("Seats")
                        .HasForeignKey("HallId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_seats_halls_hall_id");

                    b.Navigation("Hall");
                });

            modelBuilder.Entity("Kino.Features.Tickets.Ticket", b =>
                {
                    b.HasOne("Kino.Features.Screenings.Screening", "Screening")
                        .WithMany("Tickets")
                        .HasForeignKey("ScreeningId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_tickets_screenings_screening_id");

                    b.HasOne("Kino.Features.Seat", "Seat")
                        .WithMany("Tickets")
                        .HasForeignKey("SeatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_tickets_seats_seat_id");

                    b.Navigation("Screening");

                    b.Navigation("Seat");
                });

            modelBuilder.Entity("Kino.Features.Films.Film", b =>
                {
                    b.Navigation("Reviews");

                    b.Navigation("Screenings");
                });

            modelBuilder.Entity("Kino.Features.Hall", b =>
                {
                    b.Navigation("Screenings");

                    b.Navigation("Seats");
                });

            modelBuilder.Entity("Kino.Features.Screenings.Screening", b =>
                {
                    b.Navigation("Tickets");
                });

            modelBuilder.Entity("Kino.Features.Seat", b =>
                {
                    b.Navigation("Tickets");
                });

            modelBuilder.Entity("Kino.Features.Users.User", b =>
                {
                    b.Navigation("Claims");

                    b.Navigation("Reviews");
                });
#pragma warning restore 612, 618
        }
    }
}
