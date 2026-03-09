using System.Collections.Generic;

namespace CubeGame.Localization
{
    public sealed class LocalizationManager : ILocalizationManager
    {
        private readonly LocalizationConfig localizationConfig;

        private Dictionary<string, string> localizedStrings;

        public LocalizationManager(LocalizationConfig localizationConfig)
        {
            this.localizationConfig = localizationConfig;
        }

        public void Initialize()
        {
            if (localizationConfig == null)
            {
                localizedStrings = null;

                return;
            }

            localizedStrings = BuildLocalizedStrings(localizationConfig.LocalizedStrings);
        }

        public string GetString(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return key;
            }

            if (localizedStrings == null)
            {
                return key;
            }

            if (!localizedStrings.TryGetValue(key, out string localizedValue))
            {
                return key;
            }

            return localizedValue;
        }

        private Dictionary<string, string> BuildLocalizedStrings(List<LocalizedStringEntry> localizedStringEntries)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            if (localizedStringEntries == null)
            {
                return result;
            }

            for (int i = 0; i < localizedStringEntries.Count; i++)
            {
                LocalizedStringEntry localizedStringEntry = localizedStringEntries[i];

                if (localizedStringEntry == null)
                {
                    continue;
                }

                if (string.IsNullOrEmpty(localizedStringEntry.Key))
                {
                    continue;
                }

                if (result.ContainsKey(localizedStringEntry.Key))
                {
                    result[localizedStringEntry.Key] = localizedStringEntry.Locale;
                    continue;
                }

                result.Add(localizedStringEntry.Key, localizedStringEntry.Locale);
            }

            return result;
        }
    }
}
