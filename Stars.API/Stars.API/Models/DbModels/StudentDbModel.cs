using System.ComponentModel.DataAnnotations.Schema;

namespace Stars.API.Models.DbModels;

[Table("students")]
public class StudentDbModel
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    public int GroupFk { get; set; }

    public GroupDbModel Group { get; set; }
}
