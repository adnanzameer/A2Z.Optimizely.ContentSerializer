namespace A2Z.Optimizely.ContentSerializer;

public interface IUrlSettings
{
    bool UseAbsoluteUrls { get; set; }
    bool FallbackToWildcard { get; set; }
}