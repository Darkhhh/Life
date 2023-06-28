using System.Collections.Generic;
using Common;
using ECS.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace ECS.Systems
{
    public class CheckOnIntersectionFoodSystem: IEcsInitSystem, IEcsRunSystem
    {
        #region Private Values

        private EcsWorld _world;
        private SharedData _sharedData;
        private readonly EcsPoolInject<BeingComponent> _beingPool = default;
        private readonly EcsPoolInject<FoodComponent> _foodPool = default;

        #endregion
        
        
        #region ECS

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _sharedData = systems.GetShared<SharedData>();
        }

        public void Run(IEcsSystems systems)
        {
            var beingsFilter = _world.Filter<BeingComponent>().End();
            var foodFilter = _world.Filter<FoodComponent>().End();
            
            var eatenFoodPool = _world.GetPool<EatenFoodComponent>();
            var saturatedBeings = _world.GetPool<FoodHasEatenComponent>();

            foreach (var beingEntity in beingsFilter)
            {
                ref var being = ref _beingPool.Value.Get(beingEntity);
                foreach (var foodEntity in foodFilter)
                {
                    ref var food = ref _foodPool.Value.Get(foodEntity);
                    
                    if (!VirtualQuad.DoesObjectsIntersect(
                            being.View.Coordinates,
                            food.Coordinates,
                            _sharedData.Parameters.beingRadius)) continue;

                    if (!eatenFoodPool.Has(foodEntity))
                    {
                        eatenFoodPool.Add(foodEntity);
                        saturatedBeings.Add(beingEntity);
                    }
                    break;
                }
            }
            
            
            // var beingsFilter = _world.Filter<BeingComponent>().End();
            // var beings = new List<BeingComponent>(beingsFilter.GetEntitiesCount());
            //
            // foreach (var entity in beingsFilter) {
            //     ref var being = ref _beingPool.Value.Get(entity);
            //     beings.Add(being);
            // }
            //
            // var filter = _world.Filter<FoodComponent>().End();
            // var foodPool = _world.GetPool<FoodComponent>();
            //
            // var eatenFoodPool = _world.GetPool<EatenFoodComponent>();
            //
            // foreach (var entity in filter)
            // {
            //     ref var food = ref foodPool.Get(entity);
            //     if(CheckOnIntersection(food, beings)) eatenFoodPool.Add(entity);
            // }
        }

        #endregion


        private bool CheckOnIntersection(FoodComponent food, List<BeingComponent> beings)
        {
            for (var i = 0; i < beings.Count; i++)
            {
                var being = beings[i];
                if (!VirtualQuad.DoesObjectsIntersect(
                        being.View.Coordinates,
                        food.Coordinates,
                        _sharedData.Parameters.beingRadius)) continue;
                being.Saturation += _sharedData.Parameters.saturationPerIteration;
                beings[i] = being;
                return true;
            }
            
            return false;
        }
    }
}
