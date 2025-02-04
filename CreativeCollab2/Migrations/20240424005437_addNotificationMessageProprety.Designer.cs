﻿// <auto-generated />
using System;
using CreativeCollab2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CreativeCollab2.Migrations
{
    [DbContext(typeof(CreativeCollabContext))]
    [Migration("20240424005437_addNotificationMessageProprety")]
    partial class addNotificationMessageProprety
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseCollation("Latin1_General_CI_AI")
                .HasAnnotation("ProductVersion", "7.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BlockedUser", b =>
                {
                    b.Property<string>("BlockingUserId")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("BlockingUserID");

                    b.Property<string>("BlockedUserId")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("BlockedUserID");

                    b.HasKey("BlockingUserId", "BlockedUserId");

                    b.HasIndex("BlockedUserId");

                    b.ToTable("BlockedUsers", (string)null);
                });

            modelBuilder.Entity("CreativeCollab2.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("date");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CoverImage")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Email")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasMaxLength(1)
                        .IsUnicode(false)
                        .HasColumnType("char(1)")
                        .IsFixedLength();

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("ProfileImage")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("CreativeCollab2.Models.ChatMessage", b =>
                {
                    b.Property<int>("MessageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("MessageID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MessageId"));

                    b.Property<bool?>("IsRead")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValueSql("((0))");

                    b.Property<DateTime?>("MessageDateTime")
                        .HasColumnType("datetime");

                    b.Property<string>("MessageText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MessageType")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("ReceiverId")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("ReceiverID");

                    b.Property<string>("SenderId")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("SenderID");

                    b.HasKey("MessageId")
                        .HasName("PK__ChatMess__C87C037CAFB67DCE");

                    b.HasIndex("ReceiverId");

                    b.HasIndex("SenderId");

                    b.ToTable("ChatMessages");
                });

            modelBuilder.Entity("CreativeCollab2.Models.Comment", b =>
                {
                    b.Property<int>("CommentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("CommentID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CommentId"));

                    b.Property<DateTime?>("CommentDate")
                        .HasColumnType("datetime");

                    b.Property<string>("CommentText")
                        .IsUnicode(false)
                        .HasColumnType("varchar(max)");

                    b.Property<int?>("ImageId")
                        .HasMaxLength(450)
                        .HasColumnType("int")
                        .HasColumnName("ImageID");

                    b.Property<bool?>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValueSql("((0))");

                    b.Property<int?>("ParentCommentId")
                        .HasMaxLength(450)
                        .HasColumnType("int")
                        .HasColumnName("ParentCommentID");

                    b.Property<int?>("PostId")
                        .HasMaxLength(450)
                        .HasColumnType("int")
                        .HasColumnName("PostID");

                    b.Property<string>("UserId")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("UserID");

                    b.HasKey("CommentId")
                        .HasName("PK__Comments__C3B4DFAA64AFAD86");

                    b.HasIndex("ImageId");

                    b.HasIndex("ParentCommentId");

                    b.HasIndex("PostId");

                    b.HasIndex("UserId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("CreativeCollab2.Models.Group", b =>
                {
                    b.Property<int>("GroupId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("GroupID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("GroupId"));

                    b.Property<string>("CoverImage")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime?>("CreatedDateTime")
                        .HasColumnType("datetime");

                    b.Property<string>("CreatorUserId")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("CreatorUserID");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GroupName")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<int?>("InterestId")
                        .HasMaxLength(450)
                        .HasColumnType("int")
                        .HasColumnName("InterestID");

                    b.Property<bool?>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValueSql("((0))");

                    b.HasKey("GroupId")
                        .HasName("PK__Groups__149AF30AB8F84F79");

                    b.HasIndex("CreatorUserId");

                    b.HasIndex("InterestId");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("CreativeCollab2.Models.Image", b =>
                {
                    b.Property<int>("ImageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ImageID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ImageId"));

                    b.Property<string>("ImageName")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<int?>("InterestId")
                        .HasMaxLength(450)
                        .HasColumnType("int")
                        .HasColumnName("InterestID");

                    b.HasKey("ImageId")
                        .HasName("PK__Images__7516F4ECFE2BF7B1");

                    b.HasIndex("InterestId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("CreativeCollab2.Models.Interest", b =>
                {
                    b.Property<int>("InterestId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("InterestID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("InterestId"));

                    b.Property<string>("InterestName")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.HasKey("InterestId")
                        .HasName("PK__Interest__20832C076664BD6D");

                    b.ToTable("Interests");
                });

            modelBuilder.Entity("CreativeCollab2.Models.Like", b =>
                {
                    b.Property<int>("LikeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("LikeID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LikeId"));

                    b.Property<DateTime?>("LikeDate")
                        .HasColumnType("datetime");

                    b.Property<int?>("PostId")
                        .HasMaxLength(450)
                        .HasColumnType("int")
                        .HasColumnName("PostID");

                    b.Property<string>("UserId")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("UserID");

                    b.HasKey("LikeId")
                        .HasName("PK__Likes__A2922CF4D5D156EA");

                    b.HasIndex("PostId");

                    b.HasIndex("UserId");

                    b.ToTable("Likes");
                });

            modelBuilder.Entity("CreativeCollab2.Models.Notification", b =>
                {
                    b.Property<int>("NotificationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("NotificationID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("NotificationId"));

                    b.Property<bool?>("IsRead")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValueSql("((0))");

                    b.Property<DateTime?>("NotificationDateTime")
                        .HasColumnType("datetime");

                    b.Property<string>("NotificationMessage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NotificationType")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<int?>("PostId")
                        .HasMaxLength(450)
                        .HasColumnType("int")
                        .HasColumnName("PostID");

                    b.Property<string>("UserId")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("UserID");

                    b.HasKey("NotificationId")
                        .HasName("PK__Notifica__20CF2E32C5B3AE46");

                    b.HasIndex("PostId");

                    b.HasIndex("UserId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("CreativeCollab2.Models.Post", b =>
                {
                    b.Property<int>("PostId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("PostID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PostId"));

                    b.Property<int?>("GroupId")
                        .HasMaxLength(450)
                        .HasColumnType("int")
                        .HasColumnName("GroupID");

                    b.Property<bool?>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValueSql("((0))");

                    b.Property<DateTime?>("PostDateTime")
                        .HasColumnType("datetime");

                    b.Property<int?>("PostImageId")
                        .HasMaxLength(450)
                        .HasColumnType("int")
                        .HasColumnName("PostImageID");

                    b.Property<string>("PostText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("UserID");

                    b.HasKey("PostId")
                        .HasName("PK__Posts__AA12603897FFE4E7");

                    b.HasIndex("GroupId");

                    b.HasIndex("PostImageId");

                    b.HasIndex("UserId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("Follower", b =>
                {
                    b.Property<string>("FollowerId")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("FollowerID");

                    b.Property<string>("FollowingId")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("FollowingID");

                    b.HasKey("FollowerId", "FollowingId");

                    b.HasIndex("FollowingId");

                    b.ToTable("Followers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("UserGroupMembership", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("UserID");

                    b.Property<int>("GroupId")
                        .HasColumnType("int")
                        .HasColumnName("GroupID");

                    b.HasKey("UserId", "GroupId");

                    b.HasIndex("GroupId");

                    b.ToTable("UserGroupMembership", (string)null);
                });

            modelBuilder.Entity("UserInterest", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("UserID");

                    b.Property<int>("InterestId")
                        .HasColumnType("int")
                        .HasColumnName("InterestID");

                    b.HasKey("UserId", "InterestId");

                    b.HasIndex("InterestId");

                    b.ToTable("UserInterests", (string)null);
                });

            modelBuilder.Entity("BlockedUser", b =>
                {
                    b.HasOne("CreativeCollab2.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("BlockedUserId")
                        .IsRequired()
                        .HasConstraintName("FK_BlockedUsers_BlockedUser");

                    b.HasOne("CreativeCollab2.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("BlockingUserId")
                        .IsRequired()
                        .HasConstraintName("FK_BlockedUsers_BlockingUser");
                });

            modelBuilder.Entity("CreativeCollab2.Models.ChatMessage", b =>
                {
                    b.HasOne("CreativeCollab2.Models.ApplicationUser", "Receiver")
                        .WithMany("ChatMessageReceivers")
                        .HasForeignKey("ReceiverId")
                        .HasConstraintName("FK_ChatMessages_Receiver");

                    b.HasOne("CreativeCollab2.Models.ApplicationUser", "Sender")
                        .WithMany("ChatMessageSenders")
                        .HasForeignKey("SenderId")
                        .HasConstraintName("FK_ChatMessages_Sender");

                    b.Navigation("Receiver");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("CreativeCollab2.Models.Comment", b =>
                {
                    b.HasOne("CreativeCollab2.Models.Image", "Image")
                        .WithMany("Comments")
                        .HasForeignKey("ImageId")
                        .HasConstraintName("FK_Comments_Image");

                    b.HasOne("CreativeCollab2.Models.Comment", "ParentComment")
                        .WithMany("InverseParentComment")
                        .HasForeignKey("ParentCommentId")
                        .HasConstraintName("FK__Comments__Parent__59063A47");

                    b.HasOne("CreativeCollab2.Models.Post", "Post")
                        .WithMany("Comments")
                        .HasForeignKey("PostId")
                        .HasConstraintName("FK__Comments__PostID__5AEE82B9");

                    b.HasOne("CreativeCollab2.Models.ApplicationUser", "User")
                        .WithMany("Comments")
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK__Comments__UserID__59FA5E80");

                    b.Navigation("Image");

                    b.Navigation("ParentComment");

                    b.Navigation("Post");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CreativeCollab2.Models.Group", b =>
                {
                    b.HasOne("CreativeCollab2.Models.ApplicationUser", "CreatorUser")
                        .WithMany("Groups")
                        .HasForeignKey("CreatorUserId")
                        .HasConstraintName("FK_CreatorUser");

                    b.HasOne("CreativeCollab2.Models.Interest", "Interest")
                        .WithMany("Groups")
                        .HasForeignKey("InterestId")
                        .HasConstraintName("FK_Groups_Interest");

                    b.Navigation("CreatorUser");

                    b.Navigation("Interest");
                });

            modelBuilder.Entity("CreativeCollab2.Models.Image", b =>
                {
                    b.HasOne("CreativeCollab2.Models.Interest", "Interest")
                        .WithMany("Images")
                        .HasForeignKey("InterestId")
                        .HasConstraintName("FK_Images_Interest");

                    b.Navigation("Interest");
                });

            modelBuilder.Entity("CreativeCollab2.Models.Like", b =>
                {
                    b.HasOne("CreativeCollab2.Models.Post", "Post")
                        .WithMany("Likes")
                        .HasForeignKey("PostId")
                        .HasConstraintName("FK_Likes_Post");

                    b.HasOne("CreativeCollab2.Models.ApplicationUser", "User")
                        .WithMany("Likes")
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_Likes_User");

                    b.Navigation("Post");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CreativeCollab2.Models.Notification", b =>
                {
                    b.HasOne("CreativeCollab2.Models.Post", "Post")
                        .WithMany("Notifications")
                        .HasForeignKey("PostId")
                        .HasConstraintName("FK_Notifications_Post");

                    b.HasOne("CreativeCollab2.Models.ApplicationUser", "User")
                        .WithMany("Notifications")
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_Notifications_User");

                    b.Navigation("Post");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CreativeCollab2.Models.Post", b =>
                {
                    b.HasOne("CreativeCollab2.Models.Group", "Group")
                        .WithMany("Posts")
                        .HasForeignKey("GroupId")
                        .HasConstraintName("FK_Posts_Group");

                    b.HasOne("CreativeCollab2.Models.Image", "PostImage")
                        .WithMany("Posts")
                        .HasForeignKey("PostImageId")
                        .HasConstraintName("FK_Posts_Images");

                    b.HasOne("CreativeCollab2.Models.ApplicationUser", "User")
                        .WithMany("Posts")
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_Posts_User");

                    b.Navigation("Group");

                    b.Navigation("PostImage");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Follower", b =>
                {
                    b.HasOne("CreativeCollab2.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("FollowerId")
                        .IsRequired()
                        .HasConstraintName("FK_Follower");

                    b.HasOne("CreativeCollab2.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("FollowingId")
                        .IsRequired()
                        .HasConstraintName("FK_Following");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("CreativeCollab2.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("CreativeCollab2.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CreativeCollab2.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("CreativeCollab2.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("UserGroupMembership", b =>
                {
                    b.HasOne("CreativeCollab2.Models.Group", null)
                        .WithMany()
                        .HasForeignKey("GroupId")
                        .IsRequired()
                        .HasConstraintName("FK_UserGroupMembership_Group");

                    b.HasOne("CreativeCollab2.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK_UserGroupMembership_User");
                });

            modelBuilder.Entity("UserInterest", b =>
                {
                    b.HasOne("CreativeCollab2.Models.Interest", null)
                        .WithMany()
                        .HasForeignKey("InterestId")
                        .IsRequired()
                        .HasConstraintName("FK_UserInterests_Interest");

                    b.HasOne("CreativeCollab2.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK_UserInterests_User");
                });

            modelBuilder.Entity("CreativeCollab2.Models.ApplicationUser", b =>
                {
                    b.Navigation("ChatMessageReceivers");

                    b.Navigation("ChatMessageSenders");

                    b.Navigation("Comments");

                    b.Navigation("Groups");

                    b.Navigation("Likes");

                    b.Navigation("Notifications");

                    b.Navigation("Posts");
                });

            modelBuilder.Entity("CreativeCollab2.Models.Comment", b =>
                {
                    b.Navigation("InverseParentComment");
                });

            modelBuilder.Entity("CreativeCollab2.Models.Group", b =>
                {
                    b.Navigation("Posts");
                });

            modelBuilder.Entity("CreativeCollab2.Models.Image", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Posts");
                });

            modelBuilder.Entity("CreativeCollab2.Models.Interest", b =>
                {
                    b.Navigation("Groups");

                    b.Navigation("Images");
                });

            modelBuilder.Entity("CreativeCollab2.Models.Post", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Likes");

                    b.Navigation("Notifications");
                });
#pragma warning restore 612, 618
        }
    }
}
