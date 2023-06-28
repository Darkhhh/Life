using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Common
{
    public static class VirtualQuad
    {
        #region Private Values

        private static float _virtualWidth = -1, _virtualHeight = -1;
        private static float _sceneWidth = -1, _sceneHeight = -1;
        private static float _widthRatio = -1, _heightRatio = -1;
        private static bool _initialized = false;

        #endregion
        
        
        public static void Initialize(float sceneWidth, float sceneHeight, float virtualWidth, float virtualHeight)
        {
            _widthRatio = sceneWidth / virtualWidth;
            _heightRatio = sceneHeight / virtualHeight;
            _virtualWidth = virtualWidth;
            _virtualHeight = virtualHeight;
            _sceneWidth = sceneWidth;
            _sceneHeight = sceneHeight;
            _initialized = true;
        }
        

        #region Public Static

        public static (Vector3, Vector3) GetFirstPositions(int rangeInPixels)
        {
            if (!_initialized) Debug.LogError("VirtualQuad is not initialized");

            var malePosition = new Vector3(0, 0, 0);
            var femalePosition = new Vector3(rangeInPixels * _widthRatio, rangeInPixels * _heightRatio, 0);

            return (malePosition, femalePosition);
        }

        public static Vector3 GetRandomPosition()
        {
            if (!_initialized) Debug.LogError("VirtualQuad is not initialized");
            
            var xSign = Random.Range(0, 2) == 0 ? -1 : 1;
            var x = Random.Range(0, (int) _virtualWidth / 2) * xSign;
            
            var ySign = Random.Range(0, 2) == 0 ? -1 : 1;
            var y = Random.Range(0, (int) _virtualHeight / 2) * ySign;
            
            return GetVirtualCoordinates(new Vector3(x, y, 0));
        }

        public static List<(float, float)> GetRandomPositionsAround(Vector3 position, int number, int rangeInPixels)
        {
            // TODO Доделать
            var result = new List<(float, float)>(number);
            for (var i = 0; i < number; i++)
            {
                var pos = GetRandomPosition();
                result.Add((pos.x, pos.y));
            }
            return result;
        }

        public static Vector3 MoveInRandomDirection(Vector3 position, int stepInPixels)
        {
            if (!_initialized) Debug.LogError("VirtualQuad is not initialized");

            while (true)
            {
                var direction = (Direction) Random.Range(0, 5);
                if (!CanBeMoved(position, direction, stepInPixels * _heightRatio)) continue;
                return position + direction.GetVector3() * stepInPixels * _heightRatio;
            }
        }

        public static bool DoesObjectsIntersect(Vector3 firstPosition, Vector3 secondPosition, int rangeInPixels)
        {
            return (float) Math.Sqrt((firstPosition.x - secondPosition.x) * (firstPosition.x - secondPosition.x) +
                                     (firstPosition.y - secondPosition.y) * (firstPosition.y - secondPosition.y)) <
                   rangeInPixels * _heightRatio;
        }

        public static Vector3 GetScale(int rangeInPixels)
        {
            return new Vector3((float) Math.Round(rangeInPixels * _widthRatio, 2),
                (float) Math.Round(rangeInPixels * _heightRatio, 2));
        }

        #endregion


        #region Private Static

        /// <summary>
        /// Check for available move
        /// </summary>
        /// <param name="position">Current scene position of an object</param>
        /// <param name="direction">Moving direction</param>
        /// <param name="step">Step in scene (step in pixels * ratio)</param>
        /// <returns></returns>
        private static bool CanBeMoved(Vector3 position, Direction direction, float step)
        {
            var newPosition = position + direction.GetVector3() * step;
            
            if((Math.Abs(newPosition.x) > _sceneWidth / 2) || (Math.Abs(newPosition.y) > _sceneHeight / 2))
                return false;
            return true;
        }
        
        private static Vector3 GetSceneCoordinates(Vector3 virtualCoordinates)
        {
            return new Vector3(virtualCoordinates.x / _widthRatio, virtualCoordinates.y / _heightRatio, 0);
        }
        
        private static Vector3 GetVirtualCoordinates(Vector3 sceneCoordinates)
        {
            return new Vector3(sceneCoordinates.x * _widthRatio, sceneCoordinates.y * _heightRatio, 0);
        }

        #endregion
    }
}
