﻿// <auto-generated />
using System;
using GHSTShipping.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GHSTShipping.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241107065400_ChangeDatatypeForAvgMonthlyYield")]
    partial class ChangeDatatypeForAvgMonthlyYield
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("GHSTShipping.Domain.Entities.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("CalculateWeight")
                        .HasColumnType("bigint");

                    b.Property<string>("ClientOrderCode")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("CodAmount")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .HasMaxLength(4000)
                        .HasColumnType("nvarchar(4000)");

                    b.Property<long>("ConvertedWeight")
                        .HasColumnType("bigint");

                    b.Property<string>("Coupon")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CurrentStatus")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("CustomDeliveryFee")
                        .HasColumnType("int");

                    b.Property<int?>("DeliverStationId")
                        .HasColumnType("int");

                    b.Property<int>("DeliveryFee")
                        .HasColumnType("int");

                    b.Property<string>("DeliveryPartner")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FromAddress")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int?>("FromDistrictId")
                        .HasColumnType("int");

                    b.Property<string>("FromDistrictName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FromName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FromPhone")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int?>("FromProvinceId")
                        .HasColumnType("int");

                    b.Property<string>("FromProvinceName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FromWardId")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FromWardName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Height")
                        .HasColumnType("int");

                    b.Property<int>("InsuranceValue")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsPublished")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("LastModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("LastSyncDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Length")
                        .HasColumnType("int");

                    b.Property<string>("Note")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("PartnerShopId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PaymentTypeId")
                        .HasColumnType("int");

                    b.Property<string>("PickShift")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PickStationId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("PublishDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("RequiredNote")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("ReturnAddress")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("ReturnDistrictId")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ReturnDistrictName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ReturnName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ReturnPhone")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ReturnWardCode")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ReturnWardName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("ServiceId")
                        .HasColumnType("int");

                    b.Property<int>("ServiceTypeId")
                        .HasColumnType("int");

                    b.Property<Guid?>("ShopId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ToAddress")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int?>("ToDistrictId")
                        .HasColumnType("int");

                    b.Property<string>("ToDistrictName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ToName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ToPhone")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int?>("ToProvinceId")
                        .HasColumnType("int");

                    b.Property<string>("ToProvinceName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ToWardId")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ToWardName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("UniqueCode")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("Weight")
                        .HasColumnType("int");

                    b.Property<int>("Width")
                        .HasColumnType("int");

                    b.Property<DateTime?>("private_expected_delivery_time")
                        .HasColumnType("datetime2");

                    b.Property<string>("private_operation_partner")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("private_order_code")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("private_sort_code")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("private_total_fee")
                        .HasColumnType("int");

                    b.Property<string>("private_trans_type")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("ShopId");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("GHSTShipping.Domain.Entities.OrderItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Code")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("Height")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("ItemOrderCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("LastModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("Length")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("Price")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Weight")
                        .HasColumnType("int");

                    b.Property<int?>("Width")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.ToTable("OrderItem");
                });

            modelBuilder.Entity("GHSTShipping.Domain.Entities.OrderStatusHistory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ChangedBy")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("LastModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Notes")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Status")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("OrderStatusHistory");
                });

            modelBuilder.Entity("GHSTShipping.Domain.Entities.PartnerConfig", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ApiKey")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("DeliveryPartner")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FullName")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<bool>("IsActivated")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("LastModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ProdEnv")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SanboxEnv")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("PartnerConfig");
                });

            modelBuilder.Entity("GHSTShipping.Domain.Entities.Shop", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("AllowPublishOrder")
                        .HasColumnType("bit");

                    b.Property<bool>("AllowUsePartnerShopAddress")
                        .HasColumnType("bit");

                    b.Property<string>("AvgMonthlyYield")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BankAccountHolder")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("BankAccountNumber")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("BankAddress")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("BankName")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("GhnShopId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsVerified")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("LastModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTime?>("RegisterDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UniqueCode")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Shop");
                });

            modelBuilder.Entity("GHSTShipping.Domain.Entities.ShopOrderCodeSequence", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("LastModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("SequenceName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<Guid>("ShopId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("ShopOrderCodeSequence");
                });

            modelBuilder.Entity("GHSTShipping.Domain.Entities.ShopPartnerConfig", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("ClientPhone")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DistrictId")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("DistrictName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("LastModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PartnerConfigId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PartnerShopId")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ProvinceId")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ProvinceName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<Guid>("ShopId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ShopName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("WardCode")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("WardName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("PartnerConfigId");

                    b.HasIndex("ShopId");

                    b.ToTable("ShopPartnerConfig");
                });

            modelBuilder.Entity("GHSTShipping.Domain.Entities.ShopPricePlan", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("ConvertedWeight")
                        .HasColumnType("int");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Height")
                        .HasPrecision(18, 2)
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("LastModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Length")
                        .HasPrecision(18, 2)
                        .HasColumnType("int");

                    b.Property<decimal>("OfficialPrice")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("PrivatePrice")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid>("ShopId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Supplier")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("Weight")
                        .HasPrecision(18, 2)
                        .HasColumnType("int");

                    b.Property<int>("Width")
                        .HasPrecision(18, 2)
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ShopId");

                    b.ToTable("ShopPricePlan");
                });

            modelBuilder.Entity("GHSTShipping.Domain.Entities.Order", b =>
                {
                    b.HasOne("GHSTShipping.Domain.Entities.Shop", "Shop")
                        .WithMany("Orders")
                        .HasForeignKey("ShopId");

                    b.Navigation("Shop");
                });

            modelBuilder.Entity("GHSTShipping.Domain.Entities.OrderItem", b =>
                {
                    b.HasOne("GHSTShipping.Domain.Entities.Order", "Order")
                        .WithMany("Items")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");
                });

            modelBuilder.Entity("GHSTShipping.Domain.Entities.ShopPartnerConfig", b =>
                {
                    b.HasOne("GHSTShipping.Domain.Entities.PartnerConfig", "PartnerConfig")
                        .WithMany()
                        .HasForeignKey("PartnerConfigId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GHSTShipping.Domain.Entities.Shop", "Shop")
                        .WithMany("ShopPartnerConfigs")
                        .HasForeignKey("ShopId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PartnerConfig");

                    b.Navigation("Shop");
                });

            modelBuilder.Entity("GHSTShipping.Domain.Entities.ShopPricePlan", b =>
                {
                    b.HasOne("GHSTShipping.Domain.Entities.Shop", "Shop")
                        .WithMany("ShopPricePlanes")
                        .HasForeignKey("ShopId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Shop");
                });

            modelBuilder.Entity("GHSTShipping.Domain.Entities.Order", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("GHSTShipping.Domain.Entities.Shop", b =>
                {
                    b.Navigation("Orders");

                    b.Navigation("ShopPartnerConfigs");

                    b.Navigation("ShopPricePlanes");
                });
#pragma warning restore 612, 618
        }
    }
}
