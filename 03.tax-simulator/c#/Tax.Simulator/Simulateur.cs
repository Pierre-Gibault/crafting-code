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
    
    public static decimal CalculerImpotsAnnuel(
        SituationFoyer situationFoyer)
    {
        VerificationParametre(situationFoyer);

        decimal revenuAnnuel = RevenuAnnuel(situationFoyer);

        var baseQuotient = situationFoyer.SituationFamiliale == Statuts.Marie_Pacse ? 2 : 1;
        decimal quotientEnfants = 0;

        quotientEnfants = situationFoyer.NbEnfants / 2m;
        

        decimal partsFiscales = PartsFiscales(baseQuotient, quotientEnfants, revenuAnnuel, out decimal impotParPart);

        return Math.Round(impotParPart * partsFiscales, 2);
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
        if (situationFoyer.NbEnfants < 0)
        {
            throw new ArgumentException("Le nombre d'enfants ne peut pas être négatif.");
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
}