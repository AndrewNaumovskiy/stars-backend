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

    public int IsFavorite { get; set; }
    public string Impression { get; set; }
    public string Telegram { get; set; }
    public string Phone { get; set; }

    public GroupDbModel Group { get; set; }
    public IList<MarkDbModel> Marks { get; set; } = [];
    public GroupDbModel HeadOfGroup { get; set; }
}
