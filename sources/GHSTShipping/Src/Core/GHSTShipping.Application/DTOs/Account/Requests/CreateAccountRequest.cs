namespace GHSTShipping.Application.DTOs.Account.Requests
{
    public class CreateAccountRequest
    {
        public string FullName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string PhoneNumber { get; set; }
    }
}
