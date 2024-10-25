using System.ComponentModel.DataAnnotations;

namespace JSB_OnlineStore.Dtos;

public class AddProductDto
{
    [Length(5,25)]
    public string Name { get; set; } = string.Empty;
    [Length(10, 250)]
    public string Description { get; set; } = string.Empty;
    [Range(1,float.MaxValue)]
    public float Price { get; set; }
    [Range(1, int.MaxValue)]
    public int Stock { get; set; }
}
