using Game.Assets;
using Game.Assets.Assets;
using Game.Equipments.ItemData;
using UnityEngine;
using Zenject;

namespace Game.Equipments.Implementation.Handlers
{
    public class EquipmentGMHandler : EquipmentHandler<Transform, AssetResource>
    {
        [SerializeField] private AssetResource _defaultResource;
        
        private AssetsManager _assetsManager;
        private AssetResource _asset;
        
        [Inject]
        private void Install(AssetsManager assetsManager)
        {
            _assetsManager = assetsManager;
        }

        protected override AssetResource GetDefaultItem()
        {
            return _defaultResource;
        }
        
        protected override void EquipItem(EquipItemDataScriptable equipItemData, AssetResource objectVisual)
        {
            _defaultItem.gameObject.SetActive(false);
            _asset = _assetsManager.GetAsset(objectVisual, _targetData.transform.position,
                _targetData.transform.rotation, _targetData.transform);

            _asset.OnReleased += OnReleaseAsset;
        }

        protected override void ResetEquipment()
        {
            _defaultItem.gameObject.SetActive(true);
            if(_asset != null) _assetsManager.ReleaseAsset(_asset);
        }
        
        private void OnReleaseAsset(IAsset asset)
        {
            asset.OnReleased -= OnReleaseAsset;

            if (_asset.Equals(asset))
            {
                _asset = null;
                _defaultItem.gameObject.SetActive(true);
            }
        }
    }
}