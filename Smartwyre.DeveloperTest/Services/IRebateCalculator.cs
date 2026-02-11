using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services
{
    /*
     *  This interface defines the contract for rebate calculators. Each calculator implementation will handle a specific incentive type and 
     *  contain the logic to calculate the rebate amount based on the rebate, product, and request details.
     *  In case we need to support more incentive types in the future, we can simply add new implementations of this interface without modifying existing code, adhering to the Open/Closed Principle of SOLID.
     *  */
    public interface IRebateCalculator
    {
        public bool CanCalculate(IncentiveType incentiveType);
        public CalculateRebateResult CalculateRebateAmount(Rebate rebate, Product product, CalculateRebateRequest request);
    }
}
