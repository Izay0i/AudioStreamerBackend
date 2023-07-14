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

        public virtual DbSet<Artistinfo> Artistinfos { get; set; } = null!;
        public virtual DbSet<Closedcaption> Closedcaptions { get; set; } = null!;
        public virtual DbSet<Genre> Genres { get; set; } = null!;
        public virtual DbSet<Member> Members { get; set; } = null!;
        public virtual DbSet<Memberstat> Memberstats { get; set; } = null!;
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

            modelBuilder.Entity<Artistinfo>(entity =>
            {
                entity.ToTable("artistinfo");

                entity.Property(e => e.ArtistinfoId).HasColumnName("artistinfo_id");

                entity.Property(e => e.ArtistName).HasColumnName("artist_name");

                entity.Property(e => e.Avatar)
                    .HasColumnName("avatar")
                    .HasDefaultValueSql("''::text");

                entity.Property(e => e.DateCreated)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("date_created")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasDefaultValueSql("''::text");

                entity.Property(e => e.MainSiteAddress)
                    .HasColumnName("main_site_address")
                    .HasDefaultValueSql("''::text");

                entity.Property(e => e.TracksIds)
                    .HasColumnName("tracks_ids")
                    .HasDefaultValueSql("'{}'::integer[]");
            });

            modelBuilder.Entity<Closedcaption>(entity =>
            {
                entity.HasKey(e => e.CaptionId)
                    .HasName("pk_caption_id");

                entity.ToTable("closedcaption");

                entity.Property(e => e.CaptionId).HasColumnName("caption_id");

                entity.Property(e => e.Captions)
                    .HasColumnType("jsonb")
                    .HasColumnName("captions")
                    .HasDefaultValueSql("'{}'::jsonb");

                entity.Property(e => e.DateCreated)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("date_created")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.TrackId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("track_id");

                entity.HasOne(d => d.Track)
                    .WithMany(p => p.Closedcaptions)
                    .HasForeignKey(d => d.TrackId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_track_id");
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.ToTable("genre");

                entity.Property(e => e.GenreId).HasColumnName("genre_id");

                entity.Property(e => e.DateCreated)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("date_created")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.GenreName).HasColumnName("genre_name");
            });

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

                entity.Property(e => e.DateCreated)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("date_created")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.DisplayName).HasColumnName("display_name");

                entity.Property(e => e.Email).HasColumnName("email");

                entity.Property(e => e.FollowingIds)
                    .HasColumnName("following_ids")
                    .HasDefaultValueSql("'{}'::integer[]");

                entity.Property(e => e.NameTag).HasColumnName("name_tag");

                entity.Property(e => e.Password).HasColumnName("password");

                entity.Property(e => e.Token).HasColumnName("token");
            });

            modelBuilder.Entity<Memberstat>(entity =>
            {
                entity.HasKey(e => new { e.MemberId, e.TrackId })
                    .HasName("pk_com_memstat");

                entity.ToTable("memberstats");

                entity.Property(e => e.MemberId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("member_id");

                entity.Property(e => e.TrackId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("track_id");

                entity.Property(e => e.DateCreated)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("date_created")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.GenreId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("genre_id");

                entity.Property(e => e.Rating).HasColumnName("rating");

                entity.Property(e => e.Tags)
                    .HasColumnName("tags")
                    .HasDefaultValueSql("'{}'::text[]");

                entity.Property(e => e.ViewCountsTotal)
                    .HasColumnName("view_counts_total")
                    .HasDefaultValueSql("1");

                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.Memberstats)
                    .HasForeignKey(d => d.GenreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_genre_id");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Memberstats)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_member_id");

                entity.HasOne(d => d.Track)
                    .WithMany(p => p.Memberstats)
                    .HasForeignKey(d => d.TrackId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_track_id");
            });

            modelBuilder.Entity<Playlist>(entity =>
            {
                entity.ToTable("playlist");

                entity.Property(e => e.PlaylistId).HasColumnName("playlist_id");

                entity.Property(e => e.DateCreated)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("date_created")
                    .HasDefaultValueSql("now()");

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

                entity.Property(e => e.ArtistName).HasColumnName("artist_name");

                entity.Property(e => e.ArtistinfoId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("artistinfo_id");

                entity.Property(e => e.CaptionsLength).HasColumnName("captions_length");

                entity.Property(e => e.DateCreated)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("date_created")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasDefaultValueSql("''::text");

                entity.Property(e => e.GenreId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("genre_id");

                entity.Property(e => e.HasCaptions).HasColumnName("has_captions");

                entity.Property(e => e.MemberId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("member_id");

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

                entity.Property(e => e.ViewCountsPerDay).HasColumnName("view_counts_per_day");

                entity.HasOne(d => d.Artistinfo)
                    .WithMany(p => p.Tracks)
                    .HasForeignKey(d => d.ArtistinfoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_artistinfo_id");

                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.Tracks)
                    .HasForeignKey(d => d.GenreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_genre_id");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Tracks)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_member_id");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
