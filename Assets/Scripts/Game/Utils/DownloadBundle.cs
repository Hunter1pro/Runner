using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Utils
{
    public class DownloadBundle : IDownloadBundle
    {
        private ICustomLogger _logger;
        private string _catalogPath = "Default";

        public DownloadBundle(ICustomLogger logger)
        {
            _logger = logger;
        }
        
        
        public async Task<GameObject> DownloadAsset(string assetAddress)
        {
            var loadResult = Addressables.LoadAssetAsync<GameObject>(assetAddress);

            await loadResult.Task;
            
            _logger.Log($"{nameof(DownloadBundle)} status: {loadResult.Status}, exception: {loadResult.OperationException}");

            return loadResult.Result;
        }
    }

    public interface IDownloadBundle
    {
        Task<GameObject> DownloadAsset(string assetAddress);
    }
}

