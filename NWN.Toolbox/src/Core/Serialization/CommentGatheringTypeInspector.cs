using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.TypeInspectors;

namespace Jorteck.Toolbox.Core
{
  internal sealed class CommentGatheringTypeInspector(ITypeInspector innerTypeDescriptor) : TypeInspectorSkeleton
  {
    private readonly ITypeInspector innerTypeDescriptor = innerTypeDescriptor ?? throw new ArgumentNullException(nameof(innerTypeDescriptor));

    public override string GetEnumName(Type enumType, string name)
    {
      return innerTypeDescriptor.GetEnumName(enumType, name);
    }

    public override string GetEnumValue(object enumValue)
    {
      return innerTypeDescriptor.GetEnumValue(enumValue);
    }

    public override IEnumerable<IPropertyDescriptor> GetProperties(Type type, object? container)
    {
      return innerTypeDescriptor
        .GetProperties(type, container)
        .Select(d => new CommentsPropertyDescriptor(d));
    }

    private sealed class CommentsPropertyDescriptor(IPropertyDescriptor baseDescriptor) : IPropertyDescriptor
    {
      // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local
      public string Name { get; set; } = baseDescriptor.Name;

      public Type Type => baseDescriptor.Type;

      public bool Required => baseDescriptor.Required;

      public Type? ConverterType => baseDescriptor.ConverterType;

      public bool AllowNulls => baseDescriptor.AllowNulls;

      public Type? TypeOverride
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

      public void Write(object target, object? value)
      {
        baseDescriptor.Write(target, value);
      }

      public T? GetCustomAttribute<T>() where T : Attribute
      {
        return baseDescriptor.GetCustomAttribute<T>();
      }

      public IObjectDescriptor Read(object target)
      {
        DescriptionAttribute? description = baseDescriptor.GetCustomAttribute<DescriptionAttribute>();
        return description != null ? new CommentsObjectDescriptor(baseDescriptor.Read(target), description.Description) : baseDescriptor.Read(target);
      }
    }
  }
}
