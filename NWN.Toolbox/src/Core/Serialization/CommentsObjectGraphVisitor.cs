using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.ObjectGraphVisitors;

namespace Jorteck.Toolbox.Core
{
  internal sealed class CommentsObjectGraphVisitor(IObjectGraphVisitor<IEmitter> nextVisitor) : ChainedObjectGraphVisitor(nextVisitor)
  {
    public override bool EnterMapping(IPropertyDescriptor key, IObjectDescriptor value, IEmitter context, ObjectSerializer serializer)
    {
      if (value is CommentsObjectDescriptor commentsDescriptor && commentsDescriptor.Comment != null)
      {
        context.Emit(new Comment(commentsDescriptor.Comment, false));
      }

      return base.EnterMapping(key, value, context, serializer);
    }
  }
}
