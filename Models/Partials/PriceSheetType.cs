using System.Linq;

namespace EFC.AgGateway.Integration.Specifications.AgGatewayServiceContract
{
    public partial class PriceSheetType
    {
        public string GetSeedYear()
        {
            return
                PriceSheet
                    .Header
                    .To
                    .PartnerInformation
                    .ContactInformation
                    .Single(contactInformation =>
                    {
                        return contactInformation.ContactDescription[0] == "SeedYear";
                    })
                    .ContactName[0];
        }
    }
}
