﻿using System;

namespace A2Z.Optimizely.ContentSerializer.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class ContentSerializerWrapItemsAttribute : Attribute
{
    public ContentSerializerWrapItemsAttribute(bool wrapItems)
    {
        WrapItems = wrapItems;
    }

    public bool WrapItems { get; }
}