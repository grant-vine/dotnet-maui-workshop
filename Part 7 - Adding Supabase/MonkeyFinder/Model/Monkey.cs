using Postgrest.Attributes;

namespace MonkeyFinder.Model;

[Table("monkeys")]
public class Monkey : SupaBaseModel
{
    [Column("Name")]
    public string Name { get; set; }

    [Column("Location")]
    public string Location { get; set; }

    [Column("Details")]
    public string Details { get; set; }

    [Column("Image")]
    public string Image { get; set; }

    [Column("Population")]
    public int Population { get; set; }

    [Column("Latitude")]
    public double Latitude { get; set; }

    [Column("Longitude")]
    public double Longitude { get; set; }
}