namespace MyFinance.Application.Common.DTO
{
    public abstract class BaseDTO
    {
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
