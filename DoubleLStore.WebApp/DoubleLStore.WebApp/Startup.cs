using DoubleLStore.WebApp.EF;
using DoubleLStore.WebApp.IService;
using DoubleLStore.WebApp.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace DoubleLStore.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.AddTransient<IMailService, MailService>();
            //services.AddTransient<IJwtAuthenticationManager, JwtAuthenticationManager>();
            services.AddDbContext<doubleLStoreDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DoubelLStoreDb")));
            services.AddControllers();
            //services.AddMvc();
            //services.AddControllers().AddNewtonsoftJson(options =>
            //options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            //    );
            //https://www.youtube.com/watch?v=MBpH8sGqrMs add Cors fix loi request Chorme
            //services.AddCors(options => options.AddDefaultPolicy(builder => builder.WithOrigins("*")));
            services.AddCors();
            services.AddCors(options =>
            {
                options.AddPolicy("Allow",
                                  builder =>
                                  {
                                      builder.WithOrigins("http://localhost:3000", "http://localhost",
                                          "http://localhost:3001", "https://lmsg03.herokuapp.com")
                                            .AllowAnyHeader()
                                            .AllowCredentials()
                                            .AllowAnyMethod();
                                      builder.AllowAnyHeader();
                                      builder.AllowAnyMethod();
                                      builder.AllowCredentials();
                                      //builder.AllowAnyOrigin();
                                      builder.SetIsOriginAllowedToAllowWildcardSubdomains();
                                      builder.SetIsOriginAllowed(_ => true);
                                  });
            });

            var key = "daylakeydaylakeydaylakeydaylakeydaylakey";
            // su dung JWTService --> chuyen sang sai CustomAuthentication
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            // cusstom authentica
            //services.AddAuthentication("Basic").AddScheme<BasicAuthenticationOptions,CustomAuthenticationHandler>("Basic",null);

            services.AddSingleton<IJwtAuthenticationManager>(new JwtAuthenticationManager(key)); //--> chuyen sang sai customAuthentication
            //services.AddSingleton<ICustomAuthenticationManager, CustomAuthenticationManager>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DoubleLStore", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DoubleLStore v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("Allow");

            app.UseCors(builder => builder
                //.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(_ => true)
                .AllowCredentials());
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();


            });



        }
    }
}
