using System;
using System.Collections.Generic;
using System.Text;

namespace CoreSolution.Domain.Entities.Base
{
    /// <summary>
    /// 定义主键类型为Int的基类Entity
    /// </summary>
    public abstract class Entity : Entity<int>, IEntity
    {

    }

    /// <summary>
    /// 定义基类泛型Entity
    /// </summary>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    public abstract class Entity<TPrimaryKey> : IEntity<TPrimaryKey>
    {
        public virtual TPrimaryKey Id { get; set; }
        public virtual bool IsDeleted { get; set; }


        public override bool Equals(object obj)
        {
            if (!(obj is Entity<TPrimaryKey>))
            {
                return false;
            }

            //同样的实例必须是认为是相等的
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            var other = (Entity<TPrimaryKey>)obj;
            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return Id == null ? 0 : Id.GetHashCode();
        }

        public static bool operator ==(Entity<TPrimaryKey> left, Entity<TPrimaryKey> right)
        {
            return Equals(left, null) ? Equals(right, null) : left.Equals(right);
        }

        public static bool operator !=(Entity<TPrimaryKey> left, Entity<TPrimaryKey> right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return $"[{GetType().Name} {Id}]";
        }
    }
}
