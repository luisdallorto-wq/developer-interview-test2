using Smartwyre.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Smartwyre.DeveloperTest.Services
{
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
