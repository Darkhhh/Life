using Common;
using ECS.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace ECS.Systems
{
    public class CheckOnEndGameSystem: IEcsInitSystem, IEcsRunSystem
    {
        #region Private Values

        private EcsWorld _world;
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
            var beingsFilter = _world.Filter<BeingComponent>().End();
            var foodFilter = _world.Filter<FoodComponent>().End();

            if (beingsFilter.GetEntitiesCount() < 1 || foodFilter.GetEntitiesCount() < 1)
                _sharedData.GameOver = true;
        }

        #endregion
    }
}
