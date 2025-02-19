namespace Tax.Simulator;

public static class Simulateur
{
    private static readonly decimal[] TranchesImposition = {10225m, 26070m, 74545m, 160336m, 500000m}; // Plafonds des tranches
    private static readonly decimal[] TauxImposition = {0.0m, 0.11m, 0.30m, 0.41m, 0.45m, 0.48m}; // Taux correspondants

    public static decimal CalculerImpotsAnnuel(
        SituationFoyer situationFoyer)
    {
        VerificationParametre(situationFoyer);

        decimal revenuAnnuel = RevenuAnnuel(situationFoyer);

        var baseQuotient = situationFoyer.SituationFamiliale == Statuts.Marie_Pacse ? 2 : 1;
        decimal quotientEnfants = 0;

        quotientEnfants = CalculerQuotientEnfant(situationFoyer.NbEnfants);
        

        decimal partsFiscales = PartsFiscales(baseQuotient, quotientEnfants, revenuAnnuel, out decimal impotParPart);

        return Math.Round(impotParPart * partsFiscales, 2);
    }
    private static decimal PartsFiscales(int baseQuotient, decimal quotientEnfants, decimal revenuAnnuel, out decimal impotParPart)
    {
        var partsFiscales = baseQuotient + quotientEnfants;
        var revenuImposableParPart = revenuAnnuel / partsFiscales;
        
        impotParPart = TranchesImposition
            .Select((tranche, index) => new
            {
                LowerBound = index > 0 ? TranchesImposition[index - 1] : 0,
                UpperBound = tranche,
                Rate = TauxImposition[index]
            })
            .Where(t => revenuImposableParPart > t.LowerBound)
            .Sum(t => (Math.Min(revenuImposableParPart, t.UpperBound) - t.LowerBound) * t.Rate);
        
        if (revenuImposableParPart > TranchesImposition[^1])
        {
            impotParPart += (revenuImposableParPart - TranchesImposition[^1]) * TauxImposition[^1];
        }

        return partsFiscales;
    }


    private static void VerificationParametre(SituationFoyer situationFoyer)
    {

        if (situationFoyer.SituationFamiliale != Statuts.Celibataire && situationFoyer.SituationFamiliale != Statuts.Marie_Pacse)
        {
            throw new ArgumentException("Situation familiale invalide.");
        }

        if (situationFoyer.SalaireMensuel <= 0)
        {
            throw new ArgumentException("Les salaires doivent être positifs.");
        }

        if (situationFoyer.SituationFamiliale == Statuts.Marie_Pacse && situationFoyer.SalaireMensuelConjoint < 0)
        {
            throw new InvalidDataException("Les salaires doivent être positifs.");
        }
    }
    private static decimal RevenuAnnuel(SituationFoyer situationFoyer)
    {

        decimal revenuAnnuel;
        if (situationFoyer.SituationFamiliale == Statuts.Marie_Pacse)
        {
            revenuAnnuel = (situationFoyer.SalaireMensuel + situationFoyer.SalaireMensuelConjoint) * 12;
        }
        else
        {
            revenuAnnuel = situationFoyer.SalaireMensuel * 12;
        }
        return revenuAnnuel;
    }
    private static decimal CalculerQuotientEnfant(int situationFoyerNbEnfants)
    {
        if (situationFoyerNbEnfants < 0)
        {
            throw new ArgumentException("Le nombre d'enfants ne peut pas être négatif.");
        }
        return situationFoyerNbEnfants <= 2 ? situationFoyerNbEnfants * 0.5m : 1.0m + (situationFoyerNbEnfants - 2) * 0.5m;
    }
}