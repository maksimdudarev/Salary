using System.Collections.Generic;
using System.Linq;

namespace MD.Salary.Utilities
{
    class MemoizationCache
    {
        private Dictionary<long, decimal> CacheCollection { get; set; }
        public MemoizationCache() { CacheCollection = new Dictionary<long, decimal> { }; }
        public void Add(long key, decimal value)
        {
            if (!ContainsKey(key)) CacheCollection[key] = value;
        }
        public decimal GetValue(long key)
        {
            return CacheCollection[key];
        }
        public decimal GetSum()
        {
            return CacheCollection.Sum(item => item.Value);
        }
        public bool ContainsKey(long key)
        {
            return CacheCollection.ContainsKey(key);
        }
    }
}
