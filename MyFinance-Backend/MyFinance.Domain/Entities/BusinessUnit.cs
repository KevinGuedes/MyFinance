namespace MyFinance.Domain.Entities
{
    public class BusinessUnit : Entity
    {
        public BusinessUnit(string name)
            => Name = name;

        public string Name { get; private set; }

        public void Update(string name)
        {
            SetUpdateDate();
            Name = name;
        }
    }
}
