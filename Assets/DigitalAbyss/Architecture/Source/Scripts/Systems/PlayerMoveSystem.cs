using DigitalAbyss.Architecture.Source.Scripts.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DigitalAbyss.Architecture.Source.Scripts.Systems
{
    [UpdateBefore(typeof(TransformSystemGroup))]
    public partial struct PlayerMoveSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;
            var moveInput = SystemAPI.GetSingleton<PlayerMoveInput>();
            
            new PlayerMoveJob
            {
                DeltaTime = deltaTime,
                MoveInput = moveInput
            }.Schedule(state.Dependency).Complete();
        }
    }
    
    [BurstCompile]
    public partial struct PlayerMoveJob : IJobEntity
    {
        public float DeltaTime;
        public PlayerMoveInput MoveInput;    
        
        private void Execute(ref LocalTransform transform, MoveSpeed moveSpeed)
        {
            transform.Position.xy += MoveInput.Value * moveSpeed.Value * DeltaTime;
        }
    }
}
