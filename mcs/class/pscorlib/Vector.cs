// Copyright 2013 Zynga Inc.
//	
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//		
//      Unless required by applicable law or agreed to in writing, software
//      distributed under the License is distributed on an "AS IS" BASIS,
//      WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//      See the License for the specific language governing permissions and
//      limitations under the License.


using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Runtime.CompilerServices;


namespace _root {

	// this class is used to display a custom view of the vector values to the debugger
	// TODO: we need to make these elements editable 
	internal class VectorDebugView<T>
	{
		private Vector<T>  mVector;
		
		// The constructor for the type proxy class must have a 
		// constructor that takes the target type as a parameter.
		public VectorDebugView(Vector<T> vector)
		{
			this.mVector = vector;
		}
		
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public T[] Values
		{
			get
			{
				mVector._TrimCapacity();
				return mVector._GetInnerArray();
			}
		}
	}
	
	[DebuggerDisplay("length = {length}")]
	[DebuggerTypeProxy(typeof(VectorDebugView<>))]
	public sealed class Vector<T> : Object, IList<T>, IList, PlayScript.IDynamicClass, PlayScript.IKeyEnumerable
	{
		#region IList implementation

		int IList.Add(object value)
		{
			throw new NotImplementedException();
		}

		void IList.Clear()
		{
			this.length = 0;
		}

		bool IList.Contains(object value)
		{
			return this.indexOf((T)value) >= 0;
		}

		int IList.IndexOf(object value)
		{
			return this.indexOf((T)value);
		}

		void IList.Insert(int index, object value)
		{
			throw new NotImplementedException();
		}

		void IList.Remove(object value)
		{
			throw new NotImplementedException();
		}

		void IList.RemoveAt(int index)
		{
			throw new NotImplementedException();
		}

		bool IList.IsFixedSize {
			get {
				return @fixed;
			}
		}

		bool IList.IsReadOnly {
			get {
				return false;
			}
		}

		object IList.this[int index] {
			get {
				return (object)this[index];
			}
			set {
				this[index] = (T)value;
			}
		}

		#endregion

		#region ICollection implementation

		void ICollection.CopyTo(System.Array array, int index)
		{
			System.Array.Copy(mArray, 0, array, index, mCount);
		}

		bool ICollection.IsSynchronized {
			get {
				return false;
			}
		}

		int ICollection.Count {
			get {
				return (int)this.length;
			}
		}

		object ICollection.SyncRoot {
			get {
				return null;
			}
		}

		#endregion

		private const string ERROR_RESIZING_FIXED = "Error resizing fixed vector.";

		private T[] mArray;
		private uint mCount;
		private bool mFixed = false;
		private PlayScript.IDynamicClass __dynamicProps = null;		// By default it is not created as it is not commonly used (nor a good practice).
																	// We create it only if there is a dynamic set.

		private static T[] sEmptyArray = new T[0];
		
		//
		// Properties
		//
		
		public bool @fixed 
		{ 
			get { return mFixed; } 
			set { mFixed = value;} 
		}
		
		public uint length
		{
#if NET_4_5
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get { return mCount; } 
			set { 
				if (value == 0) {
					System.Array.Clear (mArray, 0, (int)mCount);
				} else if (mCount < value) {
					EnsureCapacity(value);
				} else if (mCount > value) {
					System.Array.Clear (mArray, (int)value, (int)(mCount - value));
				}
				mCount = value;
			} 
		}
		
		//
		// Methods
		//
		
		public Vector(Vector<T> v) 
		{
			mArray = v.mArray.Clone() as T[];
			mCount = v.mCount;
			mFixed = v.mFixed;
		}
		
		public Vector(Array a)
		{
			mArray = new T[a.length];
			this.append((IEnumerable)a);
		}

		public Vector(IList a)
		{
			mArray = new T[a.Count];
			this.append((IEnumerable)a);
		}

		public Vector(IEnumerable e)
		{
			mArray = sEmptyArray;
			this.append(e);
		}

