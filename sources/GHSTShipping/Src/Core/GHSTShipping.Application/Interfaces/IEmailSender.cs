using GHSTShipping.Application.Wrappers;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Interfaces
{
    public interface IEmailSender
    {
        /// <summary>
        /// Send email to set password in the first time
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="fullName"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<BaseResult> SendEmailSetPasswordAsync(string emailAddress, string fullName, string token);

        /// <summary>
        /// Send email to reset password
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="fullName"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<BaseResult> SendEmailResetPasswordAsync(string emailAddress, string fullName, string token);
    }
}
