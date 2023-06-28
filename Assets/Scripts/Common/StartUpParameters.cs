using UnityEngine;

namespace Common
{
    public class StartUpParameters : MonoBehaviour
    {
        [Header("Параметры для системы пищи")]
        [SerializeField] public int lowerLevelForFood = 10;
        [SerializeField] public int upperLevelForFood = 100;
        
        [Space] 
        
        
        [Header("Параметры размеров")] 
        [SerializeField] public int boxWidth = 800;
        [SerializeField] public int boxHeight = 400;
        [SerializeField] public int sceneWidth = 16;
        [SerializeField] public int sceneHeight = 8;
        [SerializeField] public int beingRadius = 5;
        [SerializeField] public int beingMoveStep = 1;
        [Space]
        
        
        
        [Header("Общие параметры поведения особей")]
        [Tooltip("Сколько шагов особь может совершить за жизнь")]
        [SerializeField] public int movesBeforeDeath = 500;
        
        [Tooltip("Уровень насыщения за единицу пищи")]
        [SerializeField] public int saturationPerIteration = 20;
        
        [Tooltip("Изначальное насыщение особи")]
        [SerializeField] public int initialSaturation = 20;
        
        [Tooltip("Максимальное насыщение особи")]
        [SerializeField] public int maximumSaturation = 100;
        [Space]
        
        
        
        [Header("Параметры моделирования")]
        [Tooltip("Как часто создавать новую пищу")]
        [SerializeField] public int iterationsBeforeNewFoodCreation = 100;

        [Tooltip("Как часто проверять на пересечение особей")]
        [SerializeField] public int iterationsBeforeBeingsIntersectionCheck = 400;
    }
}
