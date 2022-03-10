using Architecture.Application;
using Architecture.Database;
using Architecture.AspNetCore;
using Architecture.Security;
using Architecture;
using DotNetCore.Logging;

var builder = WebApplication.CreateBuilder();

builder.Host.UseSerilog();

builder.Services.AddHashService();

builder.Services.AddAuthenticationJwtBearer(new JwtSettings(Guid.NewGuid().ToString("N"), TimeSpan.FromHours(12)));

builder.Services.AddResponseCompression();

builder.Services.AddControllers().AddJsonOptions().AddAuthorizationPolicy();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

//builder.Services.AddSpaStaticFiles("Frontend/dist");

//builder.Services.AddContext<Context>(options => options.UseSqlServer(builder.Services.GetConnectionString(nameof(Context))));

builder.Services.AddClassesMatchingInterfaces(typeof(IUserService).Assembly, typeof(IUserRepository).Assembly);

var application = builder.Build();

application.UseException();

application.UseHttps();

application.UseRouting();

application.UseResponseCompression();

application.UseAuthentication();

application.UseAuthorization();

application.UseEndpointsMapControllers();

application.UseSwagger();

application.UseSwaggerUI();

//application.UseSpaAngular("Frontend", "start", "http://localhost:4200");

application.Run();
