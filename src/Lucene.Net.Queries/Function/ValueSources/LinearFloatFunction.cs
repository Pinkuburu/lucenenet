﻿/*
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System.Collections;
using Lucene.Net.Queries.Function.DocValues;
using org.apache.lucene.queries.function;

namespace Lucene.Net.Queries.Function.ValueSources
{
    /// <summary>
	/// <code>LinearFloatFunction</code> implements a linear function over
	/// another <seealso cref="ValueSource"/>.
	/// <br>
	/// Normally Used as an argument to a <seealso cref="FunctionQuery"/>
	/// 
	/// 
	/// </summary>
	public class LinearFloatFunction : ValueSource
	{
	  protected internal readonly ValueSource source;
	  protected internal readonly float slope;
	  protected internal readonly float intercept;

	  public LinearFloatFunction(ValueSource source, float slope, float intercept)
	  {
		this.source = source;
		this.slope = slope;
		this.intercept = intercept;
	  }

	  public override string description()
	  {
		return slope + "*float(" + source.description() + ")+" + intercept;
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: @Override public org.apache.lucene.queries.function.FunctionValues GetValues(java.util.Map context, org.apache.lucene.index.AtomicReaderContext readerContext) throws java.io.IOException
	  public override FunctionValues GetValues(IDictionary context, AtomicReaderContext readerContext)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final org.apache.lucene.queries.function.FunctionValues vals = source.GetValues(context, readerContext);
		FunctionValues vals = source.GetValues(context, readerContext);
		return new FloatDocValuesAnonymousInnerClassHelper(this, this, vals);
	  }

	  private class FloatDocValuesAnonymousInnerClassHelper : FloatDocValues
	  {
		  private readonly LinearFloatFunction outerInstance;

		  private FunctionValues vals;

		  public FloatDocValuesAnonymousInnerClassHelper(LinearFloatFunction outerInstance, LinearFloatFunction this, FunctionValues vals) : base(this)
		  {
			  this.outerInstance = outerInstance;
			  this.vals = vals;
		  }

		  public override float FloatVal(int doc)
		  {
			return vals.FloatVal(doc) * outerInstance.slope + outerInstance.intercept;
		  }
		  public override string ToString(int doc)
		  {
			return outerInstance.slope + "*float(" + vals.ToString(doc) + ")+" + outerInstance.intercept;
		  }
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: @Override public void CreateWeight(java.util.Map context, org.apache.lucene.search.IndexSearcher searcher) throws java.io.IOException
	  public override void CreateWeight(IDictionary context, IndexSearcher searcher)
	  {
		source.CreateWeight(context, searcher);
	  }

	  public override int GetHashCode()
	  {
		int h = Number.FloatToIntBits(slope);
		h = ((int)((uint)h >> 2)) | (h << 30);
		h += Number.FloatToIntBits(intercept);
		h ^= (h << 14) | ((int)((uint)h >> 19));
		return h + source.GetHashCode();
	  }

	  public override bool Equals(object o)
	  {
		if (typeof(LinearFloatFunction) != o.GetType())
		{
			return false;
		}
		LinearFloatFunction other = (LinearFloatFunction)o;
		return this.slope == other.slope && this.intercept == other.intercept && this.source.Equals(other.source);
	  }
	}

}