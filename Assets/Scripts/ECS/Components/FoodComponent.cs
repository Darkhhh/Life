using Common;
using Common.Views;
using UnityEngine;

namespace ECS.Components
{
    public struct FoodComponent
    {
        public FoodView View;

        public Vector3 Coordinates => View.Coordinates;
    }
}
