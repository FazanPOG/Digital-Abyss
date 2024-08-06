using DigitalAbyss.Architecture.Source.Input.Scripts;
using DigitalAbyss.Architecture.Source.Scripts.Components;
using Unity.Entities;
using UnityEngine;

namespace DigitalAbyss.Architecture.Source.Scripts.Systems
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial class GetPlayerInputSystem : SystemBase
    {
        private PlayerInputActions _inputActions;
        
        protected override void OnCreate()
        {
            EntityManager.CreateEntity(typeof(PlayerMoveInput));
            
            RequireForUpdate<PlayerMoveInput>();
            
            _inputActions = new PlayerInputActions();
        }

        protected override void OnStartRunning()
        {
            _inputActions.Enable();
        }

        protected override void OnUpdate()
        {
            var currentMoveInput = _inputActions.MainCharacter.Move.ReadValue<Vector2>();

            var moveInput = new PlayerMoveInput { Value = currentMoveInput };
            SystemAPI.SetSingleton(moveInput);
        }
  
        protected override void OnStopRunning()
        {
            _inputActions.Disable();
        }
    }
}
