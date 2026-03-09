using Zenject;

namespace CubeGame.Localization
{
    public interface ILocalizationManager : IInitializable
    {
        string GetString(string key);
    }
}
