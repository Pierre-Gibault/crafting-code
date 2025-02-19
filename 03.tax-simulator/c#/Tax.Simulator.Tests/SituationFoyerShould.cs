using FluentAssertions;
using Xunit;

namespace Tax.Simulator.Tests;

public class SituationFoyerShould
{
    [Fact]
    public void TestCalculImpotsPourUnCelibataire()
    {
        var situationFoyer = SituationFoyer.InstantiateSituationFoyer(Statuts.Celibataire, 2000m, 0m, 0);
        
        situationFoyer.SituationFamiliale.Should().Be(Statuts.Celibataire);
        situationFoyer.SalaireMensuel.Should().Be(2000);
        situationFoyer.SalaireMensuelConjoint.Should().Be(0);
        situationFoyer.NbEnfants.Should().Be(0);
    }
}
