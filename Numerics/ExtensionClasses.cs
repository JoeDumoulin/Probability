using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Numerics
{
  public static class ExtensionClasses
  {
    /// <summary>
    /// Repeatedly apply a monoid operation to the consecutive elements of a container
    /// </summary>
    /// <typeparam name="T">A type that has monoid properties 
    /// (a zero element and an associative binary operation)</typeparam>
    /// <param name="summables">An enumerable container of objects of type T</param>
    /// <param name="mon">A monoid object that defines a zero element and 
    /// an associative binary operation on T</param>
    /// <returns>An object of type T formed by applying the monoid's 
    /// associative operations over each consecutive pair in the source.
    /// This is just an aggregation of an IEnumerable of the  monoid operation.</returns>
    public static T MonoidOp<T>(this IEnumerable<T> source, Monoid<T> mon)
    {
        return source.Aggregate(mon.Unit, mon.Op);
    }

    /// <summary>
    /// Generate the inner product of two containers of the same type. The
    /// inner product is constructed generically by specifying two monoid 
    /// operations  on the contained type:  one representing 'multiplication'  
    /// and another representing 'addition'.  Since the operations are monoids, 
    /// they return an object that is the same type as the contained objects.
    /// </summary>
    /// <typeparam name="T">A type that has monoid properties 
    /// (a zero element and an associative binary operation)</typeparam>
    /// <param name="left">the source container</param>
    /// <param name="right">another container</param>
    /// <param name="sum">A monoid over type T used for the 'additon' (aggregation) step.</param>
    /// <param name="product">A monoid over type T used for 'multiplying' (merging) 
    /// the two containers.</param>
    /// <returns>An object of type T that represents the result of the inner product.</returns>
    public static T InnerProduct<T>(this IEnumerable<T> left, IEnumerable<T> right
      , Monoid<T> sum, Monoid<T> product)
    {
      return left.Zip(right, (x, y) => product.Op(x, y)).MonoidOp(sum);
    }
    
    public static T InnerProduct<T>(this IEnumerable<T> left, IEnumerable<T> right
      , SemiRing<T> ring)
    {
      if (left == null) throw new ArgumentNullException("left");
      if (right == null) throw new ArgumentNullException("right");
      if (ring == null) throw new ArgumentNullException("ring");

      return left.Zip(right, (x, y) => ring.Product.Op(x, y)).MonoidOp(ring.Add);
    }

    /// <summary>
    /// Permute the elements of the source list using the indexes in the permuteindexes list.
    /// </summary>
    /// <typeparam name="T">The type of the contained object</typeparam>
    /// <param name="source">Input list</param>
    /// <param name="permuteIndexes">An ordered list of indexes of the source list to permute</param>
    /// <returns>the permuted list of values from the source list</returns>
    public static IEnumerable<T> Permute<T>(this IEnumerable<T> source, IEnumerable<int> permuteIndexes)
    {
      if (source == null) throw new ArgumentNullException("source");
      if (permuteIndexes == null) throw new ArgumentNullException("permuteIndexes");
      if (permuteIndexes.Count() < 2) throw new ArgumentOutOfRangeException("permuteIndexes", @"the Count of 'permuteIndexes' must be greater than 2");

      var work = source.ToArray();
      var size = work.Count();
      int cell = permuteIndexes.First();

      foreach (var index in permuteIndexes)
      {
        T temp = work[cell];
        work[cell] = work[index];
        work[index] = temp;
        cell = index;
      }
      return work;
    }
  }

}
