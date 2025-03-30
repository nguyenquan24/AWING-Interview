namespace TreasureHunt.Core.Entities;

public class TreasureMap
{
    public int Id { get; set; }
    public int N { get; set; }
    public int M { get; set; }
    public int P { get; set; }
    public string MatrixData { get; set; }
    public double Result { get; set; }
    public DateTime CreatedAt { get; set; }
}