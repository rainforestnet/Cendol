using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cendol.Model
{
    /// <summary>
    /// 2016.07.16
    /// The purpose of this class is to ensure all the inherited classes has and ID property
    /// to be used in repository class with lambda expression of selecting id.
    /// 
    /// Where(c => c.Id == id)
    /// </summary>
    public abstract class EntityBase
    {
        public long Id { get; protected set; }
    }
}
