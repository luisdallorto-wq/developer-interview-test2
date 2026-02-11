using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using System;
using System.Collections.Generic;

namespace Smartwyre.DeveloperTest.Runner;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Rebate Calculator ===");

        Console.Write("Enter Rebate Identifier: ");
        var rebateId = Console.ReadLine();

        Console.Write("Enter Product Identifier: ");
        var productId = Console.ReadLine();

        Console.Write("Enter Volume: ");
        var volumeInput = Console.ReadLine();

        if (!decimal.TryParse(volumeInput, out var volume))
        {
            Console.WriteLine("Invalid volume.");
            return;
        }

        var request = new CalculateRebateRequest
        {
            RebateIdentifier = rebateId,
            ProductIdentifier = productId,
            Volume = volume
        };

        // Create calculators
        var calculators = new List<IRebateCalculator>
            {
                new FixedCashAmountCalculator(),
                new FixedRateRebateCalculator(),
                new AmountPerUomCalculator()
            };

        // Create service
        var rebateService = new RebateService(
            new RebateDataStore(),
            new ProductDataStore(),
            calculators);

        var result = rebateService.Calculate(request);

        Console.WriteLine();
        Console.WriteLine(result.Success
            ? "Rebate calculated successfully."
            : "Rebate calculation failed.");
    }
}
