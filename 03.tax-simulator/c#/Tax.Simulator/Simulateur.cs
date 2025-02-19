namespace Tax.Simulator;

public static class Simulateur
{
    private static readonly decimal[] TranchesImposition = {10225m, 26070m, 74545m, 160336m, 500000m}; // Plafonds des tranches
    private static readonly decimal[] TauxImposition = {0.0m, 0.11m, 0.30m, 0.41m, 0.45m, 0.48m}; // Taux correspondants

    public static decimal CalculerImpotsAnnuel(
        SituationFoyer situationFoyer)
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

        if (situationFoyer.NbEnfants < 0)
        {
            throw new ArgumentException("Le nombre d'enfants ne peut pas être négatif.");
        }

        decimal revenuAnnuel;
        if (situationFoyer.SituationFamiliale == Statuts.Marie_Pacse)
        {
            revenuAnnuel = (situationFoyer.SalaireMensuel + situationFoyer.SalaireMensuelConjoint) * 12;
        }
        else
        {
            revenuAnnuel = situationFoyer.SalaireMensuel * 12;
        }

        var baseQuotient = situationFoyer.SituationFamiliale == Statuts.Marie_Pacse ? 2 : 1;
        decimal quotientEnfants = (decimal) Math.PI;

        if (situationFoyer.NbEnfants == 0)
        {
            quotientEnfants = 0;
        }
        else if (situationFoyer.NbEnfants == 1)
        {
            quotientEnfants = 0.5m;
        }
        else if (situationFoyer.NbEnfants == 2)
        {
            quotientEnfants = 1.0m;
        }
        else
        {
            quotientEnfants = 1.0m + (situationFoyer.NbEnfants - 2) * 0.5m;
        }

        var partsFiscales = baseQuotient + quotientEnfants;
        var revenuImposableParPart = revenuAnnuel / partsFiscales;

        decimal impot = 0;
        for (var i = 0; i < TranchesImposition.Length; i++)
        {
            if (revenuImposableParPart <= TranchesImposition[i])
            {
                impot += (revenuImposableParPart - (i > 0 ? TranchesImposition[i - 1] : 0)) * TauxImposition[i];
                break;
            }
            else
            {
                impot += (TranchesImposition[i] - (i > 0 ? TranchesImposition[i - 1] : 0)) * TauxImposition[i];
            }
        }

        if (revenuImposableParPart > TranchesImposition[^1])
        {
            impot += (revenuImposableParPart - TranchesImposition[^1]) * TauxImposition[^1];
        }

        var impotParPart = impot;

        return Math.Round(impotParPart * partsFiscales, 2);
    }
}