using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Notes.Api.Data;
using Notes.Api.Model;
using Notes.Api.Repository;
using System.Text;

namespace Notes.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<NotesDB>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConn"));


            });
            builder.Services.AddScoped<RepoNote>();
            // inject the identity => userManager , roleManager
            builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<NotesDB>();

            // check jwt token header
            builder.Services.AddAuthentication(options =>
            {
                
                options.DefaultAuthenticateScheme=JwtBearerDefaults.AuthenticationScheme; 
                options.DefaultChallengeScheme=JwtBearerDefaults.AuthenticationScheme;  //unauthorize
                options.DefaultScheme=JwtBearerDefaults.AuthenticationScheme;


            }).AddJwtBearer(options => //verified  key
            {


                options.SaveToken = true;
                options.RequireHttpsMetadata = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = true,
                    ValidAudience= builder.Configuration["JWT:Audience"],
                    ValidateIssuer = true,
                    ValidIssuer= builder.Configuration["JWT:Issuer"],
                    //important line and only enough

                    IssuerSigningKey =  new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"]))




                };

            });

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();


            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
