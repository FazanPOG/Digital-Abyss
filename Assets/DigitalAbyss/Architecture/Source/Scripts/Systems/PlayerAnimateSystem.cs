using DigitalAbyss.Architecture.Source.Scripts.Components;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.VisualScripting;
using UnityEngine;

namespace DigitalAbyss.Architecture.Source.Scripts.Systems
{
    [UpdateInGroup(typeof(PresentationSystemGroup), OrderLast = true)]
    public partial struct PlayerAnimateSystem : ISystem
    {
        private const string IS_MOVING_KEY = "IsMoving";
        private const string X_KEY = "X";
        private const string Y_KEY = "Y";
        private const string LAST_X_KEY = "LastX";
        private const string LAST_Y_KEY = "LastY";

        private float _lastXInput;
        private float _lastYInput;
        
        public void OnUpdate(ref SystemState state)
        {
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
            var moveInput = SystemAPI.GetSingleton<PlayerMoveInput>();
            
            foreach (var (playerGameObjectPrefab, entity) 
                in SystemAPI
                    .Query<PlayerViewGameObjectPrefab>()
                    .WithNone<PlayerAnimatorReference>()
                    .WithEntityAccess())
            {
                var newCompanionGameObject = Object.Instantiate(playerGameObjectPrefab.Value);
                var newAnimatorReference = new PlayerAnimatorReference
                {
                    Value = newCompanionGameObject.GetComponent<Animator>()
                };
                ecb.AddComponent(entity, newAnimatorReference);
            }

            foreach (var (transform, animatorReference) 
                in SystemAPI
                    .Query<LocalTransform, PlayerAnimatorReference>())
            {
                animatorReference.Value.SetBool(IS_MOVING_KEY, math.length(moveInput.Value) > 0f);
                animatorReference.Value.SetFloat(X_KEY, moveInput.Value.x);
                animatorReference.Value.SetFloat(Y_KEY, moveInput.Value.y);

                if (math.length(moveInput.Value) == 0f)
                {
                    animatorReference.Value.SetFloat(LAST_X_KEY, _lastXInput);
                    animatorReference.Value.SetFloat(LAST_Y_KEY, _lastYInput);
                }
                else
                {
                    _lastXInput = moveInput.Value.x;
                    _lastYInput = moveInput.Value.y;
                }
                
                animatorReference.Value.transform.position = transform.Position;
                animatorReference.Value.transform.rotation = transform.Rotation;
            }

            foreach (var (animatorReference, entity) 
                in SystemAPI
                    .Query<PlayerAnimatorReference>()
                    .WithNone<PlayerViewGameObjectPrefab, LocalTransform>()
                    .WithEntityAccess())
            {
                Object.Destroy(animatorReference.Value.gameObject);
                ecb.RemoveComponent<PlayerAnimatorReference>(entity);
            }
            
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}
