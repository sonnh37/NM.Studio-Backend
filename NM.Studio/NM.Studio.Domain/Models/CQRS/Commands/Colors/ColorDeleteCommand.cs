using NM.Studio.Domain.Models.CQRS.Commands.Base;

namespace NM.Studio.Domain.Models.CQRS.Commands.Colors;

public class ColorDeleteCommand : DeleteCommand
{
    public string? Name { get; set; }
    public string? ColorCode { get; set; }
    public string? ColorType { get; set; }
    public string? Description { get; set; }
    public string? ImagePath { get; set; }
    public bool IsActive { get; set; } = true;
    public int SortOrder { get; set; }
}