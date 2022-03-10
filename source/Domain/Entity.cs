using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Architecture.Domain;

public abstract class Entity<TId> : Base<Entity<TId>>
{
    public virtual TId Id { get; protected set; }

    protected sealed override IEnumerable<object> Equals()
    {
        yield return Id;
    }
}
