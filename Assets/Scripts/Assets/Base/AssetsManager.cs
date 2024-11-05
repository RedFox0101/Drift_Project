using System.Collections.Generic;
using System.Linq;
using Game.Assets.Assets;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace Game.Assets
{
    public class AssetsManager
    {
        private Dictionary<string, AssetResource> _poolAssetResource = new();
        private Dictionary<IAsset, List<IAsset>> _poolAsset = new();
        private Dictionary<IAsset, IAsset> _activeAsset = new();

        private DiContainer _diContainer;
        
        [Inject]
        private void Install(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        public T GetAssetResource<T>(string prefabID, Vector3 position, Quaternion rotation, Transform parent) where T : AssetResource 
        {
            AssetResource assetResource;
            
            if (_poolAssetResource.TryGetValue(prefabID, out var poolAssetResource))
            {
                assetResource = poolAssetResource;
            }
            else assetResource = Resources.Load<T>(prefabID);

            if (assetResource != null)
            {
                _poolAssetResource.TryAdd(prefabID, assetResource);

                bool wasActive = assetResource.gameObject.activeSelf;

                if (wasActive) assetResource.gameObject.SetActive(false);

                var asset = GetAsset(assetResource, position, rotation, parent);

                if (wasActive) assetResource.gameObject.SetActive(true);
                
                return asset as T;
            }

            return null;
        }
        
        public T GetAsset<T>(T prefab, Transform parent) where T : Object, IAsset
        {
            if(parent != null) return GetAsset<T>(prefab, parent.position, parent.rotation, parent);
            else return GetAsset<T>(prefab, Vector3.zero, Quaternion.identity, parent);
        }

        public T GetAsset<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent) where T : Object, IAsset
        {
            T asset = null;
            
            if(_poolAsset.TryGetValue(prefab, out var poolList))
            {
                asset = poolList.FirstOrDefault() as T;
            }

            if (asset != null)
            {
                _poolAsset[prefab].Remove(asset);
                
                if (asset.Asset.IsUnityNull() || asset.Asset.IsDestroyed())
                {
                    asset = GetAsset<T>(prefab, position, rotation, parent);
                    return asset;
                }
                
                asset.Asset.transform.position = position;
                asset.Asset.transform.rotation = rotation;
                asset.Asset.transform.SetParent(parent, true);
                asset.Asset.gameObject.SetActive(true);
            }
            else
            {
                var gameObject = _diContainer.InstantiatePrefab(prefab, position, rotation, null);

                gameObject.transform.SetParent(parent, true);
                
                asset = gameObject.GetComponent<T>();
            }

            asset.OnReleased += OnReleaseAsset;
            
            _activeAsset.Add(asset, prefab);
            
            return asset;
        }

        private void OnReleaseAsset(IAsset asset)
        {
            if (!_activeAsset.ContainsKey(asset)) return;
            
            asset.OnReleased -= OnReleaseAsset;

            var assetContract = _activeAsset[asset];
            
            _activeAsset.Remove(asset);
            
            if (!asset.Asset.IsDestroyed())
            {
                _poolAsset.TryAdd(assetContract, new List<IAsset>());

                _poolAsset[assetContract].Add(asset);

                if (asset.Asset.activeSelf)
                {
                    asset.Asset.SetActive(false);
                }
            }
        }

        public void ReleaseAsset(IAsset asset)
        {
            OnReleaseAsset(asset);
        }
    }
}