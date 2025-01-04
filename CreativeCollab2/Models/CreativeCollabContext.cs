using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CreativeCollab2.Models;

public partial class CreativeCollabContext : IdentityDbContext<ApplicationUser>
{
	public CreativeCollabContext()
	{
	}

	public CreativeCollabContext(DbContextOptions<CreativeCollabContext> options)
		: base(options)
	{
	}

	public virtual DbSet<ChatMessage> ChatMessages { get; set; }

	public virtual DbSet<Comment> Comments { get; set; }

	public virtual DbSet<Group> Groups { get; set; }

	public virtual DbSet<Image> Images { get; set; }

	public virtual DbSet<Interest> Interests { get; set; }

	public virtual DbSet<Like> Likes { get; set; }

	public virtual DbSet<Notification> Notifications { get; set; }

	public virtual DbSet<Post> Posts { get; set; }

	//public virtual DbSet<ApplicationUser> AppUsers { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
		=> optionsBuilder.UseSqlServer("Server=FARAHYASSER; Database=CreativeCollab; Trusted_Connection=True;TrustServerCertificate=True");

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		modelBuilder.UseCollation("Latin1_General_CI_AI");

		modelBuilder.Entity<ChatMessage>(entity =>
		{
			entity.HasKey(e => e.MessageId).HasName("PK__ChatMess__C87C037CAFB67DCE");

			entity.Property(e => e.MessageId).HasColumnName("MessageID");
			entity.Property(e => e.IsRead).HasDefaultValueSql("((0))");
			entity.Property(e => e.MessageDateTime).HasColumnType("datetime");
			entity.Property(e => e.MessageType)
				.HasMaxLength(50)
				.IsUnicode(false);
			entity.Property(e => e.ReceiverId)
				.HasMaxLength(450)
				.HasColumnName("ReceiverID");
			entity.Property(e => e.SenderId)
				.HasMaxLength(450)
				.HasColumnName("SenderID");

			entity.HasOne(d => d.Receiver).WithMany(p => p.ChatMessageReceivers)
				.HasForeignKey(d => d.ReceiverId)
				.HasConstraintName("FK_ChatMessages_Receiver");

			entity.HasOne(d => d.Sender).WithMany(p => p.ChatMessageSenders)
				.HasForeignKey(d => d.SenderId)
				.HasConstraintName("FK_ChatMessages_Sender");
		});

		modelBuilder.Entity<Comment>(entity =>
		{
			entity.HasKey(e => e.CommentId).HasName("PK__Comments__C3B4DFAA64AFAD86");

			entity.Property(e => e.CommentId).HasColumnName("CommentID");
			entity.Property(e => e.CommentDate).HasColumnType("datetime");
			entity.Property(e => e.CommentText).IsUnicode(false);
			entity.Property(e => e.ImageId)
				.HasMaxLength(450)
				.HasColumnName("ImageID");
			entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
			entity.Property(e => e.ParentCommentId)
				.HasMaxLength(450)
				.HasColumnName("ParentCommentID");
			entity.Property(e => e.PostId)
				.HasMaxLength(450)
				.HasColumnName("PostID");
			entity.Property(e => e.UserId)
				.HasMaxLength(450)
				.HasColumnName("UserID");

			entity.HasOne(d => d.Image).WithMany(p => p.Comments)
				.HasForeignKey(d => d.ImageId)
				.HasConstraintName("FK_Comments_Image");

			entity.HasOne(d => d.ParentComment).WithMany(p => p.InverseParentComment)
				.HasForeignKey(d => d.ParentCommentId)
				.HasConstraintName("FK__Comments__Parent__59063A47");

			entity.HasOne(d => d.Post).WithMany(p => p.Comments)
				.HasForeignKey(d => d.PostId)
				.HasConstraintName("FK__Comments__PostID__5AEE82B9");

			entity.HasOne(d => d.User).WithMany(p => p.Comments)
				.HasForeignKey(d => d.UserId)
				.HasConstraintName("FK__Comments__UserID__59FA5E80");
		});

