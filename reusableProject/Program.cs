using Microsoft.EntityFrameworkCore;
using reusableProject.Services;
using SoapCore;
using System.ServiceModel;

namespace reusableProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddMvc().AddRazorRuntimeCompilation();
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("ConStr1")));

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // SOAP service registration
            builder.Services.AddSoapCore();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<ICartService,CartService>();
            
            var app = builder.Build();

            // Correct middleware order
            app.UseRouting();

            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            // SOAP endpoint configuration
            ((IApplicationBuilder)app).UseSoapEndpoint<IUserService>("/UserService.asmx", new SoapEncoderOptions(), SoapSerializer.XmlSerializer);
            ((IApplicationBuilder)app).UseSoapEndpoint<IProductService>("/ProductService.asmx", new SoapEncoderOptions(), SoapSerializer.XmlSerializer);
            ((IApplicationBuilder)app).UseSoapEndpoint<IPaymentService>("/PaymentService.asmx", new SoapEncoderOptions(), SoapSerializer.XmlSerializer);
            ((IApplicationBuilder)app).UseSoapEndpoint<IOrderService>("/OrderService.asmx", new SoapEncoderOptions(), SoapSerializer.XmlSerializer);
            ((IApplicationBuilder)app).UseSoapEndpoint<ICartService>("/CartService.asmx", new SoapEncoderOptions(), SoapSerializer.XmlSerializer);

            app.MapControllers();

            app.Run();
        }
    }
}
