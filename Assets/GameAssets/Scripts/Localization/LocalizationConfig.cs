using System.Collections.Generic;
using UnityEngine;

namespace CubeGame.Localization
{
    [CreateAssetMenu(menuName = "CubeGame/Localization/Config", fileName = "LocalizationConfig")]
    public sealed class LocalizationConfig : ScriptableObject
    {
        [SerializeField] private List<LocalizedStringEntry> localizedStrings = new List<LocalizedStringEntry>();

        public List<LocalizedStringEntry> LocalizedStrings => localizedStrings;
    }
}