		modelBuilder.Entity<Group>(entity =>
		{
			entity.HasKey(e => e.GroupId).HasName("PK__Groups__149AF30AB8F84F79");

			entity.Property(e => e.GroupId).HasColumnName("GroupID");
			entity.Property(e => e.CoverImage)
				.HasMaxLength(255)
				.IsUnicode(false);
			entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
			entity.Property(e => e.CreatorUserId)
				.HasMaxLength(450)
				.HasColumnName("CreatorUserID");
			entity.Property(e => e.GroupName)
				.HasMaxLength(100)
				.IsUnicode(false);
			entity.Property(e => e.InterestId)
				.HasMaxLength(450)
				.HasColumnName("InterestID");
			entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

			entity.HasOne(d => d.CreatorUser).WithMany(p => p.Groups)
				.HasForeignKey(d => d.CreatorUserId)
				.HasConstraintName("FK_CreatorUser");

			entity.HasOne(d => d.Interest).WithMany(p => p.Groups)
				.HasForeignKey(d => d.InterestId)
				.HasConstraintName("FK_Groups_Interest");
		});

		modelBuilder.Entity<Image>(entity =>
		{
			entity.HasKey(e => e.ImageId).HasName("PK__Images__7516F4ECFE2BF7B1");

			entity.Property(e => e.ImageId).HasColumnName("ImageID");
			entity.Property(e => e.ImageName)
				.HasMaxLength(255)
				.IsUnicode(false);
			entity.Property(e => e.InterestId)
				.HasMaxLength(450)
				.HasColumnName("InterestID");

			entity.HasOne(d => d.Interest).WithMany(p => p.Images)
				.HasForeignKey(d => d.InterestId)
				.HasConstraintName("FK_Images_Interest");
		});

		modelBuilder.Entity<Interest>(entity =>
		{
			entity.HasKey(e => e.InterestId).HasName("PK__Interest__20832C076664BD6D");

			entity.Property(e => e.InterestId).HasColumnName("InterestID");
			entity.Property(e => e.InterestName)
				.HasMaxLength(255)
				.IsUnicode(false);
		});

		modelBuilder.Entity<Like>(entity =>
		{
			entity.HasKey(e => e.LikeId).HasName("PK__Likes__A2922CF4D5D156EA");

			entity.Property(e => e.LikeId).HasColumnName("LikeID");
			entity.Property(e => e.LikeDate).HasColumnType("datetime");
			entity.Property(e => e.PostId)
				.HasMaxLength(450)
				.HasColumnName("PostID");
			entity.Property(e => e.UserId)
				.HasMaxLength(450)
				.HasColumnName("UserID");

			entity.HasOne(d => d.Post).WithMany(p => p.Likes)
				.HasForeignKey(d => d.PostId)
				.HasConstraintName("FK_Likes_Post");

			entity.HasOne(d => d.User).WithMany(p => p.Likes)
				.HasForeignKey(d => d.UserId)
				.HasConstraintName("FK_Likes_User");
		});

		modelBuilder.Entity<Notification>(entity =>
		{
			entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__20CF2E32C5B3AE46");

			entity.Property(e => e.NotificationId).HasColumnName("NotificationID");
			entity.Property(e => e.IsRead).HasDefaultValueSql("((0))");
			entity.Property(e => e.NotificationDateTime).HasColumnType("datetime");
			entity.Property(e => e.NotificationType)
				.HasMaxLength(50)
				.IsUnicode(false);
			entity.Property(e => e.PostId)
				.HasMaxLength(450)
				.HasColumnName("PostID");
			entity.Property(e => e.UserId)
				.HasMaxLength(450)
				.HasColumnName("UserID");

			entity.HasOne(d => d.Post).WithMany(p => p.Notifications)
				.HasForeignKey(d => d.PostId)
				.HasConstraintName("FK_Notifications_Post");

			entity.HasOne(d => d.User).WithMany(p => p.Notifications)
				.HasForeignKey(d => d.UserId)
				.HasConstraintName("FK_Notifications_User");
		});

