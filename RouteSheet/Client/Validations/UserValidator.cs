using FluentValidation;
using RouteSheet.Shared.ViewModels;

namespace RouteSheet.Client.Validations
{
    public class UserValidator : AbstractValidator<UserViewModel>
    {
        public UserValidator()
        {
            RuleFor(c => c.Name).NotEmpty().WithMessage("Это поле обязательно для заполнения!");
            RuleFor(c => c.Position).NotEmpty().WithMessage("Это поле обязательно для заполнения!");
            RuleFor(c => c.Email).NotEmpty().WithMessage("Это поле обязательно для заполнения!");
            RuleFor(c => c.UserName).NotEmpty().WithMessage("Это поле обязательно для заполнения!");
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<UserViewModel>.CreateWithOptions((UserViewModel)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };
    }
}
