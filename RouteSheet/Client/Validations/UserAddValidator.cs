using FluentValidation;
using RouteSheet.Shared.ViewModels;

namespace RouteSheet.Client.Validations
{
    public class UserAddValidator : AbstractValidator<UserAddViewModel>
    {
        public UserAddValidator()
        {
            RuleFor(c => c.Name).NotEmpty().WithMessage("Это поле обязательно для заполнения!");
            RuleFor(c => c.Position).NotEmpty().WithMessage("Это поле обязательно для заполнения!");
            RuleFor(c => c.Email).NotEmpty().WithMessage("Это поле обязательно для заполнения!");
            RuleFor(c => c.UserName).NotEmpty().WithMessage("Это поле обязательно для заполнения!");
            RuleFor(c => c.Role).NotEmpty().WithMessage("Это поле обязательно для заполнения!");
            RuleFor(c => c.Password).NotEmpty().WithMessage("Это поле обязательно для заполнения!");
            RuleFor(c => c.ConfirmPassword).NotEmpty().WithMessage("Это поле обязательно для заполнения!");
            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("Пароли должны совпадать!");
            // RuleFor(c => c.Password).NotEmpty().Equal(c => c.ConfirmPassword).When(c => !String.IsNullOrWhiteSpace(c.Password)).WithMessage("Это поле обязательно для заполнения!");
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<UserAddViewModel>.CreateWithOptions((UserAddViewModel)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };
    }
}
