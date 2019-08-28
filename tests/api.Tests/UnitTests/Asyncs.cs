using System;
using System.Threading.Tasks;

namespace MD.Salary.WebApi.Tests.UnitTests
{
    public class Asyncs
    {
        public static T GetAsyncActionResult<T>(Func<Task<T>> func)
        {
            var task = func();
            task.Wait();
            return task.Result;
        }
    }
}