		modelBuilder.Entity<Post>(entity =>
		{
			entity.HasKey(e => e.PostId).HasName("PK__Posts__AA12603897FFE4E7");

			entity.Property(e => e.PostId).HasColumnName("PostID");
			entity.Property(e => e.GroupId)
				.HasMaxLength(450)
				.HasColumnName("GroupID");
			entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
			entity.Property(e => e.PostDateTime).HasColumnType("datetime");
			entity.Property(e => e.PostImageId)
				.HasMaxLength(450)
				.HasColumnName("PostImageID");
			entity.Property(e => e.UserId)
				.HasMaxLength(450)
				.HasColumnName("UserID");

			entity.HasOne(d => d.Group).WithMany(p => p.Posts)
				.HasForeignKey(d => d.GroupId)
				.HasConstraintName("FK_Posts_Group");

			entity.HasOne(d => d.PostImage).WithMany(p => p.Posts)
				.HasForeignKey(d => d.PostImageId)
				.HasConstraintName("FK_Posts_Images");

			entity.HasOne(d => d.User).WithMany(p => p.Posts)
				.HasForeignKey(d => d.UserId)
				.HasConstraintName("FK_Posts_User");
		});

		modelBuilder.Entity<ApplicationUser>(entity =>
		{

			entity.Property(e => e.BirthDate).HasColumnType("date");
			entity.Property(e => e.CoverImage)
				.HasMaxLength(255)
				.IsUnicode(false);
			entity.Property(e => e.Email)
				.HasMaxLength(100)
				.IsUnicode(false);
			entity.Property(e => e.FirstName)
				.HasMaxLength(50)
				.IsUnicode(false);
			entity.Property(e => e.Gender)
				.HasMaxLength(1)
				.IsUnicode(false)
				.IsFixedLength();
			entity.Property(e => e.LastName)
				.HasMaxLength(50)
				.IsUnicode(false);
			entity.Property(e => e.ProfileImage)
				.HasMaxLength(255)
				.IsUnicode(false);

			entity.HasMany(d => d.BlockedUsers).WithMany(p => p.BlockingUsers)
				.UsingEntity<Dictionary<string, object>>(
					"BlockedUser",
					r => r.HasOne<ApplicationUser>().WithMany()
						.HasForeignKey("BlockedUserId")
						.OnDelete(DeleteBehavior.ClientSetNull)
						.HasConstraintName("FK_BlockedUsers_BlockedUser"),
					l => l.HasOne<ApplicationUser>().WithMany()
						.HasForeignKey("BlockingUserId")
						.OnDelete(DeleteBehavior.ClientSetNull)
						.HasConstraintName("FK_BlockedUsers_BlockingUser"),
					j =>
					{
						j.HasKey("BlockingUserId", "BlockedUserId");
						j.ToTable("BlockedUsers");
						j.IndexerProperty<string>("BlockingUserId").HasColumnName("BlockingUserID");
						j.IndexerProperty<string>("BlockedUserId").HasColumnName("BlockedUserID");
					});

			entity.HasMany(d => d.BlockingUsers).WithMany(p => p.BlockedUsers)
				.UsingEntity<Dictionary<string, object>>(
					"BlockedUser",
					r => r.HasOne<ApplicationUser>().WithMany()
						.HasForeignKey("BlockingUserId")
						.OnDelete(DeleteBehavior.ClientSetNull)
						.HasConstraintName("FK_BlockedUsers_BlockingUser"),
					l => l.HasOne<ApplicationUser>().WithMany()
						.HasForeignKey("BlockedUserId")
						.OnDelete(DeleteBehavior.ClientSetNull)
						.HasConstraintName("FK_BlockedUsers_BlockedUser"),
					j =>
					{
						j.HasKey("BlockingUserId", "BlockedUserId");
						j.ToTable("BlockedUsers");
						j.IndexerProperty<string>("BlockingUserId").HasColumnName("BlockingUserID");
						j.IndexerProperty<string>("BlockedUserId").HasColumnName("BlockedUserID");
					});

			entity.HasMany(d => d.Followers).WithMany(p => p.Followings)
				.UsingEntity<Dictionary<string, object>>(
					"Follower",
					r => r.HasOne<ApplicationUser>().WithMany()
						.HasForeignKey("FollowerId")
						.OnDelete(DeleteBehavior.ClientSetNull)
						.HasConstraintName("FK_Follower"),
					l => l.HasOne<ApplicationUser>().WithMany()
						.HasForeignKey("FollowingId")
						.OnDelete(DeleteBehavior.ClientSetNull)
						.HasConstraintName("FK_Following"),
					j =>
					{
						j.HasKey("FollowerId", "FollowingId");
						j.ToTable("Followers");
						j.IndexerProperty<string>("FollowerId").HasColumnName("FollowerID");
						j.IndexerProperty<string>("FollowingId").HasColumnName("FollowingID");
					});

			entity.HasMany(d => d.Followings).WithMany(p => p.Followers)
				.UsingEntity<Dictionary<string, object>>(
					"Follower",
					r => r.HasOne<ApplicationUser>().WithMany()
						.HasForeignKey("FollowingId")
						.OnDelete(DeleteBehavior.ClientSetNull)
						.HasConstraintName("FK_Following"),
					l => l.HasOne<ApplicationUser>().WithMany()
						.HasForeignKey("FollowerId")
						.OnDelete(DeleteBehavior.ClientSetNull)
						.HasConstraintName("FK_Follower"),
					j =>
					{
						j.HasKey("FollowerId", "FollowingId");
						j.ToTable("Followers");
						j.IndexerProperty<string>("FollowerId").HasColumnName("FollowerID");
						j.IndexerProperty<string>("FollowingId").HasColumnName("FollowingID");
					});


			entity.HasMany(d => d.GroupsNavigation).WithMany(p => p.Users)
				.UsingEntity<Dictionary<string, object>>(
					"UserGroupMembership",
					r => r.HasOne<Group>().WithMany()
						.HasForeignKey("GroupId")
						.OnDelete(DeleteBehavior.ClientSetNull)
						.HasConstraintName("FK_UserGroupMembership_Group"),
					l => l.HasOne<ApplicationUser>().WithMany()
						.HasForeignKey("UserId")
						.OnDelete(DeleteBehavior.ClientSetNull)
						.HasConstraintName("FK_UserGroupMembership_User"),
					j =>
					{
						j.HasKey("UserId", "GroupId");
						j.ToTable("UserGroupMembership");
						j.IndexerProperty<string>("UserId").HasColumnName("UserID");
						j.IndexerProperty<int>("GroupId").HasColumnName("GroupID");
					});

			entity.HasMany(d => d.Interests).WithMany(p => p.Users)
				.UsingEntity<Dictionary<string, object>>(
					"UserInterest",
					r => r.HasOne<Interest>().WithMany()
						.HasForeignKey("InterestId")
						.OnDelete(DeleteBehavior.ClientSetNull)
						.HasConstraintName("FK_UserInterests_Interest"),
					l => l.HasOne<ApplicationUser>().WithMany()
						.HasForeignKey("UserId")
						.OnDelete(DeleteBehavior.ClientSetNull)
						.HasConstraintName("FK_UserInterests_User"),
					j =>
					{
						j.HasKey("UserId", "InterestId");
						j.ToTable("UserInterests");
						j.IndexerProperty<string>("UserId").HasColumnName("UserID");
						j.IndexerProperty<int>("InterestId").HasColumnName("InterestID");
					});
		});

		OnModelCreatingPartial(modelBuilder);
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