		public Vector(uint length = 0, bool @fixed = false)
		{
			if (length != 0)
				mArray = new T[(int)length];
			else
				mArray = sEmptyArray;
			mCount = length;
			mFixed = @fixed;
		}

		public T this[int i]
		{
#if NET_4_5
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
#if PERFORMANCE_MODE && DEBUG
				if ((i >= mCount) || (i < 0))
				{
					throw new IndexOutOfRangeException();
				}
#elif PERFORMANCE_MODE
#else
				if ((i >= mCount) || (i < 0))
				{
					return default(T);
				}
#endif
				return mArray[i];
			}
#if NET_4_5
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
#if PERFORMANCE_MODE && DEBUG
				if (i >= mCount) {
					throw new IndexOutOfRangeException();
				}
#elif PERFORMANCE_MODE
#else
				if (i >= mCount) {
					expand((uint)(i+1));
				}
#endif
				mArray[i] = value;
			}
		}

		public T this[uint i]
		{
#if NET_4_5
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
#if PERFORMANCE_MODE && DEBUG
				if (i >= mCount)
				{
					throw new IndexOutOfRangeException();
				}
#elif PERFORMANCE_MODE
#else
				if (i >= mCount)
				{
					return default(T);
				}
#endif
				return mArray[(int)i];
			}
#if NET_4_5
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
#if PERFORMANCE_MODE && DEBUG
				if (i >= mCount) {
					throw new IndexOutOfRangeException();
				}
#elif PERFORMANCE_MODE
#else
				if (i >= mCount) {
					expand((uint)(i+1));
				}
