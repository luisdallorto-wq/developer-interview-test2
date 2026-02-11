using Smartwyre.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Smartwyre.DeveloperTest.Services
{
    public class AmountPerUomCalculator: IRebateCalculator
    {
        public CalculateRebateResult CalculateRebateAmount(Rebate rebate, Product product, CalculateRebateRequest request)
        {
            CalculateRebateResult result = new CalculateRebateResult();
            
            if (!product.SupportedIncentives.HasFlag(SupportedIncentiveType.AmountPerUom))
            {
                result.Success = false;
            }
            else if (rebate.Amount == 0 || request.Volume == 0)
            {
                result.Success = false;
            }
            else
            {
                result.RebateAmount += rebate.Amount * request.Volume;
                result.Success = true;
            }
            return result;
        }

        public bool CanCalculate(IncentiveType incentiveType) => incentiveType == IncentiveType.AmountPerUom;
    }
}
