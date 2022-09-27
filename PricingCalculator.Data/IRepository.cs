using PricingCalculator.Data.Models;

namespace PricingCalculator.Data
{
    public interface IRepository
    {
        Customer? GetCustomer(int id);
        List<Customer> GetCustomers();
        List<ServiceModel> GetServices();
    }
}