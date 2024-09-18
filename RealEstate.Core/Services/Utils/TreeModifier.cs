using RealEstate.Core.Interfaces.Utils;
using System.Linq.Expressions;

namespace RealEstate.Core.Services.Utils
{

    public class TreeModifier : ITreeModifier
    {
        private readonly IReplaceVisitor _replaceVisitor;

        public TreeModifier(IReplaceVisitor replaceVisitor)
        {
            _replaceVisitor = replaceVisitor;
        }

        ///<inheritdoc/>
        public Expression<Func<T, bool>> CombineAnd<T>(Expression<Func<T, bool>> filter1, Expression<Func<T, bool>> filter2)
        {
            // combine two predicates:
            // need to rewrite one of the lambdas, swapping in the parameter from the other

            _replaceVisitor.SetData(
                filter1.Parameters[0], filter2.Parameters[0]);

            var rewrittenBody1 = _replaceVisitor.Visit(filter1.Body);

            var newFilter = Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(rewrittenBody1, filter2.Body), filter2.Parameters);

            return newFilter;
        }

        ///<inheritdoc/>
        public Expression<Func<T, bool>> CombineOr<T>(Expression<Func<T, bool>> filter1, Expression<Func<T, bool>> filter2)
        {
            _replaceVisitor.SetData(
                filter1.Parameters[0], filter2.Parameters[0]);

            var rewrittenBody1 = _replaceVisitor.Visit(filter1.Body);

            var newFilter = Expression.Lambda<Func<T, bool>>(
                Expression.Or(rewrittenBody1, filter2.Body), filter2.Parameters);

            return newFilter;
        }
    }

}
