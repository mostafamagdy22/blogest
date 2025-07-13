using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using blogest.infrastructure.Identity;

namespace blogest.infrastructure.persistence.EntitiesConfig;

public class SaveConfiguration : IEntityTypeConfiguration<Save>
{

    public void Configure(EntityTypeBuilder<Save> builder)
    {
        builder.HasKey(s => new { s.PostId, s.UserId });

        builder.HasOne(s => s.Post)
        .WithMany(p => p.Saves)
        .HasForeignKey(s => s.PostId)
        .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<AppUser>()
        .WithMany(u => u.SavedPosts)
        .HasForeignKey(s => s.UserId)
        .OnDelete(DeleteBehavior.Restrict);           
    }
}