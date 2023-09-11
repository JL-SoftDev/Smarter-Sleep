using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Models;

public partial class postgresContext : DbContext
{
    public postgresContext()
    {
    }

    public postgresContext(DbContextOptions<postgresContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AppUser> AppUsers { get; set; }

    public virtual DbSet<Challenge> Challenges { get; set; }

    public virtual DbSet<ChallengeLog> ChallengeLogs { get; set; }

    public virtual DbSet<Device> Devices { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<PurchaseLog> PurchaseLogs { get; set; }

    public virtual DbSet<SleepReview> SleepReviews { get; set; }

    public virtual DbSet<Survey> Surveys { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<WearableData> WearableData { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("name=ConnectionStrings:local");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("app_user_pkey");

            entity.ToTable("app_user");

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("user_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Points).HasColumnName("points");
            entity.Property(e => e.Username)
                .HasMaxLength(20)
                .HasColumnName("username");
        });

        modelBuilder.Entity<Challenge>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("challenge_pkey");

            entity.ToTable("challenge");

            entity.HasIndex(e => e.Name, "challenge_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Reward).HasColumnName("reward");
        });

        modelBuilder.Entity<ChallengeLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("challenge_log_pkey");

            entity.ToTable("challenge_log");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ChallengeId).HasColumnName("challenge_id");
            entity.Property(e => e.TransactionId).HasColumnName("transaction_id");

            entity.HasOne(d => d.Challenge).WithMany(p => p.ChallengeLogs)
                .HasForeignKey(d => d.ChallengeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("challenge_log_challenge_id_fkey");

            entity.HasOne(d => d.Transaction).WithMany(p => p.ChallengeLogs)
                .HasForeignKey(d => d.TransactionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("challenge_log_transaction_id_fkey");
        });

        modelBuilder.Entity<Device>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("device_pkey");

            entity.ToTable("device");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Ip)
                .HasColumnType("character varying")
                .HasColumnName("ip");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Port).HasColumnName("port");
            entity.Property(e => e.Status)
                .HasColumnType("character varying")
                .HasColumnName("status");
            entity.Property(e => e.Type)
                .HasColumnType("character varying")
                .HasColumnName("type");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Devices)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("device_user_id_fkey");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("item_pkey");

            entity.ToTable("item");

            entity.HasIndex(e => e.Name, "item_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cost).HasColumnName("cost");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
        });

        modelBuilder.Entity<PurchaseLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("purchase_log_pkey");

            entity.ToTable("purchase_log");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ItemId).HasColumnName("item_id");
            entity.Property(e => e.TransactionId).HasColumnName("transaction_id");

            entity.HasOne(d => d.Item).WithMany(p => p.PurchaseLogs)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("purchase_log_item_id_fkey");

            entity.HasOne(d => d.Transaction).WithMany(p => p.PurchaseLogs)
                .HasForeignKey(d => d.TransactionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("purchase_log_transaction_id_fkey");
        });

        modelBuilder.Entity<SleepReview>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sleep_review_pkey");

            entity.ToTable("sleep_review");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.SmarterSleepScore).HasColumnName("smarter_sleep_score");
            entity.Property(e => e.SurveyId).HasColumnName("survey_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.WearableLogId).HasColumnName("wearable_log_id");

            entity.HasOne(d => d.Survey).WithMany(p => p.SleepReviews)
                .HasForeignKey(d => d.SurveyId)
                .HasConstraintName("sleep_review_survey_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.SleepReviews)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("sleep_review_user_id_fkey");

            entity.HasOne(d => d.WearableLog).WithMany(p => p.SleepReviews)
                .HasForeignKey(d => d.WearableLogId)
                .HasConstraintName("sleep_review_wearable_log_id_fkey");
        });

        modelBuilder.Entity<Survey>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("survey_pkey");

            entity.ToTable("survey");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.SleepDuration).HasColumnName("sleep_duration");
            entity.Property(e => e.SleepQuality).HasColumnName("sleep_quality");
            entity.Property(e => e.SurveyDate).HasColumnName("survey_date");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("transaction_pkey");

            entity.ToTable("transaction");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("description");
            entity.Property(e => e.PointAmount).HasColumnName("point_amount");
            entity.Property(e => e.Timestamp)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("timestamp");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("transaction_user_id_fkey");
        });

        modelBuilder.Entity<WearableData>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("wearable_data_pkey");

            entity.ToTable("wearable_data");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Hynogram)
                .HasColumnType("character varying")
                .HasColumnName("hynogram");
            entity.Property(e => e.SleepDate).HasColumnName("sleep_date");
            entity.Property(e => e.SleepEnd)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("sleep_end");
            entity.Property(e => e.SleepScore).HasColumnName("sleep_score");
            entity.Property(e => e.SleepStart)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("sleep_start");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    public DbSet<WebApi.Models.DailyStreak> DailyStreak { get; set; } = default!;

    public DbSet<WebApi.Models.SleepSetting> SleepSetting { get; set; } = default!;
}
