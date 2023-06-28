using Common;
using ECS.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace ECS.Systems
{
    public class BeingsMoveSystem: IEcsInitSystem, IEcsRunSystem
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
            var filter = _world.Filter<BeingComponent>().End();

            foreach (var entity in filter) {
                ref var being = ref _beingPool.Value.Get(entity);

                being.Moves++;
                being.Saturation--;
                being.View.Move(VirtualQuad.MoveInRandomDirection(
                    being.View.Coordinates, 
                    _sharedData.Parameters.beingMoveStep));
            }
            
            _sharedData.MovesNumber++;
        }

        #endregion
        
    }
}
