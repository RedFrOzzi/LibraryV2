﻿// <auto-generated />
using System;
using LibraryV2.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LibraryV2.Migrations
{
    [DbContext(typeof(LibraryDataContext))]
    [Migration("20230806065910_AddedBookCover")]
    partial class AddedBookCover
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.8");

            modelBuilder.Entity("AuthorBook", b =>
                {
                    b.Property<byte[]>("AuthorsId")
                        .HasColumnType("BLOB");

                    b.Property<byte[]>("BooksId")
                        .HasColumnType("BLOB");

                    b.HasKey("AuthorsId", "BooksId");

                    b.HasIndex("BooksId");

                    b.ToTable("AuthorBook");
                });

            modelBuilder.Entity("LibraryV2.Models.Author", b =>
                {
                    b.Property<byte[]>("Id")
                        .HasColumnType("BLOB");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("LibraryV2.Models.Book", b =>
                {
                    b.Property<byte[]>("Id")
                        .HasColumnType("BLOB");

                    b.Property<byte[]>("BookCoverId")
                        .HasColumnType("BLOB");

                    b.Property<byte[]>("CurrentReaderId")
                        .HasColumnType("BLOB");

                    b.Property<byte[]>("EditionId")
                        .HasColumnType("BLOB");

                    b.Property<DateTime?>("ReleaseDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("BookCoverId");

                    b.HasIndex("CurrentReaderId");

                    b.HasIndex("EditionId");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("LibraryV2.Models.BookCover", b =>
                {
                    b.Property<byte[]>("Id")
                        .HasColumnType("BLOB");

                    b.Property<byte[]>("ImageData")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<string>("ImageName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("BookCovers");
                });

            modelBuilder.Entity("LibraryV2.Models.BookEdition", b =>
                {
                    b.Property<byte[]>("Id")
                        .HasColumnType("BLOB");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("BookEditions");
                });

            modelBuilder.Entity("LibraryV2.Models.Reader", b =>
                {
                    b.Property<byte[]>("Id")
                        .HasColumnType("BLOB");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("TEXT");

                    b.Property<int>("Role")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RoleType")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("TokenCreated")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("TokenExpires")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Readers");
                });

            modelBuilder.Entity("LibraryV2.Models.RefreshToken", b =>
                {
                    b.Property<byte[]>("Id")
                        .HasColumnType("BLOB");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Expires")
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("ReaderId")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ReaderId");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("AuthorBook", b =>
                {
                    b.HasOne("LibraryV2.Models.Author", null)
                        .WithMany()
                        .HasForeignKey("AuthorsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LibraryV2.Models.Book", null)
                        .WithMany()
                        .HasForeignKey("BooksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("LibraryV2.Models.Book", b =>
                {
                    b.HasOne("LibraryV2.Models.BookCover", "BookCover")
                        .WithMany()
                        .HasForeignKey("BookCoverId");

                    b.HasOne("LibraryV2.Models.Reader", "CurrentReader")
                        .WithMany("BorrowedBooks")
                        .HasForeignKey("CurrentReaderId");

                    b.HasOne("LibraryV2.Models.BookEdition", "Edition")
                        .WithMany("Books")
                        .HasForeignKey("EditionId");

                    b.Navigation("BookCover");

                    b.Navigation("CurrentReader");

                    b.Navigation("Edition");
                });

            modelBuilder.Entity("LibraryV2.Models.RefreshToken", b =>
                {
                    b.HasOne("LibraryV2.Models.Reader", "Reader")
                        .WithMany()
                        .HasForeignKey("ReaderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Reader");
                });

            modelBuilder.Entity("LibraryV2.Models.BookEdition", b =>
                {
                    b.Navigation("Books");
                });

            modelBuilder.Entity("LibraryV2.Models.Reader", b =>
                {
                    b.Navigation("BorrowedBooks");
                });
#pragma warning restore 612, 618
        }
    }
}
