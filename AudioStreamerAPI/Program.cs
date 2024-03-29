using AudioStreamerAPI;
using AudioStreamerAPI.Models;
using AudioStreamerAPI.Repositories;
using AudioStreamerAPI.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = AzureConstants.MAX_FILE_SIZE;
});

builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Add services to the container.
builder.Services.AddControllers(
    options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true
);

builder.Services.AddDbContext<fsnvdezgContext>(opt => opt.UseNpgsql(ConnStr.Get()));
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<IMemberstatsRepository, MemberstatsRepository>();
builder.Services.AddScoped<IFollowerRepository, FollowerRepository>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<IArtistInfoRepository, ArtistInfoRepository>();
builder.Services.AddScoped<ITrackRepository, TrackRepository>();
builder.Services.AddScoped<IPlaylistRepository, PlaylistRepository>();
builder.Services.AddScoped<ICaptionRepository, CaptionRepository>();

builder.Services.AddCors(options => options.AddPolicy(NamedConstants.CORS_POLICY_NAME, 
    policy =>
    {
        policy.WithOrigins("http://localhost:5173",
                            "https://audiostreamerweb.azurewebsites.net")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .SetIsOriginAllowed(origin => true);
    })
);

builder.Services.AddHttpClient();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config => config.EnableAnnotations());

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseDeveloperExceptionPage();

app.UseSwagger();
if (!app.Environment.IsDevelopment())
{
    app.UseSwaggerUI(config =>
    {
        config.RoutePrefix = string.Empty;
        config.SwaggerEndpoint("/swagger/v1/swagger.json", "V1 Docs");
    });
}
else
{
    app.UseSwaggerUI();
}

app.UseCors(NamedConstants.CORS_POLICY_NAME);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
