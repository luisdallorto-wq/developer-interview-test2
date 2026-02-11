using Moq;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using System.Collections.Generic;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests;

/*
 *  important!, these tests verify the orchestration behavior of the service - that it correctly retrieves data, selects the right calculator, and stores results.
 *  so, I mock the calculator to isolate orchestration behavior
*/
public class RebateServiceTests
{
    private readonly Mock<IRebateDataStore> _rebateStoreMock;
    private readonly Mock<IProductDataStore> _productStoreMock;
    private readonly Mock<IRebateCalculator> _calculatorMock;

    private readonly RebateService _service; // using the real service implementation, but with mocked dependencies to verify orchestration behavior

    public RebateServiceTests()
    {
        _rebateStoreMock = new Mock<IRebateDataStore>();
        _productStoreMock = new Mock<IProductDataStore>();
        _calculatorMock = new Mock<IRebateCalculator>();

        var calculators = new List<IRebateCalculator>
        {
            _calculatorMock.Object
        };

        _service = new RebateService(
            _rebateStoreMock.Object,
            _productStoreMock.Object,
            calculators
        );
    }

    [Fact]
    public void Calculate_ShouldFail_WhenRebateNotFound()
    {
        var request = new CalculateRebateRequest
        {
            RebateIdentifier = "rebate1",
            ProductIdentifier = "product1",
            Volume = 10
        };

        _rebateStoreMock
            .Setup(x => x.GetRebate(request.RebateIdentifier))
            .Returns((Rebate)null);

        var result = _service.Calculate(request);

        Assert.False(result.Success);
    }

    [Fact]
    public void Calculate_ShouldFail_WhenProductNotFound()
    {
        var request = new CalculateRebateRequest
        {
            RebateIdentifier = "rebate1",
            ProductIdentifier = "product1",
            Volume = 10
        };

        _rebateStoreMock
            .Setup(x => x.GetRebate(request.RebateIdentifier))
            .Returns(new Rebate());

        _productStoreMock
            .Setup(x => x.GetProduct(request.ProductIdentifier))
            .Returns((Product)null);

        var result = _service.Calculate(request);

        Assert.False(result.Success);
    }


    [Fact]
    public void Calculate_ShouldFail_WhenNoCalculatorFound()
    {
        var rebate = new Rebate
        {
            Incentive = IncentiveType.FixedCashAmount
        };

        var product = new Product();

        var request = new CalculateRebateRequest
        {
            RebateIdentifier = "rebate1",
            ProductIdentifier = "product1"
        };

        _rebateStoreMock
            .Setup(x => x.GetRebate(It.IsAny<string>()))
            .Returns(rebate);

        _productStoreMock
            .Setup(x => x.GetProduct(It.IsAny<string>()))
            .Returns(product);

        _calculatorMock
            .Setup(x => x.CanCalculate(It.IsAny<IncentiveType>()))
            .Returns(false);

        var result = _service.Calculate(request);

        Assert.False(result.Success);
    }

    [Fact]
    public void Calculate_ShouldSucceed_WhenCalculatorReturnsSuccess()
    {
        var rebate = new Rebate
        {
            Incentive = IncentiveType.FixedCashAmount,
            Amount = 100
        };

        var product = new Product
        {
            SupportedIncentives = SupportedIncentiveType.FixedCashAmount
        };

        var request = new CalculateRebateRequest
        {
            RebateIdentifier = "rebate1",
            ProductIdentifier = "product1",
            Volume = 5
        };

        var calculateRebateResult = new CalculateRebateResult
        {
            Success = true,
            RebateAmount = 100m
        };

        _rebateStoreMock
            .Setup(x => x.GetRebate(request.RebateIdentifier))
            .Returns(rebate);

        _productStoreMock
            .Setup(x => x.GetProduct(request.ProductIdentifier))
            .Returns(product);

        _calculatorMock
            .Setup(x => x.CanCalculate(IncentiveType.FixedCashAmount))
            .Returns(true);

        _calculatorMock
            .Setup(x => x.CalculateRebateAmount(rebate, product, request))
            .Returns(calculateRebateResult);

        var result = _service.Calculate(request);

        Assert.True(result.Success);
        Assert.Equal(100m, result.RebateAmount);

        _rebateStoreMock.Verify(
            x => x.StoreCalculationResult(rebate, 100m),
            Times.Once
        );
    }


}
