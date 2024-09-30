using GHSTShipping.Domain.Enums;
using GHSTShipping.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GHSTShipping.Infrastructure.Persistence.Seeds
{
    public static class DeliveryPartnerConfig
    {
        public static async Task SeedAsync(ApplicationDbContext dbContext)
        {
            await CreateConfigAsync(dbContext, EnumDeliveryPartner.GHN);
            /*await CreateConfigAsync(dbContext, EnumDeliveryPartner.JT);
            await CreateConfigAsync(dbContext, EnumDeliveryPartner.SPX);
            await CreateConfigAsync(dbContext, EnumDeliveryPartner.Viettel);
            await CreateConfigAsync(dbContext, EnumDeliveryPartner.Best);
            await CreateConfigAsync(dbContext, EnumDeliveryPartner.GHTK);*/
        }

        public static async Task CreateConfigAsync(ApplicationDbContext dbContext, EnumDeliveryPartner key)
        {
            var existedConfig = await dbContext.PartnerConfigs.AnyAsync(i => i.DeliveryPartner == key);
            if (!existedConfig)
            {
                string sanboxEnv = "";
                string prodEnv = "";

                if(key == EnumDeliveryPartner.GHN)
                {
                    sanboxEnv = "https://5sao.ghn.dev";
                    prodEnv = "https://khachhang.ghn.vn";
                }

                var config = new Domain.Entities.PartnerConfig
                {
                    SanboxEnv = sanboxEnv,
                    ProdEnv = prodEnv,
                    DeliveryPartner = key,
                    IsActivated = false,
                };

                await dbContext.PartnerConfigs.AddAsync(config);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
