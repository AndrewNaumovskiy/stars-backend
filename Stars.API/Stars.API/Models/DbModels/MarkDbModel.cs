using System.ComponentModel.DataAnnotations.Schema;

namespace Stars.API.Models.DbModels;

[Table("marks")]
public class MarkDbModel
{
    public int Id { get; set; }
    public int StudentFk { get; set; }
    public int MarkType { get; set; }
    public DateTime DateSet { get; set; }

    public StudentDbModel Student { get; set; }
}