#endif
				mArray[(int)i] = value;
			}
		}

		public T this[string name]
		{
			get {
				if (__dynamicProps != null) {
					return default(T);				// default(T) as we can't return Undefined
				}
				else {
					dynamic result = __dynamicProps.__GetDynamicValue(name);	// The instance that was set was only of T type (or undefined)
					if (result == PlayScript.Undefined._undefined)	{
						return default(T);				// default(T) as we can't return Undefined
					}
					return (T)result;
				}
			}
			set {
				if (__dynamicProps == null) {
					__dynamicProps = new PlayScript.DynamicProperties();	// Create the dynamic propertties only on the first set usage
				}
				__dynamicProps.__SetDynamicValue(name, value);					// This will only inject T type instances.
			}
		}

		public T[] ToArray()
		{
			T[] ret = new T[mCount];
			System.Array.Copy(mArray, ret, mCount);
			return ret;
		}

		public T[] _GetInnerArray()
		{
			return mArray;
		}

		public void _TrimCapacity()
		{
			if (mCount < mArray.Length) {
				mArray = ToArray();
			}
		}

		private void EnsureCapacity(uint size)
		{
			if (mArray.Length < size) {
				if (mFixed)
					throw new InvalidOperationException(ERROR_RESIZING_FIXED);
				int newSize = mArray.Length * 2;
				if (newSize == 0) newSize = 4;
				while (newSize < size)
					newSize = newSize * 2;
				T[] newArray = new T[newSize];
				System.Array.Copy(mArray, newArray, mArray.Length);
				mArray = newArray;
			}
		}
		
		public void Add(T value) 
		{
			this.push (value);
		}

		private void _Insert(int index, T value) 
		{
			if (index > mCount) throw new NotImplementedException();

			if (mCount >= mArray.Length)
				EnsureCapacity(mCount + 1);
			if (index < (int)mCount) {
			//	System.Array.Copy (mArray, index, mArray, index + 1, (int)mCount - index);
				for (int i=(int)mCount; i > index; i--)
				{
					mArray[i] = mArray[i-1];
				}
			}
			mArray[index] = value;
			mCount++;
		}

		private void _RemoveAt(int index)
		{
			if (index < 0 || index >= (int)mCount)
				throw new IndexOutOfRangeException();

			if (index == (int)mCount - 1) {
				mArray[index] = default(T);
			} else {
				System.Array.Copy (mArray, index + 1, mArray, index, (int)mCount - index - 1);
				mArray[mCount - 1] = default(T);
			}
			mCount--;
		}

		// optionally expands the vector to accomodate the new size
		// if the vector is big enough then nothing is done
		public void expand(uint newSize) 
		{
			EnsureCapacity(newSize);
			if (mCount < newSize)
				mCount = newSize;
		}

		public void append(Vector<T> vec)
		{
			EnsureCapacity(mCount + vec.mCount);
			System.Array.Copy (vec.mArray, 0, mArray, mCount, vec.mCount);
			mCount += vec.mCount;
		}

		public void append(IEnumerable items)
		{
			if (items == null) {
				return;
			}
			if (items is IList) {
				var list = (items as IList);
				EnsureCapacity(mCount + (uint)list.Count);
			}
			
			foreach (var item in items) {
				this.Add ((T)item);
			}
		}


		public void append(IEnumerable<T> items)
		{
			if (items is IList<T>) {
				var list = (items as IList<T>);
				EnsureCapacity(mCount + (uint)list.Count);
			}
		
			foreach (var item in items) {
				this.Add (item);
			}
		}

		public void copyTo(Vector<T> dest, int sourceIndex, int destIndex, int count) {
			System.Array.Copy(this.mArray, sourceIndex, dest.mArray, destIndex, count);
		}

		public Vector<T> clone() {
			return new Vector<T>(this);
		}

		public Vector<T> concat(params object[] args) {
			Vector<T> v = clone();
			// concat all supplied vecots
			foreach (var o in args)
			{
				if (o is IEnumerable<T>)
				{
					v.append(o as IEnumerable<T>);
				} 
				else
				{
					throw new System.NotImplementedException();
				}
			}
			return v;
		}

		public bool every(Delegate callback, dynamic thisObject = null) 
		{
			throw new System.NotImplementedException();
		}
		
		public Vector<T> filter(Delegate callback, dynamic thisObject = null) 
		{
			throw new System.NotImplementedException();
		}
		
		public void forEach(Delegate callback, dynamic thisObject = null) 
		{
			if (thisObject != null)
			{
				throw new NotImplementedException();
			}
			foreach (var item in this)
			{
				callback.DynamicInvoke(item);
			}
		}

		public int indexOf(T searchElement)
		{
			return System.Array.IndexOf<T>(mArray, searchElement, 0, (int)mCount);
		}
		
		public int indexOf(T searchElement, int fromIndex) 
		{
			return System.Array.IndexOf<T>(mArray, searchElement, fromIndex, (int)mCount);
		}
		
		public string join(string sep = ",") 
		{
			var sb = new System.Text.StringBuilder();
			bool needsSeperator = false;
			foreach (var item in this)
			{
				if (needsSeperator) {
					sb.Append(sep);
				}
				if (item != null)
				{
					sb.Append(item.ToString());
				}
				needsSeperator = true;
			}
			return sb.ToString();
		}
		
		public int lastIndexOf(T searchElement, int fromIndex = 0x7fffffff) 
		{
			throw new System.NotImplementedException();
		}
		
		public Vector<T> map(Delegate callback, dynamic thisObject = null) 
		{
			throw new System.NotImplementedException();
		}
		
		public T pop() 
		{
			if (mFixed)
				throw new InvalidOperationException(ERROR_RESIZING_FIXED);
			if (mCount == 0) {
				return default(T);
			}
			T val = mArray[mCount - 1];
			mCount--;
			mArray[mCount] = default(T);
			return val;
		}

#if NET_4_5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if PERFORMANCE_MODE
		public void push(T value)
#else
		public uint push(T value)
