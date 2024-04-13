using Microsoft.EntityFrameworkCore;
using TokenBased_Authentication.Infrastructure.ApplicationDbContext;
namespace TokenBased_Authentication.Presentation;

public class Program
{
    public static void Main(string[] args)
    {
        #region Services

        var builder = WebApplication.CreateBuilder(args);

        #region MVC

        builder.Services.AddControllers();

        #endregion

        #region Add DBContext

        builder.Services.AddDbContext<TokenBased_AuthenticationDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("TokenBased_AuthenticationDbContextConnection"));
        });

        #endregion

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        #endregion



        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
