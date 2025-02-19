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
            Statuts statuts;
            switch (situationFamiliale)
            {
                case "Marié/Pacsé":
                    statuts = Statuts.Marie_Pacse;
                    break;
                case "Célibataire":
                    statuts = Statuts.Celibataire;
                    break;
                default:
                    statuts = Statuts.Celibataire;
                    break;
            }

            SituationFoyer situationFoyer =
                SituationFoyer.InstantiateSituationFoyer(statuts, salaireMensuel, salaireMensuelConjoint,
                    nombreEnfants);
            var result = Simulateur.CalculerImpotsAnnuel(situationFoyer);

            if (!result.IsSuccess)
            {
                return Results.BadRequest(result.Error);
            }

            return Results.Ok(result.Value);
        })
    .WithName("CalculateTax");

await app.RunAsync();

public partial class Program;