using System.Threading.Tasks;
using Anvil.API;
using Anvil.API.Events;

namespace Jorteck.Toolbox.Core
{
  public abstract class DialogPopupController<T> : WindowController<T> where T : WindowView<T>, IDialogView, new()
  {
    private TaskCompletionSource<DialogResult> taskCompletionSource;

    public DialogResult DialogResult { get; private set; }

    public async Task<DialogResult> WaitForDialogResult()
    {
      taskCompletionSource ??= new TaskCompletionSource<DialogResult>();
      return await taskCompletionSource.Task;
    }

    public override void ProcessEvent(ModuleEvents.OnNuiEvent eventData)
    {
      switch (eventData.EventType)
      {
        case NuiEventType.Click:
          HandleButtonClick(eventData);
          break;
      }
    }

    protected override void OnClose()
    {
      if (DialogResult == DialogResult.Unknown)
      {
        HandleDialogResult(DialogResult.Close);
      }
    }

    private void HandleButtonClick(ModuleEvents.OnNuiEvent eventData)
    {
      if (eventData.ElementId == View.OkButton.Id)
      {
        HandleDialogResult(DialogResult.Ok);
      }
      else if (eventData.ElementId == View.CancelButton.Id)
      {
        HandleDialogResult(DialogResult.Cancel);
      }
    }

    private void HandleDialogResult(DialogResult dialogResult)
    {
      DialogResult = dialogResult;
      taskCompletionSource?.SetResult(dialogResult);

      if (dialogResult != DialogResult.Close && dialogResult != DialogResult.Unknown)
      {
        Close();
      }
    }
  }
}
