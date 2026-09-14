using Saborall.Components;
using Saborall.Configs;
using Saborall.DAO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<Conexao>();

builder.Services.AddScoped<UsuarioDAO>();
builder.Services.AddScoped<VendedorDAO>();
builder.Services.AddScoped<FornecedorDAO>();
builder.Services.AddScoped<ProdutoDAO>();
builder.Services.AddScoped<EstoqueDAO>();
builder.Services.AddScoped<VendaDAO>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
