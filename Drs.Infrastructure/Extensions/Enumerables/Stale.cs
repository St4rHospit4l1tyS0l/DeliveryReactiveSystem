using System;

namespace Drs.Infrastructure.Extensions.Enumerables
{
    class Stale<T> : IStale<T>
    {
        private readonly T _update;

        public Stale() : this(true, default(T))
        {
        }

        public Stale(T update) : this(false, update)
        {
        }

        private Stale(bool isStale, T update)
        {
            IsStale = isStale;
            _update = update;
        }

        public bool IsStale { get; private set; }

        public T Data
        {
            get
            {
                if (IsStale)
                    throw new InvalidOperationException("Instancia de stale no tiene actualización.");
                return _update;
            }
        }
    }
}