using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Types;
using System.Collections.Generic;
using System.Linq;

namespace Smartwyre.DeveloperTest.Services;

public class RebateService : IRebateService
{
    private readonly RebateDataStore _rebateDataStore;
    private readonly ProductDataStore _productDataStore;
    private readonly IEnumerable<IRebateCalculator> _calculators;

    public RebateService(
        RebateDataStore rebateDataStore,
        ProductDataStore productDataStore,
        IEnumerable<IRebateCalculator> calculators)
    {
        _rebateDataStore = rebateDataStore;
        _productDataStore = productDataStore;
        _calculators = calculators;
    }

    public CalculateRebateResult Calculate(CalculateRebateRequest request)
    {
        var rebate = _rebateDataStore.GetRebate(request.RebateIdentifier);
        var product = _productDataStore.GetProduct(request.ProductIdentifier);

        var result = new CalculateRebateResult();

        if (rebate == null || product == null || rebate.Incentive == IncentiveType.None)
        {
            result.Success = false;
            return result;
        }

        var calculator = _calculators
            .FirstOrDefault(c => c.CanCalculate(rebate.Incentive));

        if (calculator == null)
        {
            result.Success = false;
            return result;
        }

        result = calculator.CalculateRebateAmount(rebate, product, request);

        if (result.Success)
        {
            _rebateDataStore.StoreCalculationResult(rebate, result.RebateAmount);
        }

        return result;
    }
}
