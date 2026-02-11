using Smartwyre.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Smartwyre.DeveloperTest.Services
{
    public class FixedRateRebateCalculator: IRebateCalculator
    {
        public CalculateRebateResult CalculateRebateAmount(Rebate rebate, Product product, CalculateRebateRequest request)
        {
            CalculateRebateResult result = new CalculateRebateResult();
           
            if (!product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedRateRebate))
            {
                result.Success = false;
            }
            else if (rebate.Percentage == 0 || product.Price == 0 || request.Volume == 0)
            {
                result.Success = false;
            }
            else
            {
                result.RebateAmount += product.Price * rebate.Percentage * request.Volume;
                result.Success = true;
            }
            return result;
        }

        public bool CanCalculate(IncentiveType incentiveType) => incentiveType == IncentiveType.FixedRateRebate;
    }
}
