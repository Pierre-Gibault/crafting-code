namespace Tax.Simulator;

public static class Simulateur
{
    private static readonly Dictionary<decimal, decimal> TranchesTauxImposition = new Dictionary<decimal, decimal>
    {
        { 10225m, 0.0m },
        { 26070m, 0.11m },
        { 74545m, 0.30m },
        { 160336m, 0.41m },
        { 500000m, 0.45m },
        { decimal.MaxValue, 0.48m }
    };

    public static Result<decimal> CalculerImpotsAnnuel(SituationFoyer situationFoyer)
    {
        var verificationResult = VerificationParametre(situationFoyer);
        if (!verificationResult.IsSuccess)
        {
            return Result<decimal>.Failure(verificationResult.Error);
        }

        decimal revenuAnnuel = RevenuAnnuel(situationFoyer);

        var baseQuotient = situationFoyer.SituationFamiliale == Statuts.Marie_Pacse ? 2 : 1;

        decimal quotientEnfants = situationFoyer.NbEnfants / 2m;

        decimal partsFiscales = PartsFiscales(baseQuotient, quotientEnfants, revenuAnnuel, out decimal impotParPart);

        return Result<decimal>.Success(Math.Round(impotParPart * partsFiscales, 2));
    }

    private static decimal PartsFiscales(int baseQuotient, decimal quotientEnfants, decimal revenuAnnuel, out decimal impotParPart)
    {
        var partsFiscales = baseQuotient + quotientEnfants;
        var revenuImposableParPart = revenuAnnuel / partsFiscales;

        impotParPart = TranchesTauxImposition
            .Select((kvp, index) => new
            {
                LowerBound = index > 0 ? TranchesTauxImposition.ElementAt(index - 1).Key : 0,
                UpperBound = kvp.Key,
                Rate = kvp.Value
            })
            .Where(t => revenuImposableParPart > t.LowerBound)
            .Sum(t => (Math.Min(revenuImposableParPart, t.UpperBound) - t.LowerBound) * t.Rate);

        return partsFiscales;
    }

    private static Result<bool> VerificationParametre(SituationFoyer situationFoyer)
    {
        if (situationFoyer.SituationFamiliale != Statuts.Celibataire && situationFoyer.SituationFamiliale != Statuts.Marie_Pacse)
        {
            return Result<bool>.Failure("Situation familiale invalide.");
        }

        if (situationFoyer.SalaireMensuel <= 0)
        {
            return Result<bool>.Failure("Les salaires doivent être positifs.");
        }

        if (situationFoyer.SituationFamiliale == Statuts.Marie_Pacse && situationFoyer.SalaireMensuelConjoint < 0)
        {
            return Result<bool>.Failure("Les salaires doivent être positifs.");
        }
        if (situationFoyer.NbEnfants < 0)
        {
            return Result<bool>.Failure("Le nombre d'enfants ne peut pas être négatif.");
        }

        return Result<bool>.Success(true);
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
}