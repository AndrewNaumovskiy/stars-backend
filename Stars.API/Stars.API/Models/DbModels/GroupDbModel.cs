using System.ComponentModel.DataAnnotations.Schema;

namespace Stars.API.Models.DbModels;

[Table("groups")]
public class GroupDbModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Year { get; set; }

    public IList<StudentDbModel> Students { get; set; } = [];
    public IList<ClassDbModel> Classes { get; set; } = [];
}
