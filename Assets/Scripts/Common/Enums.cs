using UnityEngine;

namespace Common
{
    public enum Sex
    {
        Male, Female
    }


    public enum Direction
    {
        North, East, South, West
    }


    public static class EnumExtensions
    {
        public static Vector3 GetVector3(this Direction direction)
        {
            return direction switch
            {
                Direction.North => Vector3.up,
                Direction.East => Vector3.right,
                Direction.South => Vector3.down,
                Direction.West => Vector3.left,
                _ => Vector3.zero
            };
        }
    }
}
