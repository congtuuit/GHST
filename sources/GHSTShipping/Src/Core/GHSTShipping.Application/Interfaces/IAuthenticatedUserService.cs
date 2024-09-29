namespace GHSTShipping.Application.Interfaces
{
    public interface IAuthenticatedUserService
    {
        string UserId { get; }

        string UserName { get; }

        string DisplayName { get; }
    }
}
