using MyFinance.Domain.Interfaces;

namespace MyFinance.Domain.Entities
{
    public class BusinessUnit : Entity, IAggregateRoot
    {
        public string Name { get; private set; }
        public bool IsArchived { get; private set; }
        public double CurrentBalance { get; private set; }
        public string? Description { get; set; }

        public BusinessUnit(string name, string? description)
            => (Name, IsArchived, CurrentBalance, Description) = (name, false, 0, description);

        public void Update(string name, bool isArchived)
        {
            SetUpdateDate();
            Name = name;
            IsArchived = isArchived;
        }

        public void AddBalance(double value)
        {
            SetUpdateDate();
            CurrentBalance += value;
        }
    }
}
