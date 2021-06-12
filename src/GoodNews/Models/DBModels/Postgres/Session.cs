using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoodNews.Models.DBModels.Postgres
{
  [Table("uuids")]
  public class Session
  {
    [Key] [Column("uuid")] public string Uuid { get; set; }

    [Column("created_at")] public DateTime CreatedAt { get; set; }

    [Column("last_session")] public DateTime? LastSession { get; set; }
  }
}