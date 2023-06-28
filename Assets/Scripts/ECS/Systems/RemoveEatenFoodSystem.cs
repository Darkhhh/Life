using Common;
using ECS.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MyPooler;

namespace ECS.Systems
{
    public class RemoveEatenFoodSystem: IEcsInitSystem, IEcsRunSystem
    {
        #region Private Values

        private EcsWorld _world;
        private readonly EcsCustomInject<ObjectPooler> _objectPooler = default;

        #endregion
        
        
        #region ECS

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
        }

        public void Run(IEcsSystems systems)
        {
            var filter = _world.Filter<FoodComponent>().Inc<EatenFoodComponent>().End();
            var pool = _world.GetPool<FoodComponent>();
            var eatenFoodPool = _world.GetPool<EatenFoodComponent>();

            foreach (var entity in filter)
            {
                ref var food = ref pool.Get(entity);
                _objectPooler.Value.ReturnToPool(Strings.FoodPoolTag, food.View.gameObject);
                pool.Del(entity);
                eatenFoodPool.Del(entity);
                _world.DelEntity(entity);
            }
        }

        #endregion
    }
}
