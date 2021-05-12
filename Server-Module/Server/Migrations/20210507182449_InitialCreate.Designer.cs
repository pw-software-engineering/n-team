﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Server.Database;

namespace Server.Migrations
{
    [DbContext(typeof(ServerDbContext))]
    [Migration("20210507182449_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("Server.Database.Models.AvalaibleTimeIntervalDb", b =>
                {
                    b.Property<int>("TimeIntervalID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime>("FromTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("OfferID")
                        .HasColumnType("int");

                    b.Property<DateTime>("ToTime")
                        .HasColumnType("datetime2");

                    b.HasKey("TimeIntervalID");

                    b.HasIndex("OfferID");

                    b.ToTable("AvalaibleTimeIntervals");

                    b.HasData(
                        new
                        {
                            TimeIntervalID = 1,
                            FromTime = new DateTime(2001, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            OfferID = 2,
                            ToTime = new DateTime(2001, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            TimeIntervalID = 2,
                            FromTime = new DateTime(2001, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            OfferID = 3,
                            ToTime = new DateTime(2001, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            TimeIntervalID = 3,
                            FromTime = new DateTime(2001, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            OfferID = 3,
                            ToTime = new DateTime(2001, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("Server.Database.Models.ClientDb", b =>
                {
                    b.Property<int>("ClientID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ClientID");

                    b.ToTable("Clients");

                    b.HasData(
                        new
                        {
                            ClientID = 1,
                            Email = "TestEmail1",
                            Name = "TestName1",
                            Password = "TestPassword1",
                            Surname = "TestSurname1",
                            Username = "TestUsername1"
                        },
                        new
                        {
                            ClientID = 2,
                            Email = "TestEmail2",
                            Name = "TestName2",
                            Password = "TestPassword2",
                            Surname = "TestSurname2",
                            Username = "TestUsername2"
                        },
                        new
                        {
                            ClientID = 3,
                            Email = "TestEmail3",
                            Name = "TestName3",
                            Password = "TestPassword3",
                            Surname = "TestSurname3",
                            Username = "TestUsername3"
                        });
                });

            modelBuilder.Entity("Server.Database.Models.ClientReservationDb", b =>
                {
                    b.Property<int>("ReservationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int?>("ClientID")
                        .HasColumnType("int");

                    b.Property<DateTime>("FromTime")
                        .HasColumnType("datetime2");

                    b.Property<int?>("HotelID")
                        .HasColumnType("int");

                    b.Property<long>("NumberOfAdults")
                        .HasColumnType("bigint");

                    b.Property<long>("NumberOfChildren")
                        .HasColumnType("bigint");

                    b.Property<int?>("OfferID")
                        .HasColumnType("int");

                    b.Property<int?>("ReviewID")
                        .HasColumnType("int");

                    b.Property<int?>("RoomID")
                        .HasColumnType("int");

                    b.Property<DateTime>("ToTime")
                        .HasColumnType("datetime2");

                    b.HasKey("ReservationID");

                    b.HasIndex("ClientID");

                    b.HasIndex("HotelID");

                    b.HasIndex("OfferID");

                    b.HasIndex("RoomID");

                    b.ToTable("ClientReservations");

                    b.HasData(
                        new
                        {
                            ReservationID = 1,
                            ClientID = 2,
                            FromTime = new DateTime(2001, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            HotelID = 2,
                            NumberOfAdults = 1L,
                            NumberOfChildren = 0L,
                            OfferID = 2,
                            ReviewID = 1,
                            RoomID = 2,
                            ToTime = new DateTime(2001, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            ReservationID = 2,
                            ClientID = 3,
                            FromTime = new DateTime(2001, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            HotelID = 3,
                            NumberOfAdults = 1L,
                            NumberOfChildren = 1L,
                            OfferID = 3,
                            ReviewID = 2,
                            RoomID = 2,
                            ToTime = new DateTime(3001, 2, 4, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            ReservationID = 3,
                            ClientID = 3,
                            FromTime = new DateTime(2001, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            HotelID = 3,
                            NumberOfAdults = 1L,
                            NumberOfChildren = 2L,
                            OfferID = 3,
                            ReviewID = 3,
                            RoomID = 3,
                            ToTime = new DateTime(2001, 3, 6, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("Server.Database.Models.ClientReviewDb", b =>
                {
                    b.Property<int>("ReviewID")
                        .HasColumnType("int");

                    b.Property<int>("ClientID")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("HotelID")
                        .HasColumnType("int");

                    b.Property<int>("OfferID")
                        .HasColumnType("int");

                    b.Property<long>("Rating")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("ReviewDate")
                        .HasColumnType("datetime2");

                    b.HasKey("ReviewID");

                    b.HasIndex("ClientID");

                    b.HasIndex("HotelID");

                    b.HasIndex("OfferID");

                    b.ToTable("ClientReviews");

                    b.HasData(
                        new
                        {
                            ReviewID = 1,
                            ClientID = 2,
                            Content = "TestContent1",
                            HotelID = 2,
                            OfferID = 2,
                            Rating = 1L,
                            ReviewDate = new DateTime(2001, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            ReviewID = 2,
                            ClientID = 3,
                            Content = "TestContent2",
                            HotelID = 3,
                            OfferID = 3,
                            Rating = 2L,
                            ReviewDate = new DateTime(2001, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            ReviewID = 3,
                            ClientID = 3,
                            Content = "TestContent3",
                            HotelID = 3,
                            OfferID = 3,
                            Rating = 3L,
                            ReviewDate = new DateTime(2001, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("Server.Database.Models.HotelInfoDb", b =>
                {
                    b.Property<int>("HotelID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("AccessToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HotelDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HotelName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HotelPreviewPicture")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("HotelID");

                    b.ToTable("HotelInfos");

                    b.HasData(
                        new
                        {
                            HotelID = 1,
                            AccessToken = "TestAccessToken1",
                            City = "TestCity1",
                            Country = "TestCountry1",
                            HotelDescription = "TestHotelDesc1",
                            HotelName = "TestHotelName1",
                            HotelPreviewPicture = "TestHotelPreviewPicture1"
                        },
                        new
                        {
                            HotelID = 2,
                            AccessToken = "TestAccessToken2",
                            City = "TestCity2",
                            Country = "TestCountry2",
                            HotelDescription = "TestHotelDesc2",
                            HotelName = "TestHotelName2",
                            HotelPreviewPicture = "TestHotelPreviewPicture2"
                        },
                        new
                        {
                            HotelID = 3,
                            AccessToken = "TestAccessToken3",
                            City = "TestCity3",
                            Country = "TestCountry3",
                            HotelDescription = "TestHotelDesc3",
                            HotelName = "TestHotelName3",
                            HotelPreviewPicture = "TestHotelPreviewPicture3"
                        });
                });

            modelBuilder.Entity("Server.Database.Models.HotelPictureDb", b =>
                {
                    b.Property<int>("PictureID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("HotelID")
                        .HasColumnType("int");

                    b.Property<string>("Picture")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PictureID");

                    b.HasIndex("HotelID");

                    b.ToTable("HotelPictures");

                    b.HasData(
                        new
                        {
                            PictureID = 1,
                            HotelID = 2,
                            Picture = "TestPicture1"
                        },
                        new
                        {
                            PictureID = 2,
                            HotelID = 3,
                            Picture = "TestPicture2"
                        },
                        new
                        {
                            PictureID = 3,
                            HotelID = 3,
                            Picture = "TestPicture3"
                        });
                });

            modelBuilder.Entity("Server.Database.Models.HotelRoomDb", b =>
                {
                    b.Property<int>("RoomID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("HotelID")
                        .HasColumnType("int");

                    b.Property<string>("HotelRoomNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.HasKey("RoomID");

                    b.HasIndex("HotelID");

                    b.ToTable("HotelRooms");

                    b.HasData(
                        new
                        {
                            RoomID = 1,
                            HotelID = 2,
                            HotelRoomNumber = "TestHotelRoomNumber1",
                            IsActive = false
                        },
                        new
                        {
                            RoomID = 2,
                            HotelID = 3,
                            HotelRoomNumber = "TestHotelRoomNumber2",
                            IsActive = false
                        },
                        new
                        {
                            RoomID = 3,
                            HotelID = 3,
                            HotelRoomNumber = "TestHotelRoomNumber3",
                            IsActive = false
                        },
                        new
                        {
                            RoomID = 4,
                            HotelID = 3,
                            HotelRoomNumber = "TestHotelRoomNumber4",
                            IsActive = false
                        });
                });

            modelBuilder.Entity("Server.Database.Models.OfferDb", b =>
                {
                    b.Property<int>("OfferID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<double>("CostPerAdult")
                        .HasColumnType("float");

                    b.Property<double>("CostPerChild")
                        .HasColumnType("float");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("HotelID")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<long>("MaxGuests")
                        .HasColumnType("bigint");

                    b.Property<string>("OfferPreviewPicture")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OfferTitle")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("OfferID");

                    b.HasIndex("HotelID");

                    b.ToTable("Offers");

                    b.HasData(
                        new
                        {
                            OfferID = 1,
                            CostPerAdult = 11.0,
                            CostPerChild = 10.0,
                            Description = "TestDescription1",
                            HotelID = 2,
                            IsActive = true,
                            IsDeleted = false,
                            MaxGuests = 1L,
                            OfferPreviewPicture = "TestOfferPreviewPicture1",
                            OfferTitle = "TestOfferTitle1"
                        },
                        new
                        {
                            OfferID = 2,
                            CostPerAdult = 22.0,
                            CostPerChild = 20.0,
                            Description = "TestDescription2",
                            HotelID = 3,
                            IsActive = true,
                            IsDeleted = false,
                            MaxGuests = 2L,
                            OfferPreviewPicture = "TestOfferPreviewPicture2",
                            OfferTitle = "TestOfferTitle2"
                        },
                        new
                        {
                            OfferID = 3,
                            CostPerAdult = 33.0,
                            CostPerChild = 30.0,
                            Description = "TestDescription3",
                            HotelID = 3,
                            IsActive = false,
                            IsDeleted = true,
                            MaxGuests = 3L,
                            OfferPreviewPicture = "TestOfferPreviewPicture3",
                            OfferTitle = "TestOfferTitle3"
                        });
                });

            modelBuilder.Entity("Server.Database.Models.OfferHotelRoomDb", b =>
                {
                    b.Property<int>("OfferID")
                        .HasColumnType("int");

                    b.Property<int>("RoomID")
                        .HasColumnType("int");

                    b.HasKey("OfferID", "RoomID");

                    b.HasIndex("RoomID");

                    b.ToTable("OfferHotelRooms");

                    b.HasData(
                        new
                        {
                            OfferID = 1,
                            RoomID = 1
                        },
                        new
                        {
                            OfferID = 2,
                            RoomID = 2
                        },
                        new
                        {
                            OfferID = 3,
                            RoomID = 2
                        },
                        new
                        {
                            OfferID = 3,
                            RoomID = 3
                        });
                });

            modelBuilder.Entity("Server.Database.Models.OfferPictureDb", b =>
                {
                    b.Property<int>("PictureID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("OfferID")
                        .HasColumnType("int");

                    b.Property<string>("Picture")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PictureID");

                    b.HasIndex("OfferID");

                    b.ToTable("OfferPictures");

                    b.HasData(
                        new
                        {
                            PictureID = 1,
                            OfferID = 2,
                            Picture = "TestPicture1"
                        },
                        new
                        {
                            PictureID = 2,
                            OfferID = 3,
                            Picture = "TestPicture2"
                        },
                        new
                        {
                            PictureID = 3,
                            OfferID = 3,
                            Picture = "TestPicture3"
                        });
                });

            modelBuilder.Entity("Server.Database.Models.AvalaibleTimeIntervalDb", b =>
                {
                    b.HasOne("Server.Database.Models.OfferDb", "Offer")
                        .WithMany("AvalaibleTimeIntervals")
                        .HasForeignKey("OfferID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Offer");
                });

            modelBuilder.Entity("Server.Database.Models.ClientReservationDb", b =>
                {
                    b.HasOne("Server.Database.Models.ClientDb", "Client")
                        .WithMany("ClientReservations")
                        .HasForeignKey("ClientID")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("Server.Database.Models.HotelInfoDb", "Hotel")
                        .WithMany()
                        .HasForeignKey("HotelID")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("Server.Database.Models.OfferDb", "Offer")
                        .WithMany()
                        .HasForeignKey("OfferID")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("Server.Database.Models.HotelRoomDb", "Room")
                        .WithMany()
                        .HasForeignKey("RoomID")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("Client");

                    b.Navigation("Hotel");

                    b.Navigation("Offer");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("Server.Database.Models.ClientReviewDb", b =>
                {
                    b.HasOne("Server.Database.Models.ClientDb", "Client")
                        .WithMany("ClientReviews")
                        .HasForeignKey("ClientID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Server.Database.Models.HotelInfoDb", "Hotel")
                        .WithMany("Reviews")
                        .HasForeignKey("HotelID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Server.Database.Models.OfferDb", "Offer")
                        .WithMany("ClientReviews")
                        .HasForeignKey("OfferID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Server.Database.Models.ClientReservationDb", "Reservation")
                        .WithOne("Review")
                        .HasForeignKey("Server.Database.Models.ClientReviewDb", "ReviewID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("Hotel");

                    b.Navigation("Offer");

                    b.Navigation("Reservation");
                });

            modelBuilder.Entity("Server.Database.Models.HotelPictureDb", b =>
                {
                    b.HasOne("Server.Database.Models.HotelInfoDb", "Hotel")
                        .WithMany("HotelPictures")
                        .HasForeignKey("HotelID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Hotel");
                });

            modelBuilder.Entity("Server.Database.Models.HotelRoomDb", b =>
                {
                    b.HasOne("Server.Database.Models.HotelInfoDb", "Hotel")
                        .WithMany("HotelRooms")
                        .HasForeignKey("HotelID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Hotel");
                });

            modelBuilder.Entity("Server.Database.Models.OfferDb", b =>
                {
                    b.HasOne("Server.Database.Models.HotelInfoDb", "Hotel")
                        .WithMany("Offers")
                        .HasForeignKey("HotelID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Hotel");
                });

            modelBuilder.Entity("Server.Database.Models.OfferHotelRoomDb", b =>
                {
                    b.HasOne("Server.Database.Models.OfferDb", "Offer")
                        .WithMany("OfferHotelRooms")
                        .HasForeignKey("OfferID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Server.Database.Models.HotelRoomDb", "Room")
                        .WithMany("OfferHotelRooms")
                        .HasForeignKey("RoomID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Offer");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("Server.Database.Models.OfferPictureDb", b =>
                {
                    b.HasOne("Server.Database.Models.OfferDb", "Offer")
                        .WithMany("OfferPictures")
                        .HasForeignKey("OfferID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Offer");
                });

            modelBuilder.Entity("Server.Database.Models.ClientDb", b =>
                {
                    b.Navigation("ClientReservations");

                    b.Navigation("ClientReviews");
                });

            modelBuilder.Entity("Server.Database.Models.ClientReservationDb", b =>
                {
                    b.Navigation("Review");
                });

            modelBuilder.Entity("Server.Database.Models.HotelInfoDb", b =>
                {
                    b.Navigation("HotelPictures");

                    b.Navigation("HotelRooms");

                    b.Navigation("Offers");

                    b.Navigation("Reviews");
                });

            modelBuilder.Entity("Server.Database.Models.HotelRoomDb", b =>
                {
                    b.Navigation("OfferHotelRooms");
                });

            modelBuilder.Entity("Server.Database.Models.OfferDb", b =>
                {
                    b.Navigation("AvalaibleTimeIntervals");

                    b.Navigation("ClientReviews");

                    b.Navigation("OfferHotelRooms");

                    b.Navigation("OfferPictures");
                });
#pragma warning restore 612, 618
        }
    }
}