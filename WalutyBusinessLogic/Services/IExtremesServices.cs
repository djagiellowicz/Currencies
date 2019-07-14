using System.Threading.Tasks;
using WalutyBusinessLogic.Models;

namespace WalutyBusinessLogic.Services
{
    public interface IExtremesServices
    {
        Task<GlobalExtremeValueModel> GetGlobalExtremes(GlobalExtremeValueModel extremeValue);
        Task<LocalExtremeValueModel> GetLocalExtremes(LocalExtremeValueModel extremeValue);
    }
}