#endif
		{
#if !PERFORMANCE_MODE
			if (mFixed)
				throw new InvalidOperationException(ERROR_RESIZING_FIXED);
#endif
			if (mCount >= mArray.Length)
				EnsureCapacity((uint)(1.25 * (mCount + 1)));
			mArray[mCount] = value;
			mCount++;
#if !PERFORMANCE_MODE
			return mCount;
#endif
		}
		
		public uint push(T value, params T[] args) 
		{
			if (mFixed)
				throw new InvalidOperationException(ERROR_RESIZING_FIXED);
			uint len = (uint)args.Length;
			if (mArray.Length < mCount + 1 + len)
				EnsureCapacity((uint)(1.25 * (mCount + len)));
			mArray[mCount++] = value;
			for (var i = 0; i < len; i++)
				mArray[mCount++] = args[i];
			return mCount;
		}
		
		public Vector<T> reverse() 
		{
			var nv = new Vector<T>(length, @fixed);
			int l = (int)length;
			for (int i = 0; i < l; i++)
			{
				nv[i] = this[l - i - 1];
			}
			return nv;
		}
		
		public T shift() 
		{
			if (mFixed)
				throw new InvalidOperationException(ERROR_RESIZING_FIXED);

			if (mCount == 0)
			{
				return default(T);
			}
			T v = this[0];
			_RemoveAt(0);
			return v;
		}

#if PERFORMANCE_MODE
		public void slice(int startIndex = 0, int endIndex = 16777215) 
#else
		public Vector<T> slice(int startIndex = 0, int endIndex = 16777215) 
#endif
		{
			if (startIndex < 0) 
				throw new InvalidOperationException("splice error");

			if (endIndex < 0) endIndex = (int)mCount + endIndex;		// If negative, starts from the end

			if (endIndex > (int)mCount) endIndex = (int)mCount;

			int count = endIndex - startIndex;
			if (count < 0)
				count = 0;

#if !PERFORMANCE_MODE
			var result = new Vector<T>((uint)count, false);
			System.Array.Copy(mArray, startIndex, result.mArray, 0, count);
			return result;
#endif
		}
		
		public bool some(Delegate callback, dynamic thisObject = null) 
		{
			throw new System.NotImplementedException();
		}

		private class TypedFunctionSorter : System.Collections.Generic.IComparer<T>
		{
			public TypedFunctionSorter(System.Func<T, T,int> comparerDelegate)
			{
				mDelegate = comparerDelegate;
			}

			public int Compare(T x, T y)
			{
				return mDelegate.Invoke(x, y);
			}

			private System.Func<T, T,int> mDelegate;
		}


		private class FunctionSorter : System.Collections.Generic.IComparer<T>
		{
			public FunctionSorter(object func)
			{
				mFunc = func;
			}
			
			public int Compare(T x, T y)
			{
				return (int)mFunc(x, y);
			}
			
			private dynamic mFunc;
		};
		
		private class OptionsSorter : System.Collections.Generic.IComparer<T>
		{
			public OptionsSorter(uint options)
			{
				// mOptions = options;
			}
			
			public int Compare(T x, T y)
			{
				//$$TODO examine options
				var xc = x as System.IComparable<T>;
				if (xc != null)
				{
					return xc.CompareTo(y);
				}
				else
				{
					throw new System.NotImplementedException();
				}
			}
			
			// private uint mOptions;
		};

		private class DefaultSorter : System.Collections.Generic.IComparer<T>
		{
			public int Compare(T x, T y)
			{
				// From doc:
				//	http://help.adobe.com/en_US/FlashPlatform/reference/actionscript/3/Vector.html#sort%28%29
				// All elements, regardless of data type, are sorted as if they were strings, so 100 precedes 99, because "1" is a lower string value than "9".
				// That's going to be slow...
				return x.ToString().CompareTo(y.ToString());
			}
		}
		
		public Vector<T> sort(dynamic sortBehavior = null) 
		{
			IComparer<T> comparer;
			if (sortBehavior is System.Func<T, T,int>)
			{
				System.Func<T, T,int> func = (System.Func<T, T,int>)sortBehavior;
				// By definition, we know that the vector only contains type T,
				// so if the function passed has the exact expected signature, we use the fast path
				comparer = new TypedFunctionSorter(func);
			}
			else if (sortBehavior is Delegate)
			{
				comparer = new FunctionSorter(sortBehavior);
			}
			else if (sortBehavior is uint)
			{
				comparer = new OptionsSorter((uint)sortBehavior);
			}
			else 
			{
				//	http://help.adobe.com/en_US/FlashPlatform/reference/actionscript/3/Vector.html#sort%28%29
				comparer = new DefaultSorter();
			}
			System.Array.Sort (mArray, 0, (int)mCount, comparer);
			return this;
		}

