using UnityEngine;

namespace Core.Services.AssetProvider
{
    public class AssetProvider : IAssetProvider
    {
        public TInstance LoadAsset<TInstance>(string assetPath) where TInstance :  Object
        {
            return Resources.Load<TInstance>(assetPath);
        }
    }
}