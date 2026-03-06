using Assets._Project.Develop.Runtime.Meta.Features.Combinations;
using Assets._Project.Develop.Runtime.Utilities.SceneManagment;

namespace Assets._Project.Develop.Runtime.GamePlay.Infrastructure
{
    public class GameplayInputArgs : IInputSceneArgs
    {
        public GameplayInputArgs(TypesGameModes mode)
        {
            GameMode = mode;
        }

        public TypesGameModes GameMode { get; private set; }
    }
}
