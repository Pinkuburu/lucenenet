﻿using Icu.Collation;
using Lucene.Net.Collation.TokenAttributes;
using Lucene.Net.Support;
using Lucene.Net.Util;
using System.Reflection;

namespace Lucene.Net.Collation
{
    /// <summary>
    /// Converts each token into its <see cref="System.Globalization.SortKey"/>, and
    /// then encodes bytes as an index term.
    /// </summary>
    /// <remarks>
    /// <strong>WARNING:</strong> Make sure you use exactly the same <see cref="Collator"/> at
    /// index and query time -- <see cref="System.Globalization.SortKey"/>s are only comparable when produced by
    /// the same <see cref="Collator"/>.  <see cref="RuleBasedCollator"/>s are 
    /// independently versioned, so it is safe to search against stored
    /// <see cref="System.Globalization.SortKey"/>s if the following are exactly the same (best practice is
    /// to store this information with the index and check that they remain the
    /// same at query time):
    /// <para/>
    /// <list type="number">
    ///     <item><description>Collator version - see <see cref="Collator"/> Version</description></item>
    ///     <item><description>The collation strength used - see <see cref="Collator.Strength"/></description></item>
    /// </list>
    /// <para/>
    /// <see cref="System.Globalization.SortKey"/>s generated by ICU Collators are not compatible with those
    /// generated by java.text.Collators.  Specifically, if you use 
    /// <see cref="ICUCollationAttributeFactory"/> to generate index terms, do not use 
    /// CollationAttributeFactory on the query side, or vice versa.
    /// <para/>
    /// <see cref="ICUCollationAttributeFactory"/> is significantly faster and generates significantly
    /// shorter keys than CollationAttributeFactory.  See
    /// <a href="http://site.icu-project.org/charts/collation-icu4j-sun"
    /// >http://site.icu-project.org/charts/collation-icu4j-sun</a> for key
    /// generation timing and key length comparisons between ICU4J and
    /// java.text.Collator over several languages.
    /// </remarks>
    [ExceptionToClassNameConvention]
    public class ICUCollationAttributeFactory : AttributeSource.AttributeFactory
    {
        private readonly Collator collator;
        private readonly AttributeSource.AttributeFactory @delegate;

        /// <summary>
        /// Create an <see cref="ICUCollationAttributeFactory"/>, using 
        /// <see cref="AttributeSource.AttributeFactory.DEFAULT_ATTRIBUTE_FACTORY"/> as the
        /// factory for all other attributes.
        /// </summary>
        /// <param name="collator"><see cref="System.Globalization.SortKey"/> generator</param>
        public ICUCollationAttributeFactory(Collator collator)
            : this(AttributeSource.AttributeFactory.DEFAULT_ATTRIBUTE_FACTORY, collator)
        {
        }

        /// <summary>
        /// Create an <see cref="ICUCollationAttributeFactory"/>, using the supplied Attribute 
        /// Factory as the factory for all other attributes.
        /// </summary>
        /// <param name="delegate">Attribute Factory</param>
        /// <param name="collator"><see cref="System.Globalization.SortKey"/> generator</param>
        public ICUCollationAttributeFactory(AttributeSource.AttributeFactory @delegate, Collator collator)
        {
            this.@delegate = @delegate;
            this.collator = collator;
        }

        public override Util.Attribute CreateAttributeInstance<T>()
        {
            return typeof(T).GetTypeInfo().IsAssignableFrom(typeof(ICUCollatedTermAttribute))
                ? new ICUCollatedTermAttribute(collator)
                : @delegate.CreateAttributeInstance<T>();
        }
    }
}