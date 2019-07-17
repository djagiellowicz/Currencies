using System.Threading.Tasks;
using WalutyBusinessLogic.Models;

namespace WalutyBusinessLogic.Services
{
    public interface IExtremesServices
    {
        Task<GlobalExtremeValueModel> GetGlobalExtreme(GlobalExtremeValueModel extremeValue);
        Task<LocalExtremeValueModel> GetLocalExtremes(LocalExtremeValueModel extremeValue);
    }
}