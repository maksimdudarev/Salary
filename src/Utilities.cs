using System.Collections.Generic;
using System.Linq;

namespace MD.Salary.Utilities
{
    class MemoizationCache
    {
        private Dictionary<int, decimal> CacheCollection { get; set; }
        public MemoizationCache() { CacheCollection = new Dictionary<int, decimal> { }; }
        public void Add(int key, decimal value)
        {
            if (!ContainsKey(key)) CacheCollection[key] = value;
        }
        public decimal GetValue(int key)
        {
            return CacheCollection[key];
        }
        public decimal GetSum()
        {
            return CacheCollection.Sum(item => item.Value);
        }
        public bool ContainsKey(int key)
        {
            return CacheCollection.ContainsKey(key);
        }
    }
}
