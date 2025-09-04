using System;

namespace A2Z.Optimizely.ContentSerializer.Attributes;

public class ContentSerializerNameAttribute : Attribute
{
    public ContentSerializerNameAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; }
}