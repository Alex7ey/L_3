namespace Assets._Project.Develop.Runtime.Meta.Features.Combinations
{
    public class Combination : ICombination
    {
        public Combination(string charCombination)
        {
            Value = charCombination;
        }

        public string Value { get; }
    }
}
