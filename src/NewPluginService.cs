using Anvil.Services;

namespace Jorteck.Toolbox
{
  [ServiceBinding(typeof(NewPluginService))]
  public class NewPluginService
  {
    public NewPluginService()
    {
      // Your startup service code
    }
  }
}
