namespace Famliy.Finance.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class BankModel : DbContext
    {
        public BankModel()
            : base("name=BankModel")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public virtual DbSet<bank_account> bank_accounts { get; set; }
        public virtual DbSet<bank_family> bank_familys { get; set; }
        public virtual DbSet<bank_family_user> bank_family_users { get; set; }
        public virtual DbSet<bank_operate_log> bank_operate_logs { get; set; }
        public virtual DbSet<bank_record> bank_records { get; set; }
        public virtual DbSet<sys_log> sys_logs { get; set; }
        public virtual DbSet<sys_permission> sys_permissions { get; set; }
        public virtual DbSet<sys_role> sys_roles { get; set; }
        public virtual DbSet<sys_role_permission> sys_role_permissions { get; set; }
        public virtual DbSet<sys_subject> sys_subjects { get; set; }
        public virtual DbSet<sys_user> sys_users { get; set; }
        public virtual DbSet<sys_user_log> sys_user_logs { get; set; }
        public virtual DbSet<sys_user_role> sys_user_roles { get; set; }
        public virtual DbSet<report_family> report_familys { get; set; }
        public virtual DbSet<report_family_subject_day> report_family_subject_days { get; set; }
        public virtual DbSet<report_family_day> report_family_days { get; set; }
        public virtual DbSet<report_family_user> report_family_users { get; set; }
        public virtual DbSet<report_user> report_users { get; set; }
        public virtual DbSet<report_user_day> report_user_days { get; set; }
        public virtual DbSet<report_user_subject_day> report_user_subject_days { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<bank_family>()
               .Property(e => e.assets_money)
               .HasPrecision(30, 8);
            modelBuilder.Entity<bank_family>()
                .Property(e => e.assets_debt)
                .HasPrecision(30, 8);
            modelBuilder.Entity<bank_family>()
                .Property(e => e.assets_investment)
                .HasPrecision(30, 8);
            modelBuilder.Entity<bank_family>()
                .Property(e => e.assets_net)
                .HasPrecision(30, 8);
            modelBuilder.Entity<bank_family>()
                .Property(e => e.assets_real)
                .HasPrecision(30, 8);
            modelBuilder.Entity<bank_family>()
                .Property(e => e.assets_total)
                .HasPrecision(30, 8);

            modelBuilder.Entity<bank_account>()
                .Property(e => e.money)
                .HasPrecision(30, 8);

            modelBuilder.Entity<bank_account>()
                .Property(e => e.debt)
                .HasPrecision(30, 8);

            modelBuilder.Entity<bank_account>()
                .Property(e => e.freezing)
                .HasPrecision(30, 8);

            modelBuilder.Entity<bank_operate_log>()
                .Property(e => e.money)
                .HasPrecision(30, 8);

            modelBuilder.Entity<bank_operate_log>()
                .Property(e => e.history_total)
                .HasPrecision(30, 8);

            modelBuilder.Entity<bank_operate_log>()
                .Property(e => e.order_amount)
                .HasPrecision(30, 8);

            modelBuilder.Entity<bank_record>()
                .Property(e => e.opening_balance)
                .HasPrecision(30, 8);

            modelBuilder.Entity<bank_record>()
                .Property(e => e.closed_balance)
                .HasPrecision(30, 8);

            modelBuilder.Entity<bank_record>()
                .Property(e => e.balance)
                .HasPrecision(30, 8);

            modelBuilder.Entity<sys_log>()
                .Property(e => e.mem)
                .IsUnicode(false);
            modelBuilder.Entity<report_family>()
               .Property(e => e.total)
               .HasPrecision(38, 8);

            modelBuilder.Entity<report_family_subject_day>()
                .Property(e => e.total)
                .HasPrecision(38, 8);

            modelBuilder.Entity<report_family_subject_day>()
                .Property(e => e.dt)
                .IsUnicode(false);

            modelBuilder.Entity<report_user>()
                .Property(e => e.total)
                .HasPrecision(38, 8);

            modelBuilder.Entity<report_user_day>()
                .Property(e => e.total)
                .HasPrecision(38, 8);

            modelBuilder.Entity<report_user_day>()
                .Property(e => e.dt)
                .IsUnicode(false);

            modelBuilder.Entity<report_user_day>()
                .Property(e => e.total)
                .HasPrecision(38, 8);

            modelBuilder.Entity<report_user_subject_day>()
                         .Property(e => e.total)
                         .HasPrecision(38, 8);

            modelBuilder.Entity<report_user_subject_day>()
                .Property(e => e.dt)
                .IsUnicode(false);
        }
    }
}
