﻿namespace MyFinance.Domain.Entities
{
    public class BusinessUnit : Entity
    {
        public BusinessUnit(string name, double currentBalance)
            => (Name, CurrentBalance) = (name, currentBalance);

        public string Name { get; private set; }
        public double CurrentBalance { get; private set; }

        public void Update(string name)
        {
            SetUpdateDate();
            Name = name;
        }

        public void UpdateBalance(double value)
        {
            SetUpdateDate();
            CurrentBalance =+ value;
        }
    }
}
