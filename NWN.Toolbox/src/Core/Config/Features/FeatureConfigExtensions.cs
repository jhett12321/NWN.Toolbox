namespace Jorteck.Toolbox.Core
{
  internal static class FeatureConfigExtensions
  {
    public static bool IsEnabled(this IFeatureConfig config)
    {
      return config != null && config.Enabled;
    }
  }
}
