using Common;
using ECS.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MyPooler;

namespace ECS.Systems
{
    public class RemoveDeadBeingsSystem: IEcsInitSystem, IEcsRunSystem
    {
        #region Private Values

        private EcsWorld _world;
        private SharedData _sharedData;
        private readonly EcsPoolInject<BeingComponent> _beingPool = default;
        private readonly EcsCustomInject<ObjectPooler> _objectPooler = default;

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
                if (being.Saturation < 1 || being.Moves > _sharedData.Parameters.movesBeforeDeath)
                {
                    _objectPooler.Value.ReturnToPool(Strings.BeingPoolTag, being.View.gameObject);
                    _beingPool.Value.Del(entity);
                }
            }
        }

        #endregion
    }
}
