using System;
using UnityEngine;

namespace CubeGame.Localization
{
    [Serializable]
    public sealed class LocalizedStringEntry
    {
        [SerializeField] private string key;
        [SerializeField] private string locale;

        public string Key => key;
        public string Locale => locale;
    }
}
