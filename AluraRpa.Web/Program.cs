using AluraRpa.Application.Services;
using AluraRpa.Domain.Interfaces;
using AluraRpa.Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Adiciona servi�os ao cont�iner.
builder.Services.AddControllersWithViews();

// Configura��o das depend�ncias
builder.Services.AddScoped<ICursoRepository, CursoRepository>();
builder.Services.AddScoped<CursoService>();

var app = builder.Build();

// Configura o pipeline de requisi��es HTTP.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Executa a aplica��o
app.Run();
