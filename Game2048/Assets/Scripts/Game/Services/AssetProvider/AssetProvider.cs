using UnityEngine;

namespace Game.Services.AssetProvider
{
    public class AssetProvider : IAssetProvider
    {
        public TInstance LoadAsset<TInstance>(string assetPath) where TInstance :  Object
        {
            return Resources.Load<TInstance>(assetPath);
        }
    }
}