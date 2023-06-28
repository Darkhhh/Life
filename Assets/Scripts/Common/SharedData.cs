using System.Collections.Generic;

namespace Common
{
    public class SharedData
    {
        public bool CreateNewFood { get; set; }
        
        public bool CheckOnBeingsIntersection { get; set; }

        public bool GameOver { get; set; } = false;
        
        public List<(float, float)> NewBeingsCoordinates { get; set; }
        
        public StartUpParameters Parameters { get; set; }

        public int MovesNumber { get; set; } = 0;

        public SharedData()
        {
            NewBeingsCoordinates = new List<(float, float)>(200);
            MovesNumber = 0;
        }
    }
}
