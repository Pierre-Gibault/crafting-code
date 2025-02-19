using Xunit;

namespace Tax.Simulator.Tests;

public class SituationFoyerShould
{
    [Fact]
    public void TestCalculImpotsPourUnCelibataire()
    {
        var situationFoyer = SituationFoyer.InstantiateSituationFoyer(Statuts.Celibataire, 2000, 0, 0);
        
        situationFoyer.situationFamiliale.Should().Be(Statuts.Celibataire);
        situationFoyer.salaireMensuel.Should().Be(2000);
        situationFoyer.salaireMensuelConjoint.Should().Be(0);
        situationFoyer.nbEnfants.Should().Be(0);
    }
}
