using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AudioStreamerAPI.Models
{
    public partial class fsnvdezgContext : DbContext
    {
        public fsnvdezgContext()
        {
        }

        public fsnvdezgContext(DbContextOptions<fsnvdezgContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Member> Members { get; set; } = null!;
        public virtual DbSet<Playlist> Playlists { get; set; } = null!;
        public virtual DbSet<Track> Tracks { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(ConnStr.Get());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("btree_gin")
                .HasPostgresExtension("btree_gist")
                .HasPostgresExtension("citext")
                .HasPostgresExtension("cube")
                .HasPostgresExtension("dblink")
                .HasPostgresExtension("dict_int")
                .HasPostgresExtension("dict_xsyn")
                .HasPostgresExtension("earthdistance")
                .HasPostgresExtension("fuzzystrmatch")
                .HasPostgresExtension("hstore")
                .HasPostgresExtension("intarray")
                .HasPostgresExtension("ltree")
                .HasPostgresExtension("pg_stat_statements")
                .HasPostgresExtension("pg_trgm")
                .HasPostgresExtension("pgcrypto")
                .HasPostgresExtension("pgrowlocks")
                .HasPostgresExtension("pgstattuple")
                .HasPostgresExtension("tablefunc")
                .HasPostgresExtension("unaccent")
                .HasPostgresExtension("uuid-ossp")
                .HasPostgresExtension("xml2");

            modelBuilder.Entity<Member>(entity =>
            {
                entity.ToTable("member");

                entity.Property(e => e.MemberId).HasColumnName("member_id");

                entity.Property(e => e.Avatar)
                    .HasColumnName("avatar")
                    .HasDefaultValueSql("''::text");

                entity.Property(e => e.Biography)
                    .HasColumnName("biography")
                    .HasDefaultValueSql("''::text");

                entity.Property(e => e.DisplayName).HasColumnName("display_name");

                entity.Property(e => e.Email).HasColumnName("email");

                entity.Property(e => e.FollowingIds)
                    .HasColumnName("following_ids")
                    .HasDefaultValueSql("'{}'::integer[]");

                entity.Property(e => e.NameTag).HasColumnName("name_tag");

                entity.Property(e => e.Password).HasColumnName("password");

                entity.Property(e => e.Token).HasColumnName("token");
            });

            modelBuilder.Entity<Playlist>(entity =>
            {
                entity.ToTable("playlist");

                entity.Property(e => e.PlaylistId).HasColumnName("playlist_id");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasDefaultValueSql("''::text");

                entity.Property(e => e.MemberId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("member_id");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasDefaultValueSql("''::text");

                entity.Property(e => e.TracksIds)
                    .HasColumnName("tracks_ids")
                    .HasDefaultValueSql("'{}'::integer[]");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Playlists)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_member_id");
            });

            modelBuilder.Entity<Track>(entity =>
            {
                entity.ToTable("track");

                entity.Property(e => e.TrackId).HasColumnName("track_id");

                entity.Property(e => e.AuthorsIds).HasColumnName("authors_ids");

                entity.Property(e => e.DateUploaded)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("date_uploaded")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasDefaultValueSql("''::text");

                entity.Property(e => e.Tags)
                    .HasColumnName("tags")
                    .HasDefaultValueSql("'{}'::text[]");

                entity.Property(e => e.Thumbnail)
                    .HasColumnName("thumbnail")
                    .HasDefaultValueSql("''::text");

                entity.Property(e => e.TrackName).HasColumnName("track_name");

                entity.Property(e => e.Url)
                    .HasColumnName("url")
                    .HasDefaultValueSql("''::text");

                entity.Property(e => e.ViewCountPerDay).HasColumnName("view_count_per_day");

                entity.Property(e => e.ViewCountTotal).HasColumnName("view_count_total");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
