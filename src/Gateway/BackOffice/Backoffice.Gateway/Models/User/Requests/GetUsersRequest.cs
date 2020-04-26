using FluentValidation;

namespace DTO.User
{
    public class GetUsersRequest : UserDto
    {
    }

    public class GetUsersRequestValidator : AbstractValidator<GetUsersRequest>
    {
        public GetUsersRequestValidator()
        {
            RuleFor(x => x)
                .Must(x =>
                    !string.IsNullOrEmpty(x.Account) ||
                    !string.IsNullOrEmpty(x.Email) ||
                    !string.IsNullOrEmpty(x.FirstName) ||
                    !string.IsNullOrEmpty(x.LastName)
                )
                .WithMessage("Must complete at least one field");
        }
    }
}
