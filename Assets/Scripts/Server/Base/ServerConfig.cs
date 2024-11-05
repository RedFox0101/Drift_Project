using Game.Server.Implementation;
using UnityEngine;

namespace Game.Server
{
    [CreateAssetMenu(menuName = "Data/Server/Server Config", fileName = "Server Config")]
    public class ServerConfig : ScriptableObject
    {
        [field:SerializeField] public ServerResource Character { get; private set; }
    }
}