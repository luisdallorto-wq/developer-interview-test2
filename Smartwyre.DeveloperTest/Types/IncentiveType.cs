namespace Smartwyre.DeveloperTest.Types;

public enum IncentiveType
{
    None = 0, //added, because the default value of enum is 0, and it is better to have a None value to represent no incentive type.
    FixedRateRebate = 1,
    AmountPerUom = 2,
    FixedCashAmount = 3
}
