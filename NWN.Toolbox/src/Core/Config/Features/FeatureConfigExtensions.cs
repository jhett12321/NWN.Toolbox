namespace Jorteck.Toolbox.Config
{
  internal static class FeatureConfigExtensions
  {
    public static bool IsEnabled(this IFeatureConfig config)
    {
      return config != null && config.Enabled;
    }
  }
}
