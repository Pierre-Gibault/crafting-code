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
    [Fact]
    public void TestCalculImpotsPourUnCelibataire()
    {
        var situationFamiliale = "Célibataire";
        var salaireMensuel = 2000m;
        var salaireMensuelConjoint = 0m;
        var nombreEnfants = 0;
        var expectedResult = 1515.25m;
        
        var result = Simulateur.CalculerImpotsAnnuel(situationFamiliale, salaireMensuel, salaireMensuelConjoint, nombreEnfants);
        
        result.Should().Be(expectedResult);
    }
    
    [Fact]
    public void TestLanceraUneExceptionPourUnSalaireNégatif()
    {
        var situationFamiliale = "Célibataire";
        var salaireMensuel = -2000m;
        var salaireMensuelConjoint = 0m;
        var nombreEnfants = 0;
        
        Action act = () => Simulateur.CalculerImpotsAnnuel(situationFamiliale, salaireMensuel, salaireMensuelConjoint, nombreEnfants);
        
        act.Should().Throw<ArgumentException>().WithMessage("Les salaires doivent être positifs.");
    }
    
    [Fact]
    public void TestCalculeLesImpotsPourUnCoupleMarié()
    {
        var situationFamiliale = "Marié/Pacsé";
        var salaireMensuel = 2000;
        var salaireMensuelConjoint = 2500;
        var nombreEnfants = 0;
        var expectedResult = 4043.90m;
        
        var result = Simulateur.CalculerImpotsAnnuel(situationFamiliale, salaireMensuel, salaireMensuelConjoint, nombreEnfants);
        
        result.Should().Be(expectedResult);
    }
    
    [Fact]
    public void TestLanceraUneExceptionPourUnSalaireConjointNégatif()
    {
        var situationFamiliale = "Marié/Pacsé";
        var salaireMensuel = 2000m;
        var salaireMensuelConjoint = -2500m;
        var nombreEnfants = 0;
        
        Action act = () => Simulateur.CalculerImpotsAnnuel(situationFamiliale, salaireMensuel, salaireMensuelConjoint, nombreEnfants);
        
        act.Should().Throw<InvalidDataException>().WithMessage("Les salaires doivent être positifs.");
    }
}