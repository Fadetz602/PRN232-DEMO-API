
using DemoApi.Models;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;

namespace DemoApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<NorthwindContext>(opt => opt.UseSqlServer(
                builder.Configuration.GetConnectionString("MyCnn")));
            //builder.Configuration.GetValue<String>("ConnectionStrings:MyCnn")));
            builder.Services.AddDbContext<Prn232Slot2Context>(opt => opt.UseSqlServer(
           builder.Configuration.GetValue<String>("ConnectionStrings01:Student")));
            builder.Services.AddControllers().AddOData(otp =>
            {
                otp.Select().Filter().OrderBy().Expand().Count().SetMaxTop(100);
            });

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
}
