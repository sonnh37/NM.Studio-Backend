using System.ComponentModel.DataAnnotations;
using NM.Studio.Domain.Entities.Bases;

namespace NM.Studio.Domain.Entities;
public class Color : BaseEntity
{
    public string Name { get; set; }
    public string ColorCode { get; set; }
    public string ColorType { get; set; }
    public string Description { get; set; }
    public string ImagePath { get; set; }
    public bool IsActive { get; set; } = true;
    public int SortOrder { get; set; }
    public virtual ICollection<ProductColor> ProductColors { get; set; } = new List<ProductColor>();
}