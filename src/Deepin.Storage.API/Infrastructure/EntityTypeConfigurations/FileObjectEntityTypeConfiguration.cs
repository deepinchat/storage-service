﻿using Deepin.Storage.API.Infrastructure.Entitites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Deepin.Storage.API.Infrastructure.EntityTypeConfigurations;
public class FileObjectEntityTypeConfiguration : IEntityTypeConfiguration<FileObject>
{
    public void Configure(EntityTypeBuilder<FileObject> builder)
    {
        builder.ToTable("file_objects");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").HasColumnType("uuid");
        builder.Property(x => x.Provider).HasColumnName("provider");
        builder.Property(x => x.Hash).HasColumnName("hash");
        builder.Property(x => x.Checksum).HasColumnName("checksum");
        builder.Property(x => x.Name).HasColumnName("name");
        builder.Property(x => x.Path).HasColumnName("path");
        builder.Property(x => x.Format).HasColumnName("format");
        builder.Property(x => x.MimeType).HasColumnName("mime_type");
        builder.Property(x => x.Length).HasColumnName("length");
        builder.Property(x => x.CreatedBy).HasColumnName("created_by");
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone");
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone");
    }
}
