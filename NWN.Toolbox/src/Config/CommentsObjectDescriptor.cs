using System;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace Jorteck.Toolbox
{
  internal sealed class CommentsObjectDescriptor : IObjectDescriptor
  {
    private readonly IObjectDescriptor innerDescriptor;

    public CommentsObjectDescriptor(IObjectDescriptor innerDescriptor, string comment)
    {
      this.innerDescriptor = innerDescriptor;
      this.Comment = comment;
    }

    public string Comment { get; private set; }

    public object Value
    {
      get => innerDescriptor.Value;
    }

    public Type Type
    {
      get => innerDescriptor.Type;
    }

    public Type StaticType
    {
      get => innerDescriptor.StaticType;
    }

    public ScalarStyle ScalarStyle
    {
      get => innerDescriptor.ScalarStyle;
    }
  }
}
