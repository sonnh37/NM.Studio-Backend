
using Humanizer;
using Microsoft.AspNetCore.Routing;

namespace NM.Studio.Domain.Configs;

public class SlugifyParameterTransformer : IOutboundParameterTransformer
{
    public string? TransformOutbound(object? value)
    {
        if (value == null) return null;
        return value.ToString().Pluralize()?.ToLower();
    }
}