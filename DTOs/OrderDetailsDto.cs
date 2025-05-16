namespace Laboratorio09___Patrick_Hugo_Ayllón_Rubio.DTOs;

public class OrderDetailsDto
{
    public int OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public List<ProductDto> Products { get; set; }
}