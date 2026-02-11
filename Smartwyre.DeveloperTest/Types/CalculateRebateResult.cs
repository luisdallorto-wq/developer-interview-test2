namespace Smartwyre.DeveloperTest.Types;

public class CalculateRebateResult
{
    public bool Success { get; set; }

    public decimal RebateAmount { get; set; } // added property to encapsulate the calculated rebate amount in the same result object, allowing for a more cohesive and comprehensive response from the rebate calculation process.
}
