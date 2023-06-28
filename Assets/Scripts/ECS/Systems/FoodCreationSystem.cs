using Common;
using Common.Views;
using ECS.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using MyPooler;
using Random = UnityEngine.Random;

namespace ECS.Systems
{
    public class FoodCreationSystem :  IEcsInitSystem, IEcsRunSystem
    {
        #region Private Values

        private EcsWorld _world;
        private readonly EcsCustomInject<ObjectPooler> _objectPooler = default;
        private SharedData _sharedData;

        #endregion


        #region ECS

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _sharedData = systems.GetShared<SharedData>();
        }
        
        
        public void Run(IEcsSystems systems)
        {
            if (!_sharedData.CreateNewFood) return;
            
            CreateFood();
            Debug.Log("Creating food");
            _sharedData.CreateNewFood = false;
        }

        #endregion


        #region Food Creating

        private void CreateFood()
        {
            var elements = Random.Range(
                _sharedData.Parameters.lowerLevelForFood, 
                _sharedData.Parameters.upperLevelForFood);
            
            for (var i = 0; i < elements; i++)
            {
                CreateFoodElement(VirtualQuad.GetRandomPosition());
            }
        }

        private void CreateFoodElement(Vector3 coordinates)
        {
            var entity = _world.NewEntity();
            var components = _world.GetPool<FoodComponent>();
            ref var food = ref components.Add(entity);
            
            var view = _objectPooler.Value.GetFromPool(
                    Strings.FoodPoolTag,
                    coordinates, 
                    Quaternion.identity)
                .GetComponent<FoodView>();

            food.View = view;
            food.View.ChangeLocalScale(VirtualQuad.GetScale(_sharedData.Parameters.beingRadius));
        }

        #endregion
    }
}
