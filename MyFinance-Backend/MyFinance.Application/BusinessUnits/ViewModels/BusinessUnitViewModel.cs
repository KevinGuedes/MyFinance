namespace MyFinance.Application.BusinessUnits.ViewModels
{
    public class BusinessUnitViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double CurrentBalance { get; set; }

        public BusinessUnitViewModel(Guid id, string name, double currentBalance)
        {
            Id = id;
            Name = name;
            CurrentBalance = currentBalance;
        }
    }
}
