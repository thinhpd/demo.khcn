using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using QNews.Data.Admin;

namespace QNews.Data
{
    /// <summary>
    /// Class build câu truy vấn mở rộng
    /// </summary>
    public static class QueryExtensions
    {

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        /// <summary>
        /// Truy vấn Order by
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static IQueryable<T> SortBy<T>(this IQueryable<T> source, string propertyName)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            // DataSource control passes the sort parameter with a direction
            // if the direction is descending          
            int descIndex = propertyName.IndexOf(" DESC");
            if (descIndex >= 0)
            {
                propertyName = propertyName.Substring(0, descIndex).Trim();
            }

            if (String.IsNullOrEmpty(propertyName))
            {
                return source;
            }

            ParameterExpression parameter = Expression.Parameter(source.ElementType, String.Empty);
            MemberExpression property = Expression.Property(parameter, propertyName);
            LambdaExpression lambda = Expression.Lambda(property, parameter);

            string methodName = (descIndex < 0) ? "OrderBy" : "OrderByDescending";

            Expression methodCallExpression = Expression.Call(typeof(Queryable), methodName,
                                                new Type[] { source.ElementType, property.Type },
                                                source.Expression, Expression.Quote(lambda));
            return source.Provider.CreateQuery<T>(methodCallExpression);
        }

        /// <summary>
        /// Hàm lấy qua GridRequest
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public static IQueryable<T> SelectByRequest<T>(this IQueryable<T> source, ParramRequest request, ref int totalRecord)
        {
            Expression methodCallExpression = source.Expression;
            string propertyName;
            string methodName;
            ParameterExpression parameter;
            MemberExpression property;
            LambdaExpression lambda;

            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (!string.IsNullOrEmpty(request.Keyword) && request.SearchInField.Count > 0)
            {
                foreach (var propSearch in request.SearchInField)
                {
                    source = source.Has(propSearch, request.Keyword);
                }
            }

            #region dùng cho việc Order
            if (string.IsNullOrEmpty(request.FieldSort))
            {
                request.FieldSort = source.ElementType.GetProperties()[0].Name;
                request.TypeSort = true;
            }

            propertyName = request.FieldSort;
            methodName = (request.TypeSort) ? "OrderByDescending" : "OrderBy";

            parameter = Expression.Parameter(source.ElementType, String.Empty);
            property = Expression.Property(parameter, propertyName);
            lambda = Expression.Lambda(property, parameter);
            methodCallExpression = Expression.Call(typeof(Queryable), methodName,
                                            new Type[] { source.ElementType, property.Type },
                                            source.Expression, Expression.Quote(lambda));
            source = source.Provider.CreateQuery<T>(methodCallExpression);
            #endregion

            totalRecord = source.Count();

            if (request.CurrentPage > 0 && request.RowPerPage > 0)
            {

                methodCallExpression = Expression.Call(
                    typeof(Queryable), "Skip",
                    new Type[] { source.ElementType },
                    source.Expression, Expression.Constant((request.CurrentPage - 1) * request.RowPerPage));
                source = source.Provider.CreateQuery<T>(methodCallExpression);

                methodCallExpression = Expression.Call(
                    typeof(Queryable), "Take",
                    new Type[] { source.ElementType },
                    source.Expression, Expression.Constant(request.RowPerPage));
                source = source.Provider.CreateQuery<T>(methodCallExpression);
            }
            return source;
        }


        //public static IQueryable<T> Has<T>(this IQueryable<T> source, List<string> propertyNames, string keyword)
        //{
        //    if (source == null || propertyNames == null || propertyNames.Count == 0 || string.IsNullOrEmpty(keyword))
        //    {
        //        return source;
        //    }
        //    keyword = keyword.ToLower();
        //    MethodCallExpression returnMethodCallExpression = source.Expression.;
        //    foreach (var propertyName in propertyNames)
        //    {
        //        var parameter = Expression.Parameter(source.ElementType, String.Empty);
        //        var property = Expression.Property(parameter, propertyName);
        //        var CONTAINS_METHOD = typeof(string).GetMethod("Contains", new[] { typeof(string) });
        //        var TO_LOWER_METHOD = typeof(string).GetMethod("ToLower", new Type[] { });

        //        var toLowerExpression = Expression.Call(property, TO_LOWER_METHOD);
        //        var termConstant = Expression.Constant(keyword, typeof(string));
        //        var containsExpression = Expression.Call(toLowerExpression, CONTAINS_METHOD, termConstant);

        //        var predicate = Expression.Lambda<Func<T, bool>>(containsExpression, parameter);

        //        var methodCallExpression = Expression.Call(typeof(Queryable), "Where",
        //                                    new Type[] { source.ElementType },
        //                                    source.Expression, Expression.Quote(predicate));
        //    }
        //    return source.Provider.CreateQuery<T>(returnMethodCallExpression);
        //}



        public static IQueryable<T> Has<T>(this IQueryable<T> source, string propertyName, string keyword)
        {
            if (source == null || string.IsNullOrEmpty(propertyName) || string.IsNullOrEmpty(keyword))
            {
                return source;
            }
            keyword = keyword.ToLower();

            var parameter = Expression.Parameter(source.ElementType, String.Empty);
            var property = Expression.Property(parameter, propertyName);
            var CONTAINS_METHOD = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            var TO_LOWER_METHOD = typeof(string).GetMethod("ToLower", new Type[] { });

            var toLowerExpression = Expression.Call(property, TO_LOWER_METHOD);
            var termConstant = Expression.Constant(keyword, typeof(string));
            var containsExpression = Expression.Call(toLowerExpression, CONTAINS_METHOD, termConstant);

            var predicate = Expression.Lambda<Func<T, bool>>(containsExpression, parameter);

            var methodCallExpression = Expression.Call(typeof(Queryable), "Where",
                                        new Type[] { source.ElementType },
                                        source.Expression, Expression.Quote(predicate));


            return source.Provider.CreateQuery<T>(methodCallExpression);
        }

    }
}
