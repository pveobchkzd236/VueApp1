using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace VueApp1.Server.Models;

public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TNM_AI_RESULT> TNM_AI_RESULTs { get; set; }

    public virtual DbSet<TNM_RULE_DICT> TNM_RULE_DICTs { get; set; }

    public virtual DbSet<T_BZJ_JCXX> T_BZJ_JCXXes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=LDY-X5800\\SQLSERVER1;Initial Catalog=Ai_Test;Integrated Security=True;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TNM_AI_RESULT>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_AI_TNM_RESULT");

            entity.ToTable("TNM_AI_RESULT");

            entity.Property(e => e.ID)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("(newid())");
            entity.Property(e => e.CONFIDENCE_LEVEL).HasComment("AI对分期判断准确性的信心，评分从1到5，分数越高表示信心越大");
            entity.Property(e => e.Create_time)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ERROR_MESSAGE)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasComment("错误信息");
            entity.Property(e => e.FULL_AI_RESULT)
                .HasMaxLength(5000)
                .IsUnicode(false)
                .HasComment("AI返回的全文");
            entity.Property(e => e.JCXX_ID)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.M_DESC)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasComment("M分期判断依据");
            entity.Property(e => e.M_LEVEL)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("M分期结果");
            entity.Property(e => e.N_DESC)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasComment("N分期判断依据");
            entity.Property(e => e.N_LEVEL)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("N分期结果");
            entity.Property(e => e.SUGGESTION)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasComment("如果无法准确判断，请在此字段中提出你的建议");
            entity.Property(e => e.TNM_RULE_NAME)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("tnm分期标准的名称，比如胃癌，肺癌");
            entity.Property(e => e.T_DESC)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasComment("T分期判断依据");
            entity.Property(e => e.T_LEVEL)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("T分期结果");
        });

        modelBuilder.Entity<TNM_RULE_DICT>(entity =>
        {
            entity.ToTable("TNM_RULE_DICT");

            entity.Property(e => e.ID)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("(newid())");
            entity.Property(e => e.TNM_RULE_CONTENT)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasComment("Tnm分期标准内容");
            entity.Property(e => e.TNM_RULE_NAME)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("Tnm分期标准名称");
        });

        modelBuilder.Entity<T_BZJ_JCXX>(entity =>
        {
            entity.HasKey(e => e.F_ID).HasName("PK_T_JCXX");

            entity.ToTable("T_BZJ_JCXX");

            entity.Property(e => e.F_ID)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.F_AGE).HasDefaultValue(0.0);
            entity.Property(e => e.F_AI_MARK).HasComment("是否用AI分析 1-是 其他-否");
            entity.Property(e => e.F_BLH)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.F_BM)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.F_BRBH)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasDefaultValue(" ");
            entity.Property(e => e.F_BW)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.F_BZJX_ID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.F_CREATE_BY)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.F_CREATE_BY_NAME)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.F_CREATE_TIME)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.F_FHR)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.F_FHSJ).HasColumnType("datetime");
            entity.Property(e => e.F_GUID).HasMaxLength(50);
            entity.Property(e => e.F_HY)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasDefaultValue(" ");
            entity.Property(e => e.F_ICD10_BM1)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.F_ICD10_BM2)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.F_ICD10_MC1)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.F_ICD10_MC2)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.F_JXSJ)
                .HasMaxLength(3000)
                .IsUnicode(false)
                .HasDefaultValue(" ");
            entity.Property(e => e.F_LCZD)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasDefaultValue(" ");
            entity.Property(e => e.F_LCZL)
                .HasMaxLength(4000)
                .IsUnicode(false)
                .HasDefaultValue(" ");
            entity.Property(e => e.F_LWXY_SFTB)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.F_NL)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasDefaultValue(" ");
            entity.Property(e => e.F_QPJX_ID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.F_RYSJ)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasDefaultValue(" ");
            entity.Property(e => e.F_SJCL)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValue(" ");
            entity.Property(e => e.F_SJDW)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.F_SJDW_DM)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.F_SQXH)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasDefaultValue(" ");
            entity.Property(e => e.F_TSJC)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasDefaultValue(" ");
            entity.Property(e => e.F_UPDATE_BY)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.F_UPDATE_BY_NAME)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.F_UPDATE_TIME).HasColumnType("datetime");
            entity.Property(e => e.F_WHO_BH)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.F_XB)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasDefaultValue(" ");
            entity.Property(e => e.F_YW_BM)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.F_ZDGJC)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValue(" ");
            entity.Property(e => e.F_ZT)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
