using Common;
using ECS.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace ECS.Systems
{
    public class FeedBeingsSystem : IEcsInitSystem, IEcsRunSystem
    {
        #region Private Values

        private EcsWorld _world;
        private SharedData _sharedData;
        private readonly EcsPoolInject<BeingComponent> _beingPool = default;

        #endregion

        #region ECS

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _sharedData = systems.GetShared<SharedData>();
        }

        public void Run(IEcsSystems systems)
        {
            var filter = _world.Filter<BeingComponent>().Inc<FoodHasEatenComponent>().End();
            var feedPool = _world.GetPool<FoodHasEatenComponent>();

            foreach (var entity in filter) {
                ref var being = ref _beingPool.Value.Get(entity);

                being.Saturation += _sharedData.Parameters.saturationPerIteration;

                if (being.Saturation > _sharedData.Parameters.maximumSaturation)
                    being.Saturation = _sharedData.Parameters.maximumSaturation;
                
                feedPool.Del(entity);
            }
            
            _sharedData.MovesNumber++;
        }

        #endregion
    }
}
