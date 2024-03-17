using System.Linq.Expressions;
using AutoFixture.Dsl;

namespace Tests.Extensions;

public static class IPostprocessComposerExtensions
{
    
    public static IPostprocessComposer<T> WithEmpty<T, TElement>(this IPostprocessComposer<T> composer, Expression<Func<T, IReadOnlySet<TElement>>> func)
    {
        return composer.With(func, () => new HashSet<TElement>());
    }
    
    public static IPostprocessComposer<T> WithEmpty<T, TElement>(this IPostprocessComposer<T> composer, Expression<Func<T, IList<TElement>>> func)
    {
        return composer.With(func, Array.Empty<TElement>);
    }
    
    public static IPostprocessComposer<T> WithEmpty<T, TElement>(this IPostprocessComposer<T> composer, Expression<Func<T, IReadOnlyCollection<TElement>?>> func)
    {
        return composer.With(func, Array.Empty<TElement>);
    }
}