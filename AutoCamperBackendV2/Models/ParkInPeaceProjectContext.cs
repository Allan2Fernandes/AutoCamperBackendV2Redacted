using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AutoCamperBackendV2.Models;

public partial class ParkInPeaceProjectContext : DbContext
{
    public ParkInPeaceProjectContext()
    {
    }

    public ParkInPeaceProjectContext(DbContextOptions<ParkInPeaceProjectContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblBooking> TblBookings { get; set; }

    public virtual DbSet<TblMessage> TblMessages { get; set; }

    public virtual DbSet<TblPrivateDiscussion> TblPrivateDiscussions { get; set; }

    public virtual DbSet<TblSpace> TblSpaces { get; set; }

    public virtual DbSet<TblSpaceImage> TblSpaceImages { get; set; }

    public virtual DbSet<TblUser> TblUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=REDACTED;database=REDACTED;user id=REDACTED;password=REDACTED;trusted_connection=true;TrustServerCertificate=True;integrated security=false;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblBooking>(entity =>
        {
            entity.HasKey(e => e.FldBookingId).HasName("PK__tbl_Book__55AFC5CA06BA8AEF");

            entity.ToTable("tbl_Booking");

            entity.Property(e => e.FldBookingId).HasColumnName("fld_BookingID");
            entity.Property(e => e.FldCancellation)
                .HasColumnType("datetime")
                .HasColumnName("fld_Cancellation");
            entity.Property(e => e.FldIsAccepted).HasColumnName("fld_IsAccepted");
            entity.Property(e => e.FldReservationEnd)
                .HasColumnType("datetime")
                .HasColumnName("fld_ReservationEnd");
            entity.Property(e => e.FldReservationStart)
                .HasColumnType("datetime")
                .HasColumnName("fld_ReservationStart");
            entity.Property(e => e.FldSpaceId).HasColumnName("fld_SpaceID");
            entity.Property(e => e.FldUserId).HasColumnName("fld_UserID");

            entity.HasOne(d => d.FldSpace).WithMany(p => p.TblBookings)
                .HasForeignKey(d => d.FldSpaceId)
                .HasConstraintName("FK__tbl_Booki__fld_S__3D5E1FD2");

            entity.HasOne(d => d.FldUser).WithMany(p => p.TblBookings)
                .HasForeignKey(d => d.FldUserId)
                .HasConstraintName("FK__tbl_Booki__fld_U__3C69FB99");
        });

        modelBuilder.Entity<TblMessage>(entity =>
        {
            entity.HasKey(e => e.FldMessageId).HasName("PK__tbl_Mess__ABD47A5DAB554260");

            entity.ToTable("tbl_Message");

            entity.Property(e => e.FldMessageId).HasColumnName("fld_MessageID");
            entity.Property(e => e.FldMessageDirection).HasColumnName("fld_MessageDirection");
            entity.Property(e => e.FldMessageText).HasColumnName("fld_MessageText");
            entity.Property(e => e.FldPrivateDiscussionId).HasColumnName("fld_PrivateDiscussionID");
            entity.Property(e => e.FldTimeSent)
                .HasColumnType("datetime")
                .HasColumnName("fld_TimeSent");

            entity.HasOne(d => d.FldPrivateDiscussion).WithMany(p => p.TblMessages)
                .HasForeignKey(d => d.FldPrivateDiscussionId)
                .HasConstraintName("FK__tbl_Messa__fld_P__44FF419A");
        });

        modelBuilder.Entity<TblPrivateDiscussion>(entity =>
        {
            entity.HasKey(e => e.FldPrivateDiscussionId).HasName("PK__tbl_Priv__2E07CCB14C806E75");

            entity.ToTable("tbl_PrivateDiscussion");

            entity.Property(e => e.FldPrivateDiscussionId).HasColumnName("fld_PrivateDiscussionID");
            entity.Property(e => e.FldUser1Email)
                .HasMaxLength(100)
                .HasColumnName("fld_User1Email");
            entity.Property(e => e.FldUser2Email)
                .HasMaxLength(100)
                .HasColumnName("fld_User2Email");
        });

        modelBuilder.Entity<TblSpace>(entity =>
        {
            entity.HasKey(e => e.FldSpaceId).HasName("PK__tbl_Spac__97976F94C0D7097E");

            entity.ToTable("tbl_Space");

            entity.Property(e => e.FldSpaceId).HasColumnName("fld_SpaceID");
            entity.Property(e => e.FldAddress)
                .HasMaxLength(200)
                .HasColumnName("fld_Address");
            entity.Property(e => e.FldCancellationDuration).HasColumnName("fld_CancellationDuration");
            entity.Property(e => e.FldCancellationPenalty).HasColumnName("fld_CancellationPenalty");
            entity.Property(e => e.FldElectricity).HasColumnName("fld_Electricity");
            entity.Property(e => e.FldHeight).HasColumnName("fld_Height");
            entity.Property(e => e.FldIsActive).HasColumnName("fld_IsActive");
            entity.Property(e => e.FldLatitude).HasColumnName("fld_Latitude");
            entity.Property(e => e.FldLength).HasColumnName("fld_Length");
            entity.Property(e => e.FldLongitude).HasColumnName("fld_Longitude");
            entity.Property(e => e.FldPrice).HasColumnName("fld_Price");
            entity.Property(e => e.FldSewageDisposal).HasColumnName("fld_SewageDisposal");
            entity.Property(e => e.FldUserId).HasColumnName("fld_UserID");
            entity.Property(e => e.FldWidth).HasColumnName("fld_Width");

            entity.HasOne(d => d.FldUser).WithMany(p => p.TblSpaces)
                .HasForeignKey(d => d.FldUserId)
                .HasConstraintName("FK__tbl_Space__fld_U__398D8EEE");
        });

        modelBuilder.Entity<TblSpaceImage>(entity =>
        {
            entity.HasKey(e => e.FldSpaceImagesId).HasName("PK__tbl_Spac__3DFD6F39C43E0011");

            entity.ToTable("tbl_SpaceImages");

            entity.Property(e => e.FldSpaceImagesId).HasColumnName("fld_SpaceImagesID");
            entity.Property(e => e.FldB64encoding).HasColumnName("fld_B64Encoding");
            entity.Property(e => e.FldSpaceId).HasColumnName("fld_SpaceID");

            entity.HasOne(d => d.FldSpace).WithMany(p => p.TblSpaceImages)
                .HasForeignKey(d => d.FldSpaceId)
                .HasConstraintName("FK__tbl_Space__fld_S__403A8C7D");
        });

        modelBuilder.Entity<TblUser>(entity =>
        {
            entity.HasKey(e => e.FldUserId).HasName("PK__tbl_User__C851D2E66D92ACDB");

            entity.ToTable("tbl_User");

            entity.Property(e => e.FldUserId).HasColumnName("fld_UserID");
            entity.Property(e => e.FldAdress)
                .HasMaxLength(200)
                .HasColumnName("fld_Adress");
            entity.Property(e => e.FldEmail)
                .HasMaxLength(100)
                .HasColumnName("fld_Email");
            entity.Property(e => e.FldEncryptedPassword)
                .HasMaxLength(100)
                .HasColumnName("fld_EncryptedPassword");
            entity.Property(e => e.FldIsAdmin).HasColumnName("fld_IsAdmin");
            entity.Property(e => e.FldName)
                .HasMaxLength(50)
                .HasColumnName("fld_Name");
            entity.Property(e => e.FldPhoneNumber)
                .HasMaxLength(15)
                .HasColumnName("fld_PhoneNumber");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