#if true
		public Vector<T> splice(int startIndex, uint deleteCount = 4294967295) 
		{
			Vector<T> removed = null;

			if (mFixed)
				throw new InvalidOperationException(ERROR_RESIZING_FIXED);

			if (startIndex < 0) 
				throw new InvalidOperationException("splice error");

			// determine number of items to delete
			int toDelete = (int)mCount - startIndex;
			if ((uint)toDelete > deleteCount) 
				toDelete = (int)deleteCount;

			if (toDelete == 1) {
				removed = new Vector<T>(1);
				removed.mArray[0] = mArray[startIndex];
				int toMove = (int)mCount - 1 - startIndex;
				if (toMove > 0)
					System.Array.Copy (mArray, startIndex + toDelete, mArray, startIndex, toMove);
				mArray[mCount - 1] = default(T);
				mCount--;
			} else if (toDelete > 1) {
				removed = new Vector<T>((uint)toDelete);
				System.Array.Copy (mArray, startIndex, removed.mArray, 0, toDelete);
				int toMove = (int)mCount - toDelete - startIndex;
				if (toMove > 0)
					System.Array.Copy (mArray, startIndex + toDelete, mArray, startIndex, toMove);
				System.Array.Clear (mArray, startIndex + toMove, toDelete);
				mCount = (uint)(startIndex + toMove);
			}
			
			return removed;
		}
		
		public Vector<T> splice(int startIndex, uint deleteCount = 4294967295, params T[] items) 
		{
			Vector<T> removed = null;

			if (mFixed)
				throw new InvalidOperationException(ERROR_RESIZING_FIXED);

			if (startIndex < 0) 
				throw new InvalidOperationException("splice error");

			// determine number of items to delete
			int toDelete = (int)mCount - startIndex;
			if ((uint)toDelete > deleteCount) 
				toDelete = (int)deleteCount;
			
			if (toDelete == 1) {
				removed = new Vector<T>(1);
				removed.mArray[0] = mArray[startIndex];
				int toMove = (int)mCount - 1 - startIndex;
				if (toMove > 0)
					System.Array.Copy (mArray, startIndex + toDelete, mArray, startIndex, toMove);
				mArray[mCount - 1] = default(T);
				mCount--;
			} else if (toDelete > 1) {
				removed = new Vector<T>((uint)toDelete);
				System.Array.Copy (mArray, startIndex, removed.mArray, 0, toDelete);
				int toMove = (int)mCount - toDelete - startIndex;
				if (toMove > 0)
					System.Array.Copy (mArray, startIndex + toDelete, mArray, startIndex, toMove);
				System.Array.Clear (mArray, startIndex + toMove, toDelete);
				mCount = (uint)(startIndex + toMove);
			}

			uint itemsLen = (uint)items.Length;
			if (itemsLen > 0) {
				EnsureCapacity(mCount + itemsLen);
				int toMove = (int)mCount - startIndex;
				if (toMove > 0)
					System.Array.Copy (mArray, startIndex, mArray, startIndex + itemsLen, toMove);
				if (itemsLen == 1)
					mArray[startIndex] = items[0];
				else
					System.Array.Copy (items, 0, mArray, startIndex, itemsLen);
				mCount += itemsLen;
			}
			
			return removed;
		}
		
		public void splice_noret(int startIndex, uint deleteCount = 4294967295) 
		{
			if (mFixed)
				throw new InvalidOperationException(ERROR_RESIZING_FIXED);

			if (startIndex < 0) 
				throw new InvalidOperationException("splice error");

			// determine number of items to delete
			int toDelete = (int)mCount - startIndex;
			if ((uint)toDelete > deleteCount) 
				toDelete = (int)deleteCount;
			
			if (toDelete == 1) {
				int toMove = (int)mCount - 1 - startIndex;
				if (toMove > 0)
					System.Array.Copy (mArray, startIndex + toDelete, mArray, startIndex, toMove);
				mArray[mCount - 1] = default(T);
				mCount--;
			} else if (toDelete > 1) {
				int toMove = (int)mCount - toDelete - startIndex;
				if (toMove > 0)
					System.Array.Copy (mArray, startIndex + toDelete, mArray, startIndex, toMove);
				System.Array.Clear (mArray, startIndex + toMove, toDelete);
				mCount = (uint)(startIndex + toMove);
			}
		}

		public void splice_noret(int startIndex, uint deleteCount = 4294967295, params T[] items) 
		{
			if (mFixed)
				throw new InvalidOperationException(ERROR_RESIZING_FIXED);

			if (startIndex < 0) 
				throw new InvalidOperationException("splice error");

			// determine number of items to delete
			int toDelete = (int)mCount - startIndex;
			if ((uint)toDelete > deleteCount) 
				toDelete = (int)deleteCount;
			
			if (toDelete == 1) {
				int toMove = (int)mCount - 1 - startIndex;
				if (toMove > 0)
					System.Array.Copy (mArray, startIndex + toDelete, mArray, startIndex, toMove);
				mArray[mCount - 1] = default(T);
				mCount--;
			} else if (toDelete > 1) {
				int toMove = (int)mCount - toDelete - startIndex;
				if (toMove > 0)
					System.Array.Copy (mArray, startIndex + toDelete, mArray, startIndex, toMove);
				System.Array.Clear (mArray, startIndex + toMove, toDelete);
				mCount = (uint)(startIndex + toMove);
			}
			
			uint itemsLen = (uint)items.Length;
			if (itemsLen > 0) {
				EnsureCapacity(mCount + itemsLen);
				int toMove = (int)mCount - startIndex;
				if (toMove > 0)
					System.Array.Copy (mArray, startIndex, mArray, startIndex + itemsLen, toMove);
				if (itemsLen == 1)
					mArray[startIndex] = items[0];
				else
					System.Array.Copy (items, 0, mArray, startIndex, itemsLen);
				mCount += itemsLen;
			}
		}
