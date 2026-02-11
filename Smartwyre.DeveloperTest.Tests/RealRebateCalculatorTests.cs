using Moq;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using System.Collections.Generic;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests;

//I want to verify the actual business logic of each calculator, so 3 test units were added with real calculator implementations,
public class RealRebateCalculatorTests
{
    private readonly Mock<IRebateDataStore> _rebateStoreMock;
    private readonly Mock<IProductDataStore> _productStoreMock;

    public RealRebateCalculatorTests()
    {
        _rebateStoreMock = new Mock<IRebateDataStore>();
        _productStoreMock = new Mock<IProductDataStore>();

    }

    [Fact]
    public void Calculate_WhenFixedRateRebate_IsValid_ReturnsSuccessWithAmount()
    {
        // Arrange
        var request = new CalculateRebateRequest
        {
            RebateIdentifier = "rebate1",
            ProductIdentifier = "product1",
            Volume = 10
        };

        var rebate = new Rebate
        {
            Incentive = IncentiveType.FixedRateRebate,
            Percentage = 0.1m
        };

        var product = new Product
        {
            Price = 100m,
            SupportedIncentives = SupportedIncentiveType.FixedRateRebate
        };

        var expectedAmount = 100m * 0.1m * 10; // 100

        _rebateStoreMock.Setup(x => x.GetRebate(request.RebateIdentifier))
            .Returns(rebate);

        _productStoreMock.Setup(x => x.GetProduct(request.ProductIdentifier))
            .Returns(product);

        var calculators = new List<IRebateCalculator>
            {
                new FixedRateRebateCalculator()
            };

        var service = new RebateService(
            _rebateStoreMock.Object,
            _productStoreMock.Object,
            calculators);

        // Act
        var result = service.Calculate(request);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(expectedAmount, result.RebateAmount);

    }

    [Fact]
    public void Calculate_WhenAmountPerUom_IsValid_ReturnsSuccessWithAmount()
    {
        // Arrange
        var request = new CalculateRebateRequest
        {
            RebateIdentifier = "rebate2",
            ProductIdentifier = "product2",
            Volume = 5
        };

        var rebate = new Rebate
        {
            Incentive = IncentiveType.AmountPerUom,
            Amount = 20m
        };

        var product = new Product
        {
            SupportedIncentives = SupportedIncentiveType.AmountPerUom
        };

        var expectedAmount = 20m * 5; // 100

        _rebateStoreMock.Setup(x => x.GetRebate(request.RebateIdentifier))
            .Returns(rebate);

        _productStoreMock.Setup(x => x.GetProduct(request.ProductIdentifier))
            .Returns(product);

        var calculators = new List<IRebateCalculator>
            {
                new AmountPerUomCalculator()
            };

        var service = new RebateService(
            _rebateStoreMock.Object,
            _productStoreMock.Object,
            calculators);

        // Act
        var result = service.Calculate(request);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(expectedAmount, result.RebateAmount);
    }

    [Fact]
    public void Calculate_WhenFixedCashAmount_IsValid_ReturnsSuccessWithAmount()
    {
        // Arrange
        var request = new CalculateRebateRequest
        {
            RebateIdentifier = "rebate3",
            ProductIdentifier = "product3",
            Volume = 10 // Volume shouldn't matter here
        };

        var rebate = new Rebate
        {
            Incentive = IncentiveType.FixedCashAmount,
            Amount = 250m
        };

        var product = new Product
        {
            SupportedIncentives = SupportedIncentiveType.FixedCashAmount
        };

        var expectedAmount = 250m;

        _rebateStoreMock.Setup(x => x.GetRebate(request.RebateIdentifier))
            .Returns(rebate);

        _productStoreMock.Setup(x => x.GetProduct(request.ProductIdentifier))
            .Returns(product);

        var calculators = new List<IRebateCalculator>
            {
                new FixedCashAmountCalculator()
            };

        var service = new RebateService(
            _rebateStoreMock.Object,
            _productStoreMock.Object,
            calculators);

        // Act
        var result = service.Calculate(request);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(expectedAmount, result.RebateAmount);
    }

}


