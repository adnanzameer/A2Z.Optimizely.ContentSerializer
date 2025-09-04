﻿using System.ComponentModel.DataAnnotations;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;

namespace A2Z.Optimizely.ContentSerializer.Tests.MockContent;

[ContentType(DisplayName = "VideoBlock", GUID = "f4a40b95-798e-4400-ac79-7f3a4189d5c7", Description = "")]
public class VideoBlock : BlockData
{

    [CultureSpecific]
    [Display(
        Name = "Name",
        Description = "Name field's description",
        GroupName = SystemTabNames.Content,
        Order = 1)]
    public virtual string Name { get; set; }

    [Display(Name = "Url")]
    public virtual Url Url { get; set; }
}