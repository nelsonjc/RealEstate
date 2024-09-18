using System.Linq.Expressions;

namespace RealEstate.Core.Interfaces.Utils
{
    /// <summary>
    /// Defines methods for replacing expressions within an expression tree.
    /// </summary>
    public interface IReplaceVisitor
    {
        /// <summary>
        /// Sets the expressions to be replaced and their replacements.
        /// </summary>
        /// <param name="from">The expression to be replaced.</param>
        /// <param name="to">The expression to replace with.</param>
        void SetData(Expression from, Expression to);

        /// <summary>
        /// Visits the expression tree and performs the replacement.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression with replacements, or the original expression if no replacement was needed.</returns>
        Expression Visit(Expression node);
    }

}
