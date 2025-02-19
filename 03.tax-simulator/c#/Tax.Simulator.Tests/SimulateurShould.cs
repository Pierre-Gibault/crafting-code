using FluentAssertions;
using Xunit;

namespace Tax.Simulator.Tests;

public class SimulateurShould
{
    [Fact(DisplayName = "Célibataire, 240 000€/an, 0 enfant -> 87308.56€")]
    public void TestPasDimpotsSurHautRevenu()
    {
        var situationFamiliale = Statuts.Celibataire;
        var salaireMensuel = 20000m;
        var salaireMensuelConjoint = 0m;
        var nombreEnfants = 0;
        
        var situationFoyer = SituationFoyer.InstantiateSituationFoyer(situationFamiliale, salaireMensuel, salaireMensuelConjoint, nombreEnfants);

        var impots = Simulateur.CalculerImpotsAnnuel(situationFoyer);
        
        impots.Should().Be(87308.56m);
    }
    
    [Fact(DisplayName = "Célibataire, 540 000€/an, 0 enfant -> 223508.56€")]
    public void TestImpotsSurHautRevenu()
    {
        var situationFamiliale = Statuts.Celibataire;
        var salaireMensuel = 45000m;
        var salaireMensuelConjoint = 0m;
        var nombreEnfants = 0;
        
        var situationFoyer = SituationFoyer.InstantiateSituationFoyer(situationFamiliale, salaireMensuel, salaireMensuelConjoint, nombreEnfants);

        var impots = Simulateur.CalculerImpotsAnnuel(situationFoyer);
        
        impots.Should().Be(223508.56m);
    }

    [Fact(DisplayName = "Marié/Pacsé, 25 000€/mois, 30 000€/mois, 2 enfants -> 234 925.68€")]
    public void TestImpotsSurRevenusConjoints()
    {
        var situationFamiliale = Statuts.Marie_Pacse;
        var salaireMensuel = 25000m;
        var salaireMensuelConjoint = 30000m;
        var nombreEnfants = 2;
        var situationFoyer = SituationFoyer.InstantiateSituationFoyer(situationFamiliale, salaireMensuel, salaireMensuelConjoint, nombreEnfants);

        var impots = Simulateur.CalculerImpotsAnnuel(situationFoyer);

        impots.Should().Be(234925.68m);
    }
    [Fact]
    public void TestCalculImpotsPourUnCelibataire()
    {
        var situationFamiliale = Statuts.Celibataire;
        var salaireMensuel = 2000m;
        var salaireMensuelConjoint = 0m;
        var nombreEnfants = 0;
        var expectedResult = 1515.25m;
        
        var situationFoyer = SituationFoyer.InstantiateSituationFoyer(situationFamiliale, salaireMensuel, salaireMensuelConjoint, nombreEnfants);

        var result = Simulateur.CalculerImpotsAnnuel(situationFoyer);
        
        result.Should().Be(expectedResult);
    }
    
    [Fact]
    public void TestLanceraUneExceptionPourUnSalaireNégatif()
    {
        var situationFamiliale = Statuts.Celibataire;
        var salaireMensuel = -2000m;
        var salaireMensuelConjoint = 0m;
        var nombreEnfants = 0;
        
        var situationFoyer = SituationFoyer.InstantiateSituationFoyer(situationFamiliale, salaireMensuel, salaireMensuelConjoint, nombreEnfants);
        
        Action act = () => Simulateur.CalculerImpotsAnnuel(situationFoyer);
        
        act.Should().Throw<ArgumentException>().WithMessage("Les salaires doivent être positifs.");
    }
    
    [Fact]
    public void TestCalculeLesImpotsPourUnCoupleMarié()
    {
        var situationFamiliale = Statuts.Marie_Pacse;
        var salaireMensuel = 2000;
        var salaireMensuelConjoint = 2500;
        var nombreEnfants = 0;
        var expectedResult = 4043.90m;
        var situationFoyer = SituationFoyer.InstantiateSituationFoyer(situationFamiliale, salaireMensuel, salaireMensuelConjoint, nombreEnfants);

        var result = Simulateur.CalculerImpotsAnnuel(situationFoyer);
        
        result.Should().Be(expectedResult);
    }
    
    [Fact]
    public void TestLanceraUneExceptionPourUnSalaireConjointNégatif()
    {
        var situationFamiliale = Statuts.Marie_Pacse;
        var salaireMensuel = 2000m;
        var salaireMensuelConjoint = -2500m;
        var nombreEnfants = 0;
        var situationFoyer = SituationFoyer.InstantiateSituationFoyer(situationFamiliale, salaireMensuel, salaireMensuelConjoint, nombreEnfants);

        Action act = () => Simulateur.CalculerImpotsAnnuel(situationFoyer);
        
        act.Should().Throw<InvalidDataException>().WithMessage("Les salaires doivent être positifs.");
    }
    
    [Fact]
    public void TestCalculateTaxForMarriedCoupleWithChildren()
    {
        var situationFamiliale = Statuts.Marie_Pacse;
        var salaireMensuel = 3000m;
        var salaireMensuelConjoint = 3000m;
        var nombreEnfants = 3;
        var situationFoyer = SituationFoyer.InstantiateSituationFoyer(situationFamiliale, salaireMensuel, salaireMensuelConjoint, nombreEnfants);

        decimal act = Simulateur.CalculerImpotsAnnuel(situationFoyer);
        
        act.Should().Be(3983.37m);
    }
    
    [Fact]
    public void TestLanceraUneExceptionPourUnNombreDenfantsNégatif()
    {
        var situationFamiliale = Statuts.Marie_Pacse;
        var salaireMensuel = 2000m;
        var salaireMensuelConjoint = 2000m;
        var nombreEnfants = -1;
        var situationFoyer = SituationFoyer.InstantiateSituationFoyer(situationFamiliale, salaireMensuel, salaireMensuelConjoint, nombreEnfants);

        Action act = () => Simulateur.CalculerImpotsAnnuel(situationFoyer);
        
        act.Should().Throw<ArgumentException>().WithMessage("Le nombre d'enfants ne peut pas être négatif.");
    }
    
    [Fact]
    public void TestLanceraUneExceptionPourUneSituationFamilialeInvalide()
    {
        var situationFamiliale = Statuts.Divorce;
        var salaireMensuel = 2000m;
        var salaireMensuelConjoint = 0m;
        var nombreEnfants = 0;
        var situationFoyer = SituationFoyer.InstantiateSituationFoyer(situationFamiliale, salaireMensuel, salaireMensuelConjoint, nombreEnfants);

        Action act = () => Simulateur.CalculerImpotsAnnuel(situationFoyer);
        
        act.Should().Throw<ArgumentException>().WithMessage("Situation familiale invalide.");
    }
}