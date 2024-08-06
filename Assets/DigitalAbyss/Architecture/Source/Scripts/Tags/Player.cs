using UnityEngine;
using Unity.Entities;

namespace DigitalAbyss.Architecture.Source.Scripts.Tags
{
    public class Player : MonoBehaviour
    {
        public class Baker : Baker<Player>
        {
            public override void Bake(Player authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new PlayerTag());
            }
        }
    }
    
    public struct PlayerTag : IComponentData
    {
            
    }
}
