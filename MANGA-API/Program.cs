
using MANGA_API.Configurations;
using MANGA_INFRASTRUCTURE;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddSwash();
builder.RegisterEndpoints();
builder.AddMediator();
builder.Services.AddInfrastructure(builder.Configuration);
builder.AddAutoMapperConfig();
builder.AddFluentValidationConfig();


var app = builder.Build();


// Configure the HTTP request pipeline.
app.UseSwash();
app.UseHttpsRedirection();
app.ConfigureEndpoints();

app.Run();
