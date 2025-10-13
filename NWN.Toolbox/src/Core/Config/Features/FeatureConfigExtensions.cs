using System.Diagnostics.CodeAnalysis;

namespace Jorteck.Toolbox.Core
{
  internal static class FeatureConfigExtensions
  {
    public static bool IsEnabled([NotNullWhen(true)] this IFeatureConfig? config)
    {
      return config != null && config.Enabled;
    }
  }
}
