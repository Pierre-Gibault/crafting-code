using FluentAssertions;
using Xunit;

namespace Tax.Simulator.Tests;

public class SimulateurShould
{
    [Fact(DisplayName = "Célibataire, 240 000€/an, 0 enfant -> 87308.56€")]
    public void TestPasDimpotsSurHautRevenu()
    {
        var situationFamiliale = "Célibataire";
        var salaireMensuel = 20000m;
        var salaireMensuelConjoint = 0m;
        var nombreEnfants = 0;
        
        var impots = Simulateur.CalculerImpotsAnnuel(situationFamiliale, salaireMensuel, salaireMensuelConjoint, nombreEnfants);
        
        impots.Should().Be(87308.56m);
    }
    
    [Fact(DisplayName = "Célibataire, 540 000€/an, 0 enfant -> 223508.56€")]
    public void TestImpotsSurHautRevenu()
    {
        var situationFamiliale = "Célibataire";
        var salaireMensuel = 45000m;
        var salaireMensuelConjoint = 0m;
        var nombreEnfants = 0;
        
        var impots = Simulateur.CalculerImpotsAnnuel(situationFamiliale, salaireMensuel, salaireMensuelConjoint, nombreEnfants);
        
        impots.Should().Be(223508.56m);
    }
    
    [Fact(DisplayName = "Marié/Pacsé, 25 000€/mois, 30 000€/mois, 2 enfants -> 234 925.68€")]
    public void TestImpotsSurRevenusConjoints()
    {
        var situationFamiliale = "Marié/Pacsé";
        var salaireMensuel = 25000m;
        var salaireMensuelConjoint = 30000m;
        var nombreEnfants = 2;
        
        var impots = Simulateur.CalculerImpotsAnnuel(situationFamiliale, salaireMensuel, salaireMensuelConjoint, nombreEnfants);
        
        impots.Should().Be(234925.68m);
    }
}