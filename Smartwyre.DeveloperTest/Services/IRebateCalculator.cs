using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services
{
    public interface IRebateCalculator
    {
        public bool CanCalculate(IncentiveType incentiveType);
        public CalculateRebateResult CalculateRebateAmount(Rebate rebate, Product product, CalculateRebateRequest request);
    }
}
