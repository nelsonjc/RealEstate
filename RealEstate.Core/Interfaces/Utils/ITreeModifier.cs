using System.Linq.Expressions;

namespace RealEstate.Core.Interfaces.Utils
{
    /// <summary>
    /// Provides methods to combine expression trees with logical AND and OR operations.
    /// </summary>
    public interface ITreeModifier
    {
        /// <summary>
        /// Combines two expressions using a logical AND operation.
        /// </summary>
        /// <typeparam name="T">The type of the entity to which the filter expressions apply.</typeparam>
        /// <param name="filter1">The first filter expression.</param>
        /// <param name="filter2">The second filter expression.</param>
        /// <returns>An expression that represents the logical AND combination of the two filters.</returns>
        Expression<Func<T, bool>> CombineAnd<T>(Expression<Func<T, bool>> filter1, Expression<Func<T, bool>> filter2);

        /// <summary>
        /// Combines two expressions using a logical OR operation.
        /// </summary>
        /// <typeparam name="T">The type of the entity to which the filter expressions apply.</typeparam>
        /// <param name="filter1">The first filter expression.</param>
        /// <param name="filter2">The second filter expression.</param>
        /// <returns>An expression that represents the logical OR combination of the two filters.</returns>
        Expression<Func<T, bool>> CombineOr<T>(Expression<Func<T, bool>> filter1, Expression<Func<T, bool>> filter2);
    }

}
