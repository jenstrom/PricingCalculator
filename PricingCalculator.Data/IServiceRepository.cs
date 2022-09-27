using PricingCalculator.Data.Models;

namespace PricingCalculator.Data
{
    public interface IServiceRepository
    {
        ServiceModel? GetServiceModel(string serviceName);
        List<ServiceModel> GetServices();
        bool ServicesExist(IEnumerable<string> serviceNames);
    }
}