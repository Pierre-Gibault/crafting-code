using Tax.Simulator;
using Tax.Simulator.Api;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.MapGet("/api/tax/calculate",
        (string situationFamiliale, decimal salaireMensuel, decimal salaireMensuelConjoint, int nombreEnfants) =>
        {
            try
            {
                Statuts.TryParse(situationFamiliale, out Statuts statuts);
                return Results.Ok(
                    Simulateur.CalculerImpotsAnnuel(
                        statuts,
                        salaireMensuel,
                        salaireMensuelConjoint,
                        nombreEnfants)
                );
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        })
    .WithName("CalculateTax");

await app.RunAsync();

public partial class Program;