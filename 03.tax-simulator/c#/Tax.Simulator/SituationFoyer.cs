namespace Tax.Simulator;

public class SituationFoyer
{
    private Statuts situationFamiliale;
    private decimal salaireMensuel;
    private decimal salaireMensuelConjoint;
    private int nbEnfants;
    
    private SituationFoyer()
    {
        
    }
    
    public static SituationFoyer InstantiateSituationFoyer(Statuts situationFamiliale, decimal salaireMensuel, decimal salaireMensuelConjoint, int nbEnfants)
    {
        return new SituationFoyer
        {
            situationFamiliale = situationFamiliale,
            salaireMensuel = salaireMensuel,
            salaireMensuelConjoint = salaireMensuelConjoint,
            nbEnfants = nbEnfants
        };
    }
    
    public Statuts SituationFamiliale => situationFamiliale;
    public decimal SalaireMensuel => salaireMensuel;
    public decimal SalaireMensuelConjoint => salaireMensuelConjoint;
    public int NbEnfants => nbEnfants;
    
}
