using Anvil.API;

namespace Jorteck.Toolbox.Core
{
  public interface IDialogView
  {
    public NuiButton OkButton { get; }
    public NuiButton CancelButton { get; }
  }
}
