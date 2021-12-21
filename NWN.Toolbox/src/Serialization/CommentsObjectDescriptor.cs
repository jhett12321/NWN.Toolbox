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
      Comment = comment;
    }

    public string Comment { get; set; }

    public object Value => innerDescriptor.Value;

    public Type Type => innerDescriptor.Type;

    public Type StaticType => innerDescriptor.StaticType;

    public ScalarStyle ScalarStyle => innerDescriptor.ScalarStyle;
  }
}