#else
		public Vector<T> splice(int startIndex, uint deleteCount = 4294967295, params T[] items) {
			Vector<T> removed = null;

			if (startIndex < 0) 
				throw new InvalidOperationException("splice error");

			
			// determine number of items to delete
			uint toDelete = (uint)(this.length - startIndex);
			if (toDelete > deleteCount) toDelete = deleteCount;
			
			if (toDelete > 0)
			{
				removed = new Vector<T>();
				
				// build list of items we removed
				for (int i=0; i < toDelete; i++)
				{
					removed.push( mArray[startIndex + i] );
				}
				
				// remove items
				for (int i=0; i < toDelete; i++)
				{
					this._RemoveAt(startIndex);
				}
			}
			
			if (items.Length > 0)
			{
				for (int i=0; i < items.Length; i++)
				{
					this._Insert((int)(startIndex + i), items[i] );
				}
			}
			
			return removed;
		}
#endif

		
		public string toLocaleString() 
		{
			throw new System.NotImplementedException();
		}

		public override string toString() 
		{
			return this.ToString();
		}
		
		public override string ToString()
		{
			return this.join(",");
		}

		public uint unshift(T item) 
		{
			if (mFixed)
				throw new InvalidOperationException(ERROR_RESIZING_FIXED);
			if (mCount >= mArray.Length)
				EnsureCapacity(mCount + 1);
			if (mCount > 0)
				System.Array.Copy (mArray, 0, mArray, 1, (int)mCount);
			mArray[0] = item;
			mCount++;
			return mCount;
		}

		public uint unshift(T item, params T[] args) 
		{
			if (mFixed)
				throw new InvalidOperationException(ERROR_RESIZING_FIXED);
			uint argsLen = (uint)args.Length;
			EnsureCapacity(mCount + 1 + argsLen);
			if (mCount > 0)
				System.Array.Copy (mArray, 0, mArray, args.Length, (int)mCount);
			mArray[0] = item;
			System.Array.Copy (args, 0, mArray, 1, argsLen);
			mCount += 1 + argsLen;
			return mCount;
		}
		
		#region IList implementation

		int IList<T>.IndexOf (T item)
		{
			return this.indexOf(item);
		}

		void IList<T>.Insert (int index, T item)
		{
			_Insert (index, item);
		}

		void IList<T>.RemoveAt (int index)
		{
			_RemoveAt(index);
		}

		#endregion

		#region ICollection implementation

