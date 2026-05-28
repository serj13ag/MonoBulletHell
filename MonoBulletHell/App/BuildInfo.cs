using System.Reflection;

namespace MonoBulletHell.App;

public static class BuildInfo
{
    public static string GetVersion(bool shortVersion = false)
    {
        var fullVersion = Assembly.GetExecutingAssembly()
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion;

        if (string.IsNullOrEmpty(fullVersion))
        {
            return "unknown";
        }

        var version = shortVersion ? fullVersion.Split('+')[0] : fullVersion;

#if DEBUG
        version += "-debug";
#endif

        return version;
    }
}