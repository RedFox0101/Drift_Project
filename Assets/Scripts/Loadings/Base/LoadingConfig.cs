using Game.Data;
using Game.Loadings.Scenes;
using Game.Loadings.UI;
using UnityEngine;

namespace Game.Loadings
{
    [CreateAssetMenu(menuName = "Data/Loading/Loading Config", fileName = "Loading Config")]
    public class LoadingConfig : DataConfig<LoadingDataScriptable>
    {
        [SerializeField] public SceneField BootScene { get; private set; }
        [SerializeField] public UILoadingScreen LoadingScreen { get; private set; }
        [SerializeField] public LoadingDataScriptable MetaLoadingData { get; private set; }
        [SerializeField] public int SkipForLoop { get; private set; }
    }
}