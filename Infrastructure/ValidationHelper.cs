using System.ComponentModel.DataAnnotations;

namespace ProductApi.Infrastructure;

public static class ValidationHelper
{
    public static (bool IsValid, object? ErrorResult) Validate<T>(T dto)
    {
        var context = new ValidationContext(dto!);
        var results = new List<ValidationResult>();
        bool valid = Validator.TryValidateObject(dto!, context, results, true);
        if (valid) return (true, null);

        var errors = results
            .SelectMany(r => r.MemberNames.Select(m => new { field = m, error = r.ErrorMessage }))
            .GroupBy(x => x.field)
            .ToDictionary(g => g.Key, g => g.Select(x => x.error).ToArray());

        return (false, new { errors });
    }
}
