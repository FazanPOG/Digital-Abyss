using Unity.Entities;
using UnityEngine;

namespace DigitalAbyss.Architecture.Source.Scripts.Components
{
    public class PlayerViewGameObjectPrefab : IComponentData
    {
        public GameObject Value;
    }

    public class PlayerAnimatorReference : ICleanupComponentData
    {
        public Animator Value;
    }

    public class PlayerAnimatorAuthoring : MonoBehaviour
    {
        public GameObject PlayerViewGameObjectPrefab;
        
        public class  Baker : Baker<PlayerAnimatorAuthoring>
        {
            public override void Bake(PlayerAnimatorAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponentObject(entity, new PlayerViewGameObjectPrefab
                {
                    Value = authoring.PlayerViewGameObjectPrefab
                });
            }
        }
    }
}