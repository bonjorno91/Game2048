using UnityEngine;

namespace Game.Services.AssetProvider
{
    public interface IAssetProvider
    {
        TInstance LoadAsset<TInstance>(string assetPath) where TInstance :  Object;
    }
}