using apiCripto_tp.Data;
using static apiCripto_tp.Services.CriptoYaPrecio;
using Microsoft.EntityFrameworkCore;


internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        builder.Services.AddControllers();


        _ = builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddHttpClient<ICriptoYaService, CriptoYaService>();

        const string PoliticaCors = "PermitirFrontend";
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(PoliticaCors, policy =>
                policy.WithOrigins("http://localhost:5173", "http://127.0.0.1:5173")
                      .AllowAnyHeader()
                      .AllowAnyMethod());
        });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            db.Database.Migrate();
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors(PoliticaCors);
        app.MapControllers();

        app.Run();
    }
}