namespace GHSTShipping.Application.DTOs.Account.Requests
{
    public class ForgotPasswordRequest
    {
        public required string Email { get; set; }

        public required string PhoneNumber { get; set; }
    }
}
