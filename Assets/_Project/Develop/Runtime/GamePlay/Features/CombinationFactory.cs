using Assets._Project.Develop.Runtime.Configs;
using Assets._Project.Develop.Runtime.Meta.Features.Combinations;
using System.Text;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.GamePlay.Features
{
    public class CombinationFactory
    {  
        private CombinationConfig _config;
        private StringBuilder _stringBuilder = new();

        public CombinationFactory(CombinationConfig config) => _config = config;
        
        public ICombination CreateCombination(TypesGameModes mode)
        {         
            string chars = mode == TypesGameModes.Letter ? _config.LetterChars : _config.NumberChars;

            for (int i = 0; i < _config.LengthCombination; i++)           
                _stringBuilder.Append(chars[Random.Range(0, chars.Length)]);           
          
            string combinationString = _stringBuilder.ToString();

            _stringBuilder.Clear();

            return new Combination(combinationString);
        }
    }
}
