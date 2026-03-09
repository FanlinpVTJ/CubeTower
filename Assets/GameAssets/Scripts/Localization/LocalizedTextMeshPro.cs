using CubeGame.Localization;
using TMPro;
using UnityEngine;
using Zenject;

namespace CubeGame
{
    public class LocalizedTextMeshPro : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _text;
        [SerializeField]
        private string _key;
        [Inject]
        private ILocalizationManager _manager;

        private void Start()
        {
            _text.text = _manager.GetString(_key);
        }
    }
}
