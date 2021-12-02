using FluentValidation;
using RouteSheet.Shared.ViewModels;

namespace RouteSheet.Client.Validations
{
    public class ClassroomValidator : AbstractValidator<ClassroomViewModel>
    {
        public ClassroomValidator()
        {
            RuleFor(c => c.Name).NotEmpty();
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<ClassroomViewModel>.CreateWithOptions((ClassroomViewModel)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };
    }
}
