using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Data
{
    // This interface defines the contract for a product data store, which is responsible for retrieving product information based on a product identifier.
    // also added because we need an interface to mock the product store in unit tests, to verify the orchestration behavior of the service. * SOLID Principles *
    public interface IProductDataStore
    {
        public Product GetProduct(string productIdentifier);
    }
}
