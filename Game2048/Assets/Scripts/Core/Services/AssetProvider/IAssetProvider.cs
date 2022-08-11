using UnityEngine;

namespace Core.Services.AssetProvider
{
    public interface IAssetProvider
    {
        TInstance LoadAsset<TInstance>(string assetPath) where TInstance :  Object;
    }
}