using System.Threading.Tasks;
using WalutyBusinessLogic.Models;

namespace WalutyBusinessLogic.Services
{
    public interface IChartService
    {
        Task<ChartModel> CreateChartModel(string currencyCode);
    }
}
