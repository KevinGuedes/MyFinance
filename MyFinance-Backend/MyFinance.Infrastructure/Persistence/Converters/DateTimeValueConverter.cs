using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MyFinance.Infrastructure.Persistence.Converters;

internal sealed class DateTimeValueConverter : ValueConverter<DateTime, DateTime>
{
    public DateTimeValueConverter() : base(
        dateTime => dateTime.ToUniversalTime(),
        dateTimeAsString => DateTime.SpecifyKind(dateTimeAsString, DateTimeKind.Utc),
        null)
    {
    }
}

internal sealed class NullableDateTimeValueConverter : ValueConverter<DateTime?, DateTime?>
{
    public NullableDateTimeValueConverter() : base(
        dateTime => dateTime.HasValue ? dateTime.Value.ToUniversalTime() : dateTime,
        dateTimeAsString => dateTimeAsString.HasValue ? DateTime.SpecifyKind(dateTimeAsString.Value, DateTimeKind.Utc) : dateTimeAsString,
        null)
    {
    }
}