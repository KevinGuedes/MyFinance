namespace MyFinance.Domain.Entities
{
    public class BusinessUnit : Entity
    {
        public string Name { get; private set; }
        public double CurrentBalance { get; private set; }

        public BusinessUnit(string name)
            => (Name, CurrentBalance) = (name, 0);

        public void Update(string name)
        {
            SetUpdateDate();
            Name = name;
        }

        public void AddRevenue(double value)
        {
            SetUpdateDate();
            CurrentBalance += value;
        }
    }
}
