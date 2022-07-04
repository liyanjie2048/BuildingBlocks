using System;
using System.Linq;
using System.Reflection;

namespace Liyanjie.Linq.Internals
{
    internal class Methods
    {
        static readonly Func<MethodInfo, bool> _predicateHas2Parameters = _ => _.GetParameters()[1].ToString().Contains("Func`2");

        static MethodInfo GetMethod(
            string name,
            int parameterCount = 0,
            Func<MethodInfo, bool>? predicate = null)
        {
            return typeof(Queryable)
                .GetTypeInfo()
                .GetDeclaredMethods(name)
                .Single(_ => _.GetParameters().Length == parameterCount + 1 && (predicate is null || predicate(_)));
        }

        public static MethodInfo AllWithPredicate = GetMethod(nameof(Queryable.All), 1);
        public static MethodInfo Any = GetMethod(nameof(Queryable.Any));
        public static MethodInfo AnyWithPredicate = GetMethod(nameof(Queryable.Any), 1);
        public static MethodInfo Count = GetMethod(nameof(Queryable.Count));
        public static MethodInfo CountWithPredicate = GetMethod(nameof(Queryable.Count), 1);
        public static MethodInfo Distinct = GetMethod(nameof(Queryable.Distinct));
        public static MethodInfo ElementAtWithIndex = GetMethod(nameof(Queryable.ElementAt), 1);
        public static MethodInfo ElementAtOrDefaultWithIndex = GetMethod(nameof(Queryable.ElementAtOrDefault), 1);
        public static MethodInfo First = GetMethod(nameof(Queryable.First));
        public static MethodInfo FirstWithPredicate = GetMethod(nameof(Queryable.First), 1);
        public static MethodInfo FirstOrDefault = GetMethod(nameof(Queryable.FirstOrDefault));
        public static MethodInfo FirstOrDefaultWithPredicate = GetMethod(nameof(Queryable.FirstOrDefault), 1);
        public static MethodInfo GroupByWithKeySelector = GetMethod(nameof(Queryable.GroupBy), 1);
        public static MethodInfo Last = GetMethod(nameof(Queryable.Last));
        public static MethodInfo LastWithPredicate = GetMethod(nameof(Queryable.Last), 1);
        public static MethodInfo LastOrDefault = GetMethod(nameof(Queryable.LastOrDefault));
        public static MethodInfo LastOrDefaultWithPredicate = GetMethod(nameof(Queryable.LastOrDefault), 1);
        public static MethodInfo Max = GetMethod(nameof(Queryable.Max));
        public static MethodInfo MaxWithSelector = GetMethod(nameof(Queryable.Max), 1);
        public static MethodInfo Min = GetMethod(nameof(Queryable.Min));
        public static MethodInfo MinWithSelector = GetMethod(nameof(Queryable.Min), 1);
        public static MethodInfo OrderByWithSelector = GetMethod(nameof(Queryable.OrderBy), 1);
        public static MethodInfo OrderByDescendingWithSelector = GetMethod(nameof(Queryable.OrderByDescending), 1);
        public static MethodInfo Reverse = GetMethod(nameof(Queryable.Reverse));
        public static MethodInfo SelectWithSelector = GetMethod(nameof(Queryable.Select), 1, _predicateHas2Parameters);
        public static MethodInfo Single = GetMethod(nameof(Queryable.Single));
        public static MethodInfo SingleWithPredicate = GetMethod(nameof(Queryable.Single), 1);
        public static MethodInfo SingleOrDefault = GetMethod(nameof(Queryable.SingleOrDefault));
        public static MethodInfo SingleOrDefaultWithPredicate = GetMethod(nameof(Queryable.SingleOrDefault), 1);
        public static MethodInfo SkipWithCount = GetMethod(nameof(Queryable.Skip), 1);
        public static MethodInfo SkipWhileWithPredicate = GetMethod(nameof(Queryable.SkipWhile), 1, _predicateHas2Parameters);
        //public static MethodInfo Sum = GetMethod(nameof(Queryable.Sum));
        //public static MethodInfo SumWithSelector = GetMethod(nameof(Queryable.Sum), 1);
        public static MethodInfo TakeWithCount = GetMethod(nameof(Queryable.Take), 1);
        public static MethodInfo TakeWhileWithPredicate = GetMethod(nameof(Queryable.TakeWhile), 1, _predicateHas2Parameters);
        public static MethodInfo ThenByWithSelector = GetMethod(nameof(Queryable.ThenBy), 1);
        public static MethodInfo ThenByDescendingWithSelector = GetMethod(nameof(Queryable.ThenByDescending), 1);
        public static MethodInfo WhereWithPredicate = GetMethod(nameof(Queryable.Where), 1, _predicateHas2Parameters);
    }
}
