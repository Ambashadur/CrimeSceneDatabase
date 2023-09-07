using System;

namespace CSD.Domain.Entities;

public abstract class EntityBase
{
    public long Id { get; set; }

    public DateTimeOffset CreateDate { get; set; }

    public DateTimeOffset UpdateDate { get; set; }
}
