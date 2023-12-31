using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Utils
{
    public class DownloadBundle : IDownloadBundle
    {
        private ICustomLogger _logger;
        
        public DownloadBundle(ICustomLogger logger)
        {
            _logger = logger;
        }
        
        
        public async Task<GameObject> DownloadAsset(string assetAddress)
        {
            return await DownloadAsset<GameObject>(assetAddress);
        }

        public async Task<GameObject> DownloadAsset(AssetReference asset)
        {
            return await DownloadAsset<GameObject>(asset);
        }

        public async Task<T> DownloadAsset<T>(AssetReference asset) where T : Object
        {
            var loadResult = Addressables.LoadAssetAsync<T>(asset);

            await loadResult.Task;
            
            _logger.Log($"{nameof(DownloadBundle)} status: {loadResult.Status}, exception: {loadResult.OperationException}");

            return loadResult.Result;
        }

        public async Task<T> DownloadAsset<T>(string assetAddress) where T : Object
        {
            var loadResult = Addressables.LoadAssetAsync<T>(assetAddress);

            await loadResult.Task;
            
            _logger.Log($"{nameof(DownloadBundle)} status: {loadResult.Status}, exception: {loadResult.OperationException}");

            return loadResult.Result;
        }
    }

    public interface IDownloadBundle
    {
        Task<GameObject> DownloadAsset(string assetAddress);
        Task<GameObject> DownloadAsset(AssetReference asset);
        Task<T> DownloadAsset<T>(AssetReference asset) where T : Object;
        Task<T> DownloadAsset<T>(string assetAddress) where T : Object;
    }
}

