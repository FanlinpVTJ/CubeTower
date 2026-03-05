using CubeGame.Screen;
using UnityEngine;
using Zenject;

namespace CubeGame
{
    public class GameInitializer : MonoBehaviour
    {
        [Inject]
        private IScrollZone _rightZone;
        [Inject]
        private ILeftZone _leftZone;
        [Inject]
        private IScrollZone _scrollZone;

        [Inject]
        private void Construct()
        {
            _rightZone.Initialize();
            _leftZone.Initialize();
            _scrollZone.Initialize();
        }
    }
}
