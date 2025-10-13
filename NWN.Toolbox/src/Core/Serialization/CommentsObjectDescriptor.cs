using System;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace Jorteck.Toolbox.Core
{
  internal sealed class CommentsObjectDescriptor(IObjectDescriptor innerDescriptor, string comment) : IObjectDescriptor
  {
    public string? Comment { get; set; } = comment;

    public object? Value => innerDescriptor.Value;

    public Type Type => innerDescriptor.Type;

    public Type StaticType => innerDescriptor.StaticType;

    public ScalarStyle ScalarStyle => innerDescriptor.ScalarStyle;
  }
}