//		void ICollection<T>.Add (T item)
//		{
//			this._Add (item);
//		}

		void ICollection<T>.Clear ()
		{
			this.length = 0;
		}

		bool ICollection<T>.Contains (T item)
		{
			return this.indexOf(item) != -1;
		}

		void ICollection<T>.CopyTo (T[] array, int arrayIndex)
		{
			System.Array.Copy (mArray, 0, array, arrayIndex, (int)mCount);
		}

		bool ICollection<T>.Remove (T item)
		{
			int i = this.indexOf(item);
			if (i >= 0) {
				_RemoveAt(i);
				return true;
			} else {
				return false;
			}
		}

		int ICollection<T>.Count {
			get {
				return (int)mCount;
			}
		}

		bool ICollection<T>.IsReadOnly {
			get {
				return false;
			}
		}

		#endregion

		#region IEnumerable implementation

		private class VectorEnumeratorClass : IEnumerator<T>
		{
			private readonly Vector<T> mVector;
			private int mIndex;

			public VectorEnumeratorClass(Vector<T> vector)
			{
				mVector = vector;
				mIndex = -1;
			}

			#region IEnumerator implementation

			public bool MoveNext ()
			{
				mIndex++;
				return mIndex < mVector.mCount;
			}

			public void Reset ()
			{
				mIndex = -1;
			}

			object System.Collections.IEnumerator.Current {
				get {
					return mVector.mArray[mIndex];
				}
			}

			#endregion

			#region IDisposable implementation

			public void Dispose ()
			{
			}

			#endregion

			#region IEnumerator implementation

			public T Current {
				get {
					return mVector.mArray[mIndex];
				}
			}

			#endregion

		}

		// this is the public struct enumerator, it does not implement IDisposable and doesnt allocate space on the heap
		public struct VectorEnumeratorStruct
		{
			private readonly Vector<T> mVector;
			private int mIndex;

			public VectorEnumeratorStruct(Vector<T> vector)
			{
				mVector = vector;
				mIndex = -1;
			}

			#region IEnumerator implementation
			public bool MoveNext ()
			{
				mIndex++;
				return mIndex < mVector.mCount;
			}

			public void Reset ()
			{
				mIndex = -1;
			}

			public T Current {
				get {
					return mVector.mArray[mIndex];
				}
			}
			#endregion
		}

		// public get enumerator that returns a faster struct
		public VectorEnumeratorStruct GetEnumerator ()
		{
			return new VectorEnumeratorStruct(this);
		}

		// private IEnumerable<T> get enumerator that returns a (slower) class on the heap
		IEnumerator<T> IEnumerable<T>.GetEnumerator ()
		{
			return new VectorEnumeratorClass(this);
		}

		#endregion

		#region IEnumerable implementation

		// private IEnumerable get enumerator that returns a (slower) class on the heap
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			return new VectorEnumeratorClass(this);
		}

		#endregion


		#region IKeyEnumerable implementation

		private class VectorKeyEnumeratorClass : IEnumerator
		{
			private readonly IList mVector;
			private int mIndex;
			
			public VectorKeyEnumeratorClass(IList vector)
			{
				mVector = vector;
				mIndex = -1;
			}
			
			#region IEnumerator implementation
			
			public bool MoveNext ()
			{
				mIndex++;
				return mIndex < mVector.Count;
			}
			
			public void Reset ()
			{
				mIndex = -1;
			}
			
			object System.Collections.IEnumerator.Current {
				get {
					return mIndex;
				}
			}
			
			#endregion
			
			#region IDisposable implementation
			
			public void Dispose ()
			{
			}
			
			#endregion
		}

		// this is the public struct enumerator, it does not implement IDisposable and doesnt allocate space on the heap
		public struct VectorKeyEnumeratorStruct 
		{
			private readonly IList mVector;
			private int mIndex;

			public VectorKeyEnumeratorStruct(IList vector)
			{
				mVector = vector;
				mIndex = -1;
			}

			#region IEnumerator implementation

			public bool MoveNext ()
			{
				mIndex++;
				return mIndex < mVector.Count;
			}

			public void Reset ()
			{
				mIndex = -1;
			}

			// unfortunately this has to return object because the for() loop could use a non-int as its variable, causing bad IL
			public object Current {
				get {
					return mIndex;
				}
			}
			#endregion
		}

		// public get enumerator that returns a faster struct
		public VectorKeyEnumeratorStruct GetKeyEnumerator()
		{
			return new VectorKeyEnumeratorStruct(this);
		}

		// private IKeyEnumerable get enumerator that returns a (slower) class on the heap
		IEnumerator PlayScript.IKeyEnumerable.GetKeyEnumerator()
		{
			return new VectorKeyEnumeratorClass(this);
		}

		#endregion

		#region IDynamicClass implementation

		// this method can be used to override the dynamic property implementation of this dynamic class
		void __SetDynamicProperties(PlayScript.IDynamicClass props) {
			__dynamicProps = props;
		}

		dynamic PlayScript.IDynamicClass.__GetDynamicValue (string name) {
			object value = null;
			if (__dynamicProps != null) {
				value = __dynamicProps.__GetDynamicValue(name);
			}
			return value;
		}

		bool PlayScript.IDynamicClass.__TryGetDynamicValue (string name, out object value) {
			if (__dynamicProps != null) {
				return __dynamicProps.__TryGetDynamicValue(name, out value);
			} else {
				value = null;
				return false;
			}
		}

		void PlayScript.IDynamicClass.__SetDynamicValue (string name, object value) {
			if (__dynamicProps == null) {
				__dynamicProps = new PlayScript.DynamicProperties(this);
			}
			__dynamicProps.__SetDynamicValue(name, value);
		}

		bool PlayScript.IDynamicClass.__DeleteDynamicValue (object name) {
			if (__dynamicProps != null) {
				return __dynamicProps.__DeleteDynamicValue(name);
			}
			return false;
		}

		bool PlayScript.IDynamicClass.__HasDynamicValue (string name) {
			if (__dynamicProps != null) {
				return __dynamicProps.__HasDynamicValue(name);
			}
			return false;
		}

		IEnumerable PlayScript.IDynamicClass.__GetDynamicNames () {
			if (__dynamicProps != null) {
				return __dynamicProps.__GetDynamicNames();
			}
			return null;
		}

		#endregion

		public static implicit operator Vector<T>(Array a) {
			PlayScript.Stats.Increment(PlayScript.StatsCounter.Runtime_CastArrayToVector);
			return new Vector<T>(a);
		}
		
		public static implicit operator Array(Vector<T> v) {
			PlayScript.Stats.Increment(PlayScript.StatsCounter.Runtime_CastVectorToArray);
			return new Array(v);
		}

	}
}

