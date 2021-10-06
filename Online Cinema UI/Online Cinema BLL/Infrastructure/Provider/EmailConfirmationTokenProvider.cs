using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Online_Cinema_BLL.Infrastructure.Provider
{
    public class EmailConfirmationTokenProvider<TUser> : DataProtectorTokenProvider<TUser> where TUser : class
    {
        public EmailConfirmationTokenProvider(IDataProtectionProvider dataProtectionProvider,
            IOptions<DataProtectionTokenProviderOptions> options) : base(dataProtectionProvider, options) { }


        //   public EmailConfirmationTokenProvider(IDataProtectionProvider dataProtectionProvider, IOptions<DataProtectionTokenProviderOptions> options,
        //ILogger<DataProtectorTokenProvider<TUser>> userLogger) : base(dataProtectionProvider, options, userLogger) { }


    }

    public class EmailConfirmationProviderOption : DataProtectionTokenProviderOptions
    { }
}