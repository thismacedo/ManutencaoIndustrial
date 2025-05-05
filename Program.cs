using ManutencaoIndustrial.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Adicionar suporte ao Razor Pages
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AllowAnonymousToPage("/Login");
    options.Conventions.AllowAnonymousToPage("/Error");
});

// Configurar o DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Adicionar suporte à autenticação
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";
        options.LogoutPath = "/Logout";
        options.AccessDeniedPath = "/AccessDenied";
        options.Cookie.Name = "ManutencaoIndustrial.Cookies";
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });

// Adicionar suporte à autorização
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

var app = builder.Build();

// Configure o pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

// Seed inicial do banco de dados
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
    
    // Criar usuário admin se não existir
    if (!context.Usuarios.Any())
    {
        context.Usuarios.Add(new ManutencaoIndustrial.Models.Usuario
        {
            Nome = "Administrador",
            Email = "admin@admin.com",
            Senha = "admin123",
            Tipo = "Admin"
        });
        context.SaveChanges();
    }

    // Adicionar máquinas de exemplo se não existirem
    if (!context.Maquinas.Any())
    {
        var maquinas = new List<ManutencaoIndustrial.Models.Maquina>
        {
            new ManutencaoIndustrial.Models.Maquina
            {
                Nome = "Torno CNC",
                Setor = "Usinagem",
                CodigoInterno = "USI-001"
            },
            new ManutencaoIndustrial.Models.Maquina
            {
                Nome = "Fresadora",
                Setor = "Usinagem",
                CodigoInterno = "USI-002"
            },
            new ManutencaoIndustrial.Models.Maquina
            {
                Nome = "Prensa Hidráulica",
                Setor = "Estamparia",
                CodigoInterno = "EST-001"
            },
            new ManutencaoIndustrial.Models.Maquina
            {
                Nome = "Injetora de Plástico",
                Setor = "Injeção",
                CodigoInterno = "INJ-001"
            },
            new ManutencaoIndustrial.Models.Maquina
            {
                Nome = "Empilhadeira Elétrica",
                Setor = "Logística",
                CodigoInterno = "LOG-001"
            }
        };

        context.Maquinas.AddRange(maquinas);
        context.SaveChanges();
    }
}

app.Run();
