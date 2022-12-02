using BlackJack.Core.Configuration;
using BlackJack.Players.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBlackJackCore(builder.Configuration);
builder.Services.AddBlackJackPlayers();

builder.Services.AddControllers();
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
app.UseAuthorization();
app.MapControllers();
app.Run();