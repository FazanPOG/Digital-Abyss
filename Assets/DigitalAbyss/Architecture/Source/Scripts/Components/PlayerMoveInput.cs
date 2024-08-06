using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace DigitalAbyss.Architecture.Source.Scripts.Components
{
    public struct PlayerMoveInput : IComponentData
    {
        public float2 Value;
    }
}
