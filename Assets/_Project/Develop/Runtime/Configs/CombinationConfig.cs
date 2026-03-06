using UnityEngine;

namespace Assets._Project.Develop.Runtime.Configs
{
    [CreateAssetMenu(fileName = "Configs", menuName = "Configs/CombinationConfig")]
    public class CombinationConfig : ScriptableObject
    {
        [field: SerializeField] public string NumberChars { get; set; }
        [field: SerializeField] public string LetterChars { get; set; }
        [field: SerializeField] public int LengthCombination { get; set; }
    }
}