using FluentValidation;
using RouteSheet.Shared.ViewModels;

namespace RouteSheet.Client.Validations
{
    public class CadetValidator : AbstractValidator<CadetViewModel>
    {
        public CadetValidator()
        {
            RuleFor(c => c.Name).NotEmpty().WithMessage("Поле Ф.И.О. обязательно для заполнения!");
            RuleFor(c => c.Classroom.Name).NotEmpty().WithMessage("Поле класс обязательно для заполнения!");
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<CadetViewModel>.CreateWithOptions((CadetViewModel)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };
    }
}
