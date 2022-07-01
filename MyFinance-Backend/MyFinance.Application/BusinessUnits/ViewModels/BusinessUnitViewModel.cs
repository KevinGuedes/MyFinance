namespace MyFinance.Application.BusinessUnits.ViewModels
{
    public class BusinessUnitViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double CurrentBalance { get; set; }
        public bool IsArchived { get; set; }

        public BusinessUnitViewModel(Guid id, string name, double currentBalance, bool isArchived)
            => (Id, Name, CurrentBalance, IsArchived) = (id, name, currentBalance, isArchived);
    }
}
