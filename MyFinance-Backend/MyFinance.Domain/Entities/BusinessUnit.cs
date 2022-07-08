using MyFinance.Domain.Interfaces;

namespace MyFinance.Domain.Entities
{
    public class BusinessUnit : Entity, IAggregateRoot
    {
        public string Name { get; private set; }
        public bool IsArchived { get; private set; }
        public double CurrentBalance { get; private set; }

        public BusinessUnit(string name)
            => (Name, IsArchived, CurrentBalance) = (name, false, 0);

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
