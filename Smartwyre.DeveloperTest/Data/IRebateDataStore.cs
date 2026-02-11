using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Data
{
    // This interface defines the contract for a rebate data store, which is responsible for retrieving rebate information based on a rebate identifier and storing the results of rebate calculations.
    // also added because we need an interface to mock the rebate store in unit tests, to verify the orchestration behavior of the service. * SOLID Principles *.
    public interface IRebateDataStore
    {
        public Rebate GetRebate(string rebateIdentifier);

        public void StoreCalculationResult(Rebate account, decimal rebateAmount);
    }
}
