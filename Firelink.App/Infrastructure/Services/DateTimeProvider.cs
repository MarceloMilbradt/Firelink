using Firelink.Application.Common.Interfaces;

namespace Firelink.Infrastructure.Services;

public class DateTimeProvider : IDateTime
{
    public DateTime Now => DateTime.Now;
}