namespace MyFinance.Application.UseCases.BusinessUnits.DTOs;

public class BusinessUnitDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public double Balance { get; set; }
    public double Income { get; set; }
    public double Outcome { get; set; }
    public bool IsArchived { get; set; }

    public BusinessUnitDTO(
        Guid id,
        string name,
        double balance,
        double income,
        double outcome,
        bool isArchived)
    {
        Id = id;
        Name = name;
        Balance = balance;
        Income = income;
        Outcome = outcome;
        IsArchived = isArchived;
    }
}
