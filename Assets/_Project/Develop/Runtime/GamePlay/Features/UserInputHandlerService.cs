using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using UnityEngine.InputSystem;

namespace Assets._Project.Develop.Runtime.GamePlay.Features
{
    public class UserInputHandlerService : IDisposable
    {
        private ReactiveVariable<string> _inputPlayer = new("");
        private ReactiveVariable<string> _expectedChars = new("");
        private ReactiveVariable<string> _informationText = new("");

        private int _currentCharIndex;
  
        public IReadOnlyVariable<string> InputPlayer => _inputPlayer;
        public IReadOnlyVariable<string> ExpectedChars => _expectedChars;
        public IReadOnlyVariable<string> InformationTexts => _informationText;

        public event Action WinGame;
        public event Action LossGame;

        public void SetString(string exceptedChars)
        {
            _informationText.Value = "Введите правильно символы, чтобы победить!";
            _expectedChars.Value = exceptedChars;
            Keyboard.current.onTextInput += ProcessInput;
        }

        public void Dispose()
        {
            _expectedChars.Value = "";
            _inputPlayer.Value = "";

            Keyboard.current.onTextInput -= ProcessInput;
        }

        private bool IsCorrectChar(char inputChar)
        {
            if (inputChar.ToString().ToUpper() == _expectedChars.Value[_currentCharIndex].ToString().ToUpper())
                return true;

            return false;
        }

        private void ProcessInput(char inputChar)
        {
            _inputPlayer.Value += inputChar;

            if (IsCorrectChar(inputChar))
            {
                _currentCharIndex++;

                if (_currentCharIndex >= ExpectedChars.Value.Length)
                {
                    _informationText.Value = "Вы победили! Нажмите пробел чтобы продолжить!";
                    WinGame?.Invoke();
                }
                return;
            }

            _informationText.Value = "Вы проиграли! Нажмите пробел чтобы продолжить!";
            LossGame?.Invoke();
        }

    }
}
