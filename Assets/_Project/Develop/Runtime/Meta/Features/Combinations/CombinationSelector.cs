using UnityEngine;

namespace Assets._Project.Develop.Runtime.Meta.Features.Combinations
{
    public class CombinationSelector
    {
        private TypesGameModes _defaultMode = TypesGameModes.Number;

        public bool TryGetSelectedModeType(out TypesGameModes selectedMode)
        {
            selectedMode = _defaultMode;

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                selectedMode = TypesGameModes.Number;        
                return true;
            }

            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                selectedMode = TypesGameModes.Letter;
                return true;
            }

            return false;
        }
    }
}