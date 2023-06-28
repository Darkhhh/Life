using System;
using Common;
using Common.Views;
using ECS.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MyPooler;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ECS.Systems
{
    public class BeingsCreationSystem : IEcsInitSystem, IEcsRunSystem
    {
        #region Private Values

        private EcsWorld _world;
        private SharedData _sharedData;
        private readonly EcsCustomInject<ObjectPooler> _objectPooler = default;

        private int _createdBeings = 0;

        #endregion
        
        
        //private readonly EcsPoolInject<BeingComponent> _beingPool = default;

        //public BeingsCreationSystem(EcsWorld world) => _world = world;

        #region ECS

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _sharedData = systems.GetShared<SharedData>();
        }
        
        public void Run(IEcsSystems systems)
        {
            if (_sharedData.NewBeingsCoordinates.Count < 1) return;

            var pool = _world.GetPool<BeingComponent>();
            Debug.Log("Creating beings");
            
            foreach (var coordinate in _sharedData.NewBeingsCoordinates)
            {
                CreateBeing(pool, new Vector3(coordinate.Item1, coordinate.Item2, 0));
            }
            
            _sharedData.NewBeingsCoordinates.Clear();
        }

        #endregion
        
        
        

        
        private void CreateBeing(EcsPool<BeingComponent> components,  Vector3 coordinates)
        {
            var entity = _world.NewEntity();
            ref var being = ref components.Add(entity);
            
            var view = _objectPooler.Value.GetFromPool(
                Strings.BeingPoolTag,
                coordinates,
                Quaternion.identity)
                .GetComponent<BeingView>();

            being.View = view;
            being.Saturation = _sharedData.Parameters.initialSaturation;
            being.Moves = 0;
            being.Sex = _createdBeings % 2 == 0 ? Sex.Male : Sex.Female;
            
            being.View.ChangeSexVisualisation(being.Sex);
            being.View.ChangeLocalScale(VirtualQuad.GetScale(_sharedData.Parameters.beingRadius));

            _createdBeings++;
        }

        
    }
}
