namespace MyFinance.Application.BusinessUnits.ViewModels
{
    public class BusinessUnitViewModel
    {
        public BusinessUnitViewModel(Guid id, string name, double currentBalance)
        {
            Id = id;
            Name = name;
            CurrentBalance = currentBalance;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public double CurrentBalance { get; set; }
    }
}
