using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NanhiDuniya.Data.Entities;


namespace NanhiDuniya.Data.Entities;

public partial class NanhiDuniyaDbContext : IdentityDbContext<ApplicationUser>
{
    public NanhiDuniyaDbContext()
    {
    }

    public NanhiDuniyaDbContext(DbContextOptions<NanhiDuniyaDbContext> options)
        : base(options)
    {
    }

   

    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
    //        => optionsBuilder.UseSqlServer("Integrated Security=SSPI;Pooling=false;MultipleActiveResultSets=true;Data Source=DESKTOP-EIN8N90;Initial Catalog=efilec2cdbstage;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);



       
       

       

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
