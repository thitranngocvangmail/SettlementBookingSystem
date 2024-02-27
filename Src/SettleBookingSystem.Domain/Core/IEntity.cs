using System;
using System.Collections.Generic;

namespace SettleBookingSystem.Domain.Core
{
    [Serializable]
    public abstract class Entity<TKey> : Entity, IEntity<TKey>, IEntity
    {
        /// <inheritdoc />
        public virtual TKey Id { get; protected set; }

        protected Entity()
        {
        }

        protected Entity(TKey id)
        {
            Id = id;
        }

        public override object[] GetKeys()
        {
            return new object[1] { Id };
        }

   }

    /// <summary>
    /// Defines an entity. It's primary key may not be "Id" or it may have a composite primary key.
    /// Use <see cref="T:Volo.Abp.Domain.Entities.IEntity`1" /> where possible for better integration to repositories and other structures in the framework.
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Returns an array of ordered keys for this entity.
        /// </summary>
        /// <returns></returns>
        object[] GetKeys();
    }


    /// <inheritdoc />
    [Serializable]
    public abstract class Entity : IEntity
    {
        protected Entity()
        {
        }

        public abstract object[] GetKeys();
    }

    /// <summary>
    /// Defines an entity with a single primary key with "Id" property.
    /// </summary>
    /// <typeparam name="TKey">Type of the primary key of the entity</typeparam>
    public interface IEntity<TKey> : IEntity
    {
        /// <summary>
        /// Unique identifier for this entity.
        /// </summary>
        TKey Id { get; }
    }
}

