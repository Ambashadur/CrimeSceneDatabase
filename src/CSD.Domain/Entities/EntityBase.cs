using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSD.Domain.Entities;

public abstract class EntityBase
{
    [Column("id")]
    public long Id { get; set; }

    [Column("create_date")]
    public DateTimeOffset CreateDate { get; set; }

    [Column("update_date")]
    public DateTimeOffset UpdateDate { get; set; }
}
