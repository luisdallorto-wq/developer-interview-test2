using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services
{
    //Specifically for fixed cash amount rebate, the rebate amount is determined solely by the rebate's Amount property, regardless of the product price or volume.
    public class FixedCashAmountCalculator : IRebateCalculator
    {
        public CalculateRebateResult CalculateRebateAmount(Rebate rebate, Product product, CalculateRebateRequest request)
        {
            CalculateRebateResult result = new CalculateRebateResult();

            if (!product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedCashAmount))
            {
                result.Success = false;
            }
            else if (rebate.Amount == 0)
            {
                result.Success = false;
            }
            else
            {
                result.RebateAmount = rebate.Amount;
                result.Success = true;
            }
            return result;
        }

        public bool CanCalculate(IncentiveType incentiveType) => incentiveType == IncentiveType.FixedCashAmount;
    }
}
