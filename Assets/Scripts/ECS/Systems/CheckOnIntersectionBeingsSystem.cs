using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using ECS.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace ECS.Systems
{
    public class CheckOnIntersectionBeingsSystem : IEcsInitSystem, IEcsRunSystem
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
            if (!_sharedData.CheckOnBeingsIntersection) return;
            
            _sharedData.NewBeingsCoordinates.Clear();
            
            var filter = _world.Filter<BeingComponent>().End();
            var beings = new List<BeingComponent>(filter.GetEntitiesCount());

            foreach (var entity in filter)
            {
                ref var being = ref _beingPool.Value.Get(entity);
                beings.Add(being);
            }
            var temp = new List<(Sex, int, Vector3)>(beings.Count);
            temp.AddRange(beings.Select(being => (being.Sex, being.Saturation, being.View.Coordinates)));
            List<((int, Vector3), (int, Vector3))> intersectedBeings;

            if (beings.Count > 100)
            {
                var firstHalf = temp.GetRange(0, temp.Count / 2);
                var secondHalf = temp.GetRange(temp.Count / 2, temp.Count / 2);
                
                var task = new Task<List<((int, Vector3), (int, Vector3))>>(() => 
                    GetIntersectedEntities1(firstHalf, temp));
                task.Start();
                intersectedBeings = GetIntersectedEntities1(secondHalf, temp);
                var secondPart = task.Result;
                intersectedBeings.AddRange(secondPart);
            }
            else
            {
                intersectedBeings = GetIntersectedEntities1(temp, temp);
            }
            
            // if (beings.Count > 100)
            // {
            //     var firstHalf = beings.GetRange(0, beings.Count / 2);
            //     var secondHalf = beings.GetRange(beings.Count / 2, beings.Count / 2);
            //     
            //     var task = new Task<List<((int, Vector3), (int, Vector3))>>(() => 
            //         GetIntersectedEntities(firstHalf, beings));
            //     task.Start();
            //     
            //     intersectedBeings = GetIntersectedEntities(secondHalf, beings);
            //     
            //     var secondPart = task.Result;
            //     
            //     intersectedBeings.AddRange(secondPart);
            // }
            // else
            // {
            //     intersectedBeings = GetIntersectedEntities(beings, beings);
            // }
            
            var withoutDuplicates = new HashSet<((int, Vector3), (int, Vector3))>();
            foreach (var intersected in intersectedBeings)
            {
                withoutDuplicates.Add(intersected);
            }
            CreateNewBeingsCoordinates(withoutDuplicates.ToList());
            
            //CreateNewBeingsCoordinates(intersectedBeings);

            _sharedData.CheckOnBeingsIntersection = false;
        }

        #endregion


        #region Private Methods

        /// <summary>
        /// Возвращает список пар, подходящих для воспроизведения особей
        /// </summary>
        /// <param name="checkingEntities">Проверяемые особи</param>
        /// <param name="allEntities">Все особи</param>
        /// <returns>Возвращает (int, Vector3) обеих особей, где int - насыщение особи, Vector3 - ее координаты</returns>
        private List<((int, Vector3), (int, Vector3))> GetIntersectedEntities(List<BeingComponent> checkingEntities,
            List<BeingComponent> allEntities)
        {
            var result = new List<((int, Vector3), (int, Vector3))>(50);
            foreach (var being in checkingEntities)
            {
                foreach (var checkingBeing in allEntities)
                {
                    if (being.Sex != checkingBeing.Sex &&
                        VirtualQuad.DoesObjectsIntersect(
                            being.View.Coordinates, 
                            checkingBeing.View.Coordinates,
                            _sharedData.Parameters.beingRadius))
                    {
                        result.Add((
                            (being.Saturation, being.View.Coordinates), 
                            (checkingBeing.Saturation, checkingBeing.View.Coordinates)));
                    }
                        
                }
            }
            return result;
        }
        
        private List<((int, Vector3), (int, Vector3))> GetIntersectedEntities1(List<(Sex, int, Vector3)> checkingEntities,
            List<(Sex, int, Vector3)> allEntities)
        {
            var result = new List<((int, Vector3), (int, Vector3))>(50);
            foreach (var being in checkingEntities)
            {
                foreach (var checkingBeing in allEntities)
                {
                    if (being.Item1 != checkingBeing.Item1 &&
                        VirtualQuad.DoesObjectsIntersect(
                            being.Item3, 
                            checkingBeing.Item3,
                            _sharedData.Parameters.beingRadius))
                    {
                        result.Add((
                            (being.Item2, being.Item3), 
                            (checkingBeing.Item2, checkingBeing.Item3)));
                    }
                        
                }
            }
            return result;
        }

        private void CreateNewBeingsCoordinates(List<((int, Vector3), (int, Vector3))> intersectedBeings)
        {
            foreach (var item in intersectedBeings)
            {
                var m = item.Item1.Item1;
                var f = item.Item2.Item1;

                var numberOfNewBeings = 0;

                if (m <= 10 && f <= 10) numberOfNewBeings = 4;
                else if (m is > 10 and <= 20 || f is > 10 and <= 20) numberOfNewBeings = 3;
                else if (m is > 20 and <= 40 || f is > 20 and <= 40) numberOfNewBeings = 2;
                else if (m is > 40 and <= 60 || f is > 40 and <= 60) numberOfNewBeings = 1;
                
                if (numberOfNewBeings == 0) continue;
                
                _sharedData.NewBeingsCoordinates.AddRange(
                VirtualQuad.GetRandomPositionsAround(
                    item.Item1.Item2, 
                    numberOfNewBeings,
                    _sharedData.Parameters.beingRadius));
            }
        }

        #endregion
    }
}
