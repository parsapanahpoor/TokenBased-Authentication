using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text;
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

        #region Api Versioning

        //کانفیگ ورژن بندی
        builder.Services.AddApiVersioning(Options =>
        {
            Options.AssumeDefaultVersionWhenUnspecified = true;//ای پی آی های قبلی باورژن دیفالت ست بشوند
            Options.DefaultApiVersion = new ApiVersion(1, 0);//معرفی ورژن دیفالت
            Options.ReportApiVersions = true; //افزودن اطلاعات ورژن جاری به هدر جواب درخواست کاربر
        });

        #endregion

        #region Swagger

        builder.Services.AddSwaggerGen(c =>
        {
            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "TokenBased-Authentication.xml"), true);

            c.SwaggerDoc("v1", new OpenApiInfo { Title = "TokenBased-Authentication", Version = "v1" });
            c.SwaggerDoc("v2", new OpenApiInfo { Title = "TokenBased-Authentication", Version = "v2" });

            //نمایش اطلاعات هر ورژن بصورت جداگانه
            c.DocInclusionPredicate((doc, apiDescription) =>
            {
                if (!apiDescription.TryGetMethodInfo(out MethodInfo methodInfo)) return false;

                var version = methodInfo.DeclaringType
                    .GetCustomAttributes<ApiVersionAttribute>(true)
                    .SelectMany(attr => attr.Versions);

                return version.Any(v => $"v{v.ToString()}" == doc);
            });

            var security = new OpenApiSecurityScheme
            {
                Name = "JWT Auth",
                Description = "توکن خود را وارد کنید- دقت کنید فقط توکن را وارد کنید",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };
            c.AddSecurityDefinition(security.Reference.Id, security);
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { security , new string[]{ } }
                });

        });


        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        #endregion

        #region  Authentication

        //Add and Set JWT for Authentication of this Project
        builder.Services.AddAuthentication(Options =>
        {
            Options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            Options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            Options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        //JWT Configurations
        .AddJwtBearer(configureOptions =>
        {
            //با قوانین زیر توکن هارا ولیدیت میکند-اطلاعاتی که در کنترلر وارد شده است احراز میگردد
            configureOptions.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidIssuer = builder.Configuration["JWtConfig:issuer"],
                ValidAudience = builder.Configuration["JWtConfig:audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWtConfig:Key"])),
                ValidateIssuerSigningKey = true,//امضای توکن را چک میکند
                ValidateLifetime = true,//توکن هایی که تایم آن ها گذشته است را رد میکند
            };

            configureOptions.SaveToken = true; // HttpContext.GetTokenAsunc(); با این دستور میتوانید هرجایی توکن رو بدست بیاورید

            //لیست ایونت هایی که میتوانید از آن ها استفاده کنید
            configureOptions.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context => { return Task.CompletedTask; },

                OnChallenge = context => { return Task.CompletedTask; },

                OnMessageReceived = context => { return Task.CompletedTask; },

                OnForbidden = context => { return Task.CompletedTask; },

                OnTokenValidated = context =>
                {
                    //var tokenValidatorService = context.HttpContext.RequestServices.GetRequiredService<ITokenValidator>();
                    //return tokenValidatorService.Execute(context);

                    return Task.CompletedTask;
                },
            };

        });

        #endregion

        #endregion

        #region Middlewares

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            //بااستفاده از رفلکشن تمام ای پی آی های مان را پیدا میکند
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.SwaggerEndpoint("/swagger/v2/swagger.json", "v2");
            });
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();

        #endregion
    }
}
