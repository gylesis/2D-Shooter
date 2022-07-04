using Fusion;
using UnityEngine;

namespace Project
{
    public struct NetworkInputData : INetworkInput
    {
        public bool FirePressed;
        public Vector2 Direction;
        public bool RightArrowPressed;
        public bool LeftArrowPressed;
    }
}