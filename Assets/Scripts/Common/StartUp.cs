using System.Collections.Generic;
using ECS.Components;
using ECS.Systems;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MyPooler;
using UnityEngine;

namespace Common
{
    public class StartUp : MonoBehaviour
    {
        #region Serialize Fields

        [SerializeField] private ObjectPooler objectPooler;

        [SerializeField] private StartUpParameters parameters;

        #endregion

        
        #region Private Values

        private EcsWorld _world;
        private EcsSystems _systems;
        private SharedData _sharedData;

        private int _modelingTime = 0;

        #endregion


        #region Event Functions

        private void Start()
        {
            InitializeParameters();

            _systems = new EcsSystems(_world, _sharedData);
            var beingsPool = _world.GetPool<BeingComponent>();
            var foodPool = _world.GetPool<FoodComponent>();
            
            _systems
                .Add(new BeingsCreationSystem()) 
                .Add(new FoodCreationSystem())
                .Add(new BeingsMoveSystem()) //+
                .Add(new CheckOnIntersectionBeingsSystem()) //+
                .Add(new CheckOnIntersectionFoodSystem())
                .Add(new FeedBeingsSystem())
                .Add(new RemoveDeadBeingsSystem())
                .Add(new RemoveEatenFoodSystem()) //+
                .Add(new CheckOnEndGameSystem())
                .Inject(objectPooler, beingsPool, foodPool)
                .Init();
        }
        
        private void Update()
        {
            if (_sharedData.GameOver) return;
            _systems?.Run();

            _modelingTime++;
            if (_modelingTime > 2000) _modelingTime = 1000;
            
            if(_modelingTime % _sharedData.Parameters.iterationsBeforeBeingsIntersectionCheck == 0) 
                _sharedData.CheckOnBeingsIntersection = true;
            if (_modelingTime % _sharedData.Parameters.iterationsBeforeNewFoodCreation == 0) 
                _sharedData.CreateNewFood = true;

            if (_modelingTime < 65) _sharedData.CheckOnBeingsIntersection = true;
        }

        private void OnDestroy() {
            if (_systems != null) {
                _systems.Destroy ();
                _systems = null;
            }
            if (_world != null) {
                _world.Destroy ();
                _world = null;
            }
        }

        #endregion


        #region Private Methods

        private void InitializeParameters()
        {
            VirtualQuad.Initialize(parameters.sceneWidth, parameters.sceneHeight,
                parameters.boxWidth, parameters.boxHeight);

            var positions = VirtualQuad.GetFirstPositions(parameters.beingRadius);
            
            _world = new EcsWorld();

            _sharedData = new SharedData
            {
                CreateNewFood = true,
                CheckOnBeingsIntersection = true,
                NewBeingsCoordinates = new List<(float, float)>(200)
                {
                    (positions.Item1.x, positions.Item1.y),
                    (positions.Item2.x, positions.Item2.y)
                },
                Parameters = parameters,
                MovesNumber = 0,
                GameOver = false
            };
        }

        #endregion
    }
}
