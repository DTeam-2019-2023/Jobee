//using System;
//using System.Collections.Generic;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata;

//namespace Jobee_API.Entities
//{
//    public partial class Project_JobeeContext : DbContext
//    {
//        public Project_JobeeContext()
//        {
//        }

//        public Project_JobeeContext(DbContextOptions<Project_JobeeContext> options)
//            : base(options)
//        {
//        }

//        public virtual DbSet<Activity> Activities { get; set; } = null!;
//        public virtual DbSet<Award> Awards { get; set; } = null!;
//        public virtual DbSet<Certificate> Certificates { get; set; } = null!;
//        public virtual DbSet<Education> Educations { get; set; } = null!;
//        public virtual DbSet<Project> Projects { get; set; } = null!;
//        public virtual DbSet<TbAccount> TbAccounts { get; set; } = null!;
//        public virtual DbSet<TbAdmin> TbAdmins { get; set; } = null!;
//        public virtual DbSet<TbCv> TbCvs { get; set; } = null!;
//        public virtual DbSet<TbEmployee> TbEmployees { get; set; } = null!;
//        public virtual DbSet<TbForgotPwd> TbForgotPwds { get; set; } = null!;
//        public virtual DbSet<TbProfile> TbProfiles { get; set; } = null!;
//        public virtual DbSet<TbTypeAccount> TbTypeAccounts { get; set; } = null!;

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseSqlServer("Data Source = localhost; Initial Catalog = Project_Jobee; User ID = sa; Password=123456 ; integrated security = True; Encrypt=False");
//            }
//        }

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            modelBuilder.Entity<Activity>(entity =>
//            {
//                entity.ToTable("Activity");

//                entity.Property(e => e.Id)
//                    .HasMaxLength(64)
//                    .HasColumnName("ID");

//                entity.Property(e => e.EndDate).HasColumnType("date");

//                entity.Property(e => e.Idcv)
//                    .HasMaxLength(64)
//                    .HasColumnName("IDCV");

//                entity.Property(e => e.StartDate).HasColumnType("date");

//                entity.HasOne(d => d.IdcvNavigation)
//                    .WithMany(p => p.Activities)
//                    .HasForeignKey(d => d.Idcv)
//                    .OnDelete(DeleteBehavior.ClientSetNull)
//                    .HasConstraintName("FK_Activity_tbCV");
//            });

//            modelBuilder.Entity<Award>(entity =>
//            {
//                entity.ToTable("Award");

//                entity.Property(e => e.Id)
//                    .HasMaxLength(64)
//                    .HasColumnName("ID");

//                entity.Property(e => e.EndDate).HasColumnType("date");

//                entity.Property(e => e.Idcv)
//                    .HasMaxLength(64)
//                    .HasColumnName("IDCV");

//                entity.Property(e => e.StartDate).HasColumnType("date");

//                entity.HasOne(d => d.IdcvNavigation)
//                    .WithMany(p => p.Awards)
//                    .HasForeignKey(d => d.Idcv)
//                    .OnDelete(DeleteBehavior.ClientSetNull)
//                    .HasConstraintName("FK_Award_tbCV");
//            });

//            modelBuilder.Entity<Certificate>(entity =>
//            {
//                entity.ToTable("Certificate");

//                entity.Property(e => e.Id)
//                    .HasMaxLength(64)
//                    .HasColumnName("ID");

//                entity.Property(e => e.EndDate).HasColumnType("date");

//                entity.Property(e => e.Idcv)
//                    .HasMaxLength(64)
//                    .HasColumnName("IDCV");

//                entity.Property(e => e.StartDate).HasColumnType("date");

//                entity.HasOne(d => d.IdcvNavigation)
//                    .WithMany(p => p.Certificates)
//                    .HasForeignKey(d => d.Idcv)
//                    .OnDelete(DeleteBehavior.ClientSetNull)
//                    .HasConstraintName("FK_Certificate_tbCV");
//            });

//            modelBuilder.Entity<Education>(entity =>
//            {
//                entity.ToTable("Education");

//                entity.Property(e => e.Id)
//                    .HasMaxLength(64)
//                    .HasColumnName("ID");

//                entity.Property(e => e.EndDate).HasColumnType("date");

//                entity.Property(e => e.Gpa).HasColumnName("GPA");

//                entity.Property(e => e.Idcv)
//                    .HasMaxLength(64)
//                    .HasColumnName("IDCV");

//                entity.Property(e => e.StartDate).HasColumnType("date");

//                entity.HasOne(d => d.IdcvNavigation)
//                    .WithMany(p => p.Educations)
//                    .HasForeignKey(d => d.Idcv)
//                    .OnDelete(DeleteBehavior.ClientSetNull)
//                    .HasConstraintName("FK_Education_tbCV");
//            });

//            modelBuilder.Entity<Project>(entity =>
//            {
//                entity.ToTable("Project");

//                entity.Property(e => e.Id)
//                    .HasMaxLength(64)
//                    .HasColumnName("ID");

//                entity.Property(e => e.EndDate).HasColumnType("date");

//                entity.Property(e => e.Idcv)
//                    .HasMaxLength(64)
//                    .HasColumnName("IDCV");

//                entity.Property(e => e.StartDate).HasColumnType("date");

//                entity.HasOne(d => d.IdcvNavigation)
//                    .WithMany(p => p.Projects)
//                    .HasForeignKey(d => d.Idcv)
//                    .OnDelete(DeleteBehavior.ClientSetNull)
//                    .HasConstraintName("FK_Project_tbCV");
//            });

//            modelBuilder.Entity<TbAccount>(entity =>
//            {
//                entity.ToTable("tbAccount");

//                entity.Property(e => e.Id)
//                    .HasMaxLength(64)
//                    .HasColumnName("ID");

//                entity.Property(e => e.IdtypeAccount)
//                    .HasMaxLength(64)
//                    .HasColumnName("IDTypeAccount");

//                entity.HasOne(d => d.IdtypeAccountNavigation)
//                    .WithMany(p => p.TbAccounts)
//                    .HasForeignKey(d => d.IdtypeAccount)
//                    .OnDelete(DeleteBehavior.ClientSetNull)
//                    .HasConstraintName("FK_tbAccount_tbTypeAccount");
//            });

//            modelBuilder.Entity<TbAdmin>(entity =>
//            {
//                entity.ToTable("tbAdmin");

//                entity.Property(e => e.Id)
//                    .HasMaxLength(64)
//                    .HasColumnName("ID");

//                entity.Property(e => e.Idprofile)
//                    .HasMaxLength(64)
//                    .HasColumnName("IDProfile");

//                entity.HasOne(d => d.IdprofileNavigation)
//                    .WithMany(p => p.TbAdmins)
//                    .HasForeignKey(d => d.Idprofile)
//                    .OnDelete(DeleteBehavior.ClientSetNull)
//                    .HasConstraintName("FK_tbAdmin_tbProfile");
//            });

//            modelBuilder.Entity<TbCv>(entity =>
//            {
//                entity.ToTable("tbCV");

//                entity.Property(e => e.Id)
//                    .HasMaxLength(64)
//                    .HasColumnName("ID");

//                entity.Property(e => e.DesirySalary).HasColumnType("money");

//                entity.Property(e => e.Idaccount)
//                    .HasMaxLength(64)
//                    .HasColumnName("IDAccount");

//                entity.HasOne(d => d.IdaccountNavigation)
//                    .WithMany(p => p.TbCvs)
//                    .HasForeignKey(d => d.Idaccount)
//                    .OnDelete(DeleteBehavior.ClientSetNull)
//                    .HasConstraintName("FK_tbCV_tbAccount");
//            });

//            modelBuilder.Entity<TbEmployee>(entity =>
//            {
//                entity.ToTable("tbEmployee");

//                entity.Property(e => e.Id)
//                    .HasMaxLength(64)
//                    .HasColumnName("ID");

//                entity.Property(e => e.Idprofile)
//                    .HasMaxLength(64)
//                    .HasColumnName("IDProfile");

//                entity.HasOne(d => d.IdprofileNavigation)
//                    .WithMany(p => p.TbEmployees)
//                    .HasForeignKey(d => d.Idprofile)
//                    .OnDelete(DeleteBehavior.ClientSetNull)
//                    .HasConstraintName("FK_tbEmployee_tbProfile");
//            });

//            modelBuilder.Entity<TbForgotPwd>(entity =>
//            {
//                entity.ToTable("tbForgotPwd");

//                entity.Property(e => e.Id).HasColumnName("id");

//                entity.Property(e => e.ExpireDay)
//                    .HasColumnType("datetime")
//                    .HasColumnName("expire_day");

//                entity.Property(e => e.Link)
//                    .HasMaxLength(511)
//                    .IsUnicode(false)
//                    .HasColumnName("link")
//                    .IsFixedLength();

//                entity.Property(e => e.Uid)
//                    .HasMaxLength(64)
//                    .HasColumnName("uid");

//                entity.HasOne(d => d.UidNavigation)
//                    .WithMany(p => p.TbForgotPwds)
//                    .HasForeignKey(d => d.Uid)
//                    .OnDelete(DeleteBehavior.ClientSetNull)
//                    .HasConstraintName("FK_tbForgotPwd_tbAccount");
//            });

//            modelBuilder.Entity<TbProfile>(entity =>
//            {
//                entity.ToTable("tbProfile");

//                entity.Property(e => e.Id)
//                    .HasMaxLength(64)
//                    .HasColumnName("ID");

//                entity.Property(e => e.DoB).HasColumnType("datetime");

//                entity.Property(e => e.Idaccount)
//                    .HasMaxLength(64)
//                    .HasColumnName("IDAccount");

//                entity.HasOne(d => d.IdaccountNavigation)
//                    .WithMany(p => p.TbProfiles)
//                    .HasForeignKey(d => d.Idaccount)
//                    .OnDelete(DeleteBehavior.ClientSetNull)
//                    .HasConstraintName("FK_tbProfile_tbAccount");
//            });

//            modelBuilder.Entity<TbTypeAccount>(entity =>
//            {
//                entity.ToTable("tbTypeAccount");

//                entity.Property(e => e.Id)
//                    .HasMaxLength(64)
//                    .HasColumnName("ID");
//            });

//            OnModelCreatingPartial(modelBuilder);
//        }

//        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
//    }
//}
