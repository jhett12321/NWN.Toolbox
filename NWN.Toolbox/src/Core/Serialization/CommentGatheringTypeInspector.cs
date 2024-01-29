using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.TypeInspectors;

namespace Jorteck.Toolbox.Core
{
  internal sealed class CommentGatheringTypeInspector : TypeInspectorSkeleton
  {
    private readonly ITypeInspector innerTypeDescriptor;

    public CommentGatheringTypeInspector(ITypeInspector innerTypeDescriptor)
    {
      this.innerTypeDescriptor = innerTypeDescriptor ?? throw new ArgumentNullException(nameof(innerTypeDescriptor));
    }

    public override IEnumerable<IPropertyDescriptor> GetProperties(Type type, object container)
    {
      return innerTypeDescriptor
        .GetProperties(type, container)
        .Select(d => new CommentsPropertyDescriptor(d));
    }

    private sealed class CommentsPropertyDescriptor : IPropertyDescriptor
    {
      private readonly IPropertyDescriptor baseDescriptor;

      public CommentsPropertyDescriptor(IPropertyDescriptor baseDescriptor)
      {
        this.baseDescriptor = baseDescriptor;
        Name = baseDescriptor.Name;
      }

      // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local
      public string Name { get; set; }

      public Type Type => baseDescriptor.Type;

      public Type TypeOverride
      {
        get => baseDescriptor.TypeOverride;
        set => baseDescriptor.TypeOverride = value;
      }

      public int Order { get; set; }

      public ScalarStyle ScalarStyle
      {
        get => baseDescriptor.ScalarStyle;
        set => baseDescriptor.ScalarStyle = value;
      }

      public bool CanWrite => baseDescriptor.CanWrite;

      public void Write(object target, object value)
      {
        baseDescriptor.Write(target, value);
      }

      public T GetCustomAttribute<T>() where T : Attribute
      {
        return baseDescriptor.GetCustomAttribute<T>();
      }

      public IObjectDescriptor Read(object target)
      {
        DescriptionAttribute description = baseDescriptor?.GetCustomAttribute<DescriptionAttribute>();
        return description != null ? new CommentsObjectDescriptor(baseDescriptor.Read(target), description.Description) : baseDescriptor!.Read(target);
      }
    }
  }
}
