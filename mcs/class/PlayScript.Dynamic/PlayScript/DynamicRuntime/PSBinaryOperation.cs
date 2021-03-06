//
// PSBinaryOperation.cs
//
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


#if !DYNAMIC_SUPPORT

using System;
using System.Collections.Generic;

namespace PlayScript.DynamicRuntime
{
	public static class PSBinaryOperation
	{
		private static string ADD = "add";
		private static string SUB = "sub";
		private static string MUL = "mul";
		private static string DIV = "div";
		private static string MOD = "mod";
		private static string SHL = "shl";
		private static string SHR = "shr";
		private static string LT = "lt";
		private static string LTE = "lte";
		private static string GT = "gt";
		private static string GTE = "gte";
		private static string EQ = "eq";
		private static string NEQ = "ne";
//		private static string AND = "and";
//		private static string OR = "or";
		private static string XOR = "xor";

		/*
	 * 		public enum Operator {
			Multiply	= 0 | ArithmeticMask,
			Division	= 1 | ArithmeticMask,
			Modulus		= 2 | ArithmeticMask,
			Addition	= 3 | ArithmeticMask | AdditionMask,
			Subtraction = 4 | ArithmeticMask | SubtractionMask,

			LeftShift	= 5 | ShiftMask,
			RightShift	= 6 | ShiftMask,
			AsURightShift = 7 | ShiftMask,  // PlayScript Unsigned Right Shift

			LessThan	= 8 | ComparisonMask | RelationalMask,
			GreaterThan	= 9 | ComparisonMask | RelationalMask,
			LessThanOrEqual		= 10 | ComparisonMask | RelationalMask,
			GreaterThanOrEqual	= 11 | ComparisonMask | RelationalMask,
			Equality	= 12 | ComparisonMask | EqualityMask,
			Inequality	= 13 | ComparisonMask | EqualityMask,
			AsRefEquality = 14 | ComparisonMask | EqualityMask,
			AsRefInequality = 15 | ComparisonMask | EqualityMask,

			BitwiseAnd	= 16 | BitwiseMask,
			ExclusiveOr	= 17 | BitwiseMask,
			BitwiseOr	= 18 | BitwiseMask,

			LogicalAnd	= 19 | LogicalMask,
			LogicalOr	= 20 | LogicalMask,

			AsE4xChild				= 21 | AsE4xMask,
			AsE4xDescendant			= 22 | AsE4xMask,
			AsE4xChildAttribute		= 23 | AsE4xMask,
			AsE4xDescendantAttribute = 24 | AsE4xMask,

			//
			// Operator masks
			//
			ValuesOnlyMask	= ArithmeticMask - 1,
			ArithmeticMask	= 1 << 6,
			ShiftMask		= 1 << 7,
			ComparisonMask	= 1 << 8,
			EqualityMask	= 1 << 9,
			BitwiseMask		= 1 << 10,
			LogicalMask		= 1 << 11,
			AdditionMask	= 1 << 12,
			SubtractionMask	= 1 << 13,
			RelationalMask	= 1 << 14,
			AsE4xMask		= 1 << 15
		}

*/

		private static void ThrowOnInvalidOp (object o, string op)
		{
			throw new Exception ("Invalid " + op + " operation with type " + o.GetType ().Name);
		}

		// Arithmetic operations are using the following rules:
		//	A. If the object is of type of the other operand, cast the operand to the common type, and do the operation directly.
		//	B. If the object is of different type, we whole operation is done in double precision.
		// This keeps fast performance if the user code keeps all types the same, and allows for other cases to keep maximum precision.
		// There is a bit of performance loss in some cases (like when mixing int with uint), but it should not be common with AS.

		// We also have to test for null and undefined (before calling Convert.ToXYZ().
		// Here are the checks these operators have to do:
		//	1. If of expected, type direct cast and apply the operation.
		//	2. If null and undefined, assume the value is 0 and apply the operation.
		//	3. Otherwise convert to Double both operands and apply the operation.

		public static object AdditionObjInt (object a, int b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (a is int)
			{
				return (int)a + b;
			}
			if ((a == null) || (a == PlayScript.Undefined._undefined))
			{
				return b;
			}
			return Convert.ToDouble(a) + (double)b;
		}

		public static object AdditionIntObj (int a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (b is int)
			{
				return a + (int)b;
			}
			if ((b == null) || (b == PlayScript.Undefined._undefined))
			{
				return a;
			}
			return (double)a + Convert.ToDouble(b);
		}

		public static object AdditionObjUInt (object a, uint b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (a is uint)
			{
				return (uint)a + b;
			}
			if ((a == null) || (a == PlayScript.Undefined._undefined))
			{
				return b;
			}
			return Convert.ToDouble(a) + (double)b;
		}

		public static object AdditionUIntObj (uint a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (b is uint)
			{
				return a + (uint)b;
			}
			if ((b == null) || (b == PlayScript.Undefined._undefined))
			{
				return a;
			}
			return (double)a + Convert.ToDouble(b);
		}

		public static object AdditionObjDouble (object a, double b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (a is double)
			{
				return (double)a + b;
			}
			if ((a == null) || (a == PlayScript.Undefined._undefined))
			{
				return b;
			}
			return Convert.ToDouble(a) + b;
		}

		public static object AdditionDoubleObj (double a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (b is double)
			{
				return a + (double)b;
			}
			if ((b == null) || (b == PlayScript.Undefined._undefined))
			{
				return a;
			}
			return a + Convert.ToDouble(b);
		}

		public static object AdditionStringObj (string a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if ((b == null) || (b == PlayScript.Undefined._undefined))
			{
				return a;
			}
			return a + b.ToString();
		}

		public static object AdditionObjString (object a, string b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if ((a == null) || (a == PlayScript.Undefined._undefined))
			{
				return b;
			}
			return a.ToString() + b;
		}

		public static object AdditionObjObj (object a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (a is int) {
				return AdditionIntObj ((int)a, b);
			} else if (a is double) {
				return AdditionDoubleObj ((double)a, b);
			} else if (a is float) {
				return AdditionDoubleObj ((float)a, b);
			} else if (a is String) {
				return AdditionStringObj ((string)a, b);
			} else if (a is uint) {
				return AdditionUIntObj ((uint)a, b);
			} else {
				ThrowOnInvalidOp (a, ADD);
				return null;
			}
		}

		public static object SubtractionObjInt (object a, int b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (a is int)
			{
				return (int)a - b;
			}
			if ((a == null) || (a == PlayScript.Undefined._undefined))
			{
				return -b;
			}
			return Convert.ToDouble(a) - (double)b;
		}

		public static object SubtractionIntObj (int a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (b is int)
			{
				return a - (int)b;
			}
			if ((b == null) || (b == PlayScript.Undefined._undefined))
			{
				return a;
			}
			return (double)a - Convert.ToDouble(b);
		}

		public static object SubtractionObjUInt (object a, uint b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (a is uint)
			{
				return (uint)a - b;
			}
			if ((a == null) || (a == PlayScript.Undefined._undefined))
			{
				return -b;
			}
			return Convert.ToDouble(a) - (double)b;
		}

		public static object SubtractionUIntObj (uint a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (b is uint)
			{
				return a - (uint)b;
			}
			if ((b == null) || (b == PlayScript.Undefined._undefined))
			{
				return a;
			}
			return (double)a - Convert.ToDouble(b);
		}

		public static object SubtractionObjDouble (object a, double b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (a is double) 
			{
				return (double)a - b;
			}
			if ((a == null) || (a == PlayScript.Undefined._undefined))
			{
				return -b;
			}
			return Convert.ToDouble(a) - b;
		}

		public static object SubtractionDoubleObj (double a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (b is double) 
			{
				return a - (double)b;
			}
			if ((b == null) || (b == PlayScript.Undefined._undefined))
			{
				return a;
			}
			return a - Convert.ToDouble(b);
		}

		public static object SubtractionObjObj (object a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (a is int) {
				return SubtractionIntObj ((int)a, b);
			} else if (a is double) {
				return SubtractionDoubleObj ((double)a, b);
			} else if (a is float) {
				return SubtractionDoubleObj ((float)a, b);
			} else if (a is uint) {
				return SubtractionUIntObj ((uint)a, b);
			} else {
				ThrowOnInvalidOp (a, SUB);
				return null;
			}
		}

		public static object MultiplyObjInt (object a, int b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (a is int)
			{
				return (int)a * b;
			}
			if ((a == null) || (a == PlayScript.Undefined._undefined))
			{
				return (int)0;
			}
			return Convert.ToDouble(a) * (double)b;
		}

		public static object MultiplyIntObj (int a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (b is int)
			{
				return a * (int)b;
			}
			if ((b == null) || (b == PlayScript.Undefined._undefined))
			{
				return (int)0;
			}
			return (double)a * Convert.ToDouble(b);
		}

		public static object MultiplyObjUInt (object a, uint b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (a is uint)
			{
				return (uint)a * b;
			}
			if ((a == null) || (a == PlayScript.Undefined._undefined))
			{
				return (uint)0;
			}
			return Convert.ToDouble(a) * (double)b;
		}

		public static object MultiplyUIntObj (uint a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (b is uint)
			{
				return a * (uint)b;
			}
			if ((b == null) || (b == PlayScript.Undefined._undefined))
			{
				return (uint)0;
			}
			return (double)a * Convert.ToDouble(b);
		}

		public static object MultiplyObjDouble (object a, double b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (a is double) 
			{
				return (double)a * b;
			}
			if ((a == null) || (a == PlayScript.Undefined._undefined))
			{
				return (double)0;
			}
			return Convert.ToDouble(a) * b;
		}

		public static object MultiplyDoubleObj (double a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (b is double) 
			{
				return a * (double)b;
			}
			if ((b == null) || (b == PlayScript.Undefined._undefined))
			{
				return (double)0;
			}
			return a * Convert.ToDouble(b);
		}

		public static object MultiplyObjObj (object a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (a is int) {
				return MultiplyIntObj ((int)a, b);
			} else if (a is double) {
				return MultiplyDoubleObj ((double)a, b);
			} else if (a is float) {
				return MultiplyDoubleObj ((float)a, b);
			} else if (a is uint) {
				return MultiplyUIntObj ((uint)a, b);
			} else {
				ThrowOnInvalidOp (a, MUL);
				return null;
			}
		}

		public static object DivisionObjInt (object a, int b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (a is int)
			{
				return (int)a / b;
			}
			if ((a == null) || (a == PlayScript.Undefined._undefined))
			{
				return (int)0;
			}
			return Convert.ToDouble(a) / (double)b;
		}

		public static object DivisionIntObj (int a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (b is int)
			{
				return a / (int)b;
			}
			if ((b == null) || (b == PlayScript.Undefined._undefined))
			{
				return Double.NaN;		// Should probably also use Positive and Negative Infinity
			}
			return (double)a / Convert.ToDouble(b);
		}

		public static object DivisionObjUInt (object a, uint b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (a is uint)
			{
				return (uint)a / b;
			}
			if ((a == null) || (a == PlayScript.Undefined._undefined))
			{
				return (uint)0;
			}
			return Convert.ToDouble(a) / (double)b;
		}

		public static object DivisionUIntObj (uint a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (b is uint)
			{
				return a / (uint)b;
			}
			if ((b == null) || (b == PlayScript.Undefined._undefined))
			{
				return Double.NaN;		// Should probably also use Positive and Negative Infinity
			}
			return (double)a / Convert.ToDouble(b);
		}

		public static object DivisionObjDouble (object a, double b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (a is double) 
			{
				return (double)a / b;
			}
			if ((a == null) || (a == PlayScript.Undefined._undefined))
			{
				return (double)0;
			}
			return Convert.ToDouble(a) / b;
		}

		public static object DivisionDoubleObj (double a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (b is double) 
			{
				return a / (double)b;
			}
			if ((b == null) || (b == PlayScript.Undefined._undefined))
			{
				return Double.NaN;		// Should probably also use Positive and Negative Infinity
			}
			return a / Convert.ToDouble(b);
		}

		public static object DivisionObjObj (object a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (a is int) {
				return DivisionIntObj ((int)a, b);
			} else if (a is double) {
				return DivisionDoubleObj ((double)a, b);
			} else if (a is float) {
				return DivisionDoubleObj ((float)a, b);
			} else if (a is uint) {
				return DivisionUIntObj ((uint)a, b);
			} else {
				ThrowOnInvalidOp (a, DIV);
				return null;
			}
		}

		public static object ModulusObjInt (object a, int b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (a is int)
			{
				return (int)a % b;
			}
			if ((a == null) || (a == PlayScript.Undefined._undefined))
			{
				return (int)0;
			}
			return Convert.ToDouble(a) % (double)b;
		}

		public static object ModulusIntObj (int a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (b is int)
			{
				return a / (int)b;
			}
			if ((b == null) || (b == PlayScript.Undefined._undefined))
			{
				return Double.NaN;		// Should probably also use Positive and Negative Infinity
			}
			return (double)a % Convert.ToDouble(b);
		}

		public static object ModulusObjUInt (object a, uint b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (a is uint)
			{
				return (uint)a % b;
			}
			if ((a == null) || (a == PlayScript.Undefined._undefined))
			{
				return (uint)0;
			}
			return Convert.ToDouble(b) % (double)b;
		}

		public static object ModulusUIntObj (uint a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (b is uint)
			{
				return a / (uint)b;
			}
			if ((b == null) || (b == PlayScript.Undefined._undefined))
			{
				return Double.NaN;		// Should probably also use Positive and Negative Infinity
			}
			return (double)a % Convert.ToDouble(b);
		}

		public static object ModulusObjDouble (object a, double b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if ((a == null) || (a == PlayScript.Undefined._undefined))
			{
				return (uint)0;
			}
			return Convert.ToDouble(a) % b;
		}

		public static object ModulusDoubleObj (double a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if ((b == null) || (b == PlayScript.Undefined._undefined))
			{
				return Double.NaN;		// Should probably also use Positive and Negative Infinity
			}
			return a % Convert.ToDouble(b);
		}

		public static object ModulusObjObj (object a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (a is int) {
				return ModulusIntObj ((int)a, b);
			} else if (a is double) {
				return ModulusDoubleObj ((double)a, b);
			} else if (a is float) {
				return ModulusDoubleObj ((float)a, b);
			} else if (a is uint) {
				return ModulusUIntObj ((uint)a, b);
			} else {
				ThrowOnInvalidOp (a, MOD);
				return null;
			}
		}

		// Shift operations are like logical operations, integer only

		public static object LeftShiftObjInt (object a, int b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if ((a == null) || (a == PlayScript.Undefined._undefined))
			{
				return (uint)0;
			}
			return Convert.ToInt32(a) << b;
		}

		public static object LeftShiftIntObj (int a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if ((b == null) || (b == PlayScript.Undefined._undefined))
			{
				return a;
			}
			return a << Convert.ToInt32(b);
		}

		public static object LeftShiftObjUInt (object a, uint b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if ((a == null) || (a == PlayScript.Undefined._undefined))
			{
				return (uint)0;
			}
			return Convert.ToInt32(a) << (int)b;
		}

		public static object LeftShiftUIntObj (uint a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if ((b == null) || (b == PlayScript.Undefined._undefined))
			{
				return a;
			}
			return (int)a << Convert.ToInt32(b);
		}

		public static object LeftShiftObjDouble (object a, double b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if ((a == null) || (a == PlayScript.Undefined._undefined))
			{
				return (int)0;
			}
			return Convert.ToInt32(a) << (int)b;
		}

		public static object LeftShiftDoubleObj (double a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if ((b == null) || (b == PlayScript.Undefined._undefined))
			{
				return a;
			}
			return (int)a << Convert.ToInt32(b);
		}

		public static object LeftShiftObjObj (object a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (a is int) {
				return LeftShiftIntObj ((int)a, b);
			} else if (a is uint) {
				return LeftShiftUIntObj ((uint)a, b);
			} else if (a is double) {
				return LeftShiftDoubleObj ((double)a, b);
			} else if (a is float) {
				return LeftShiftDoubleObj ((float)a, b);
			} else {
				ThrowOnInvalidOp (a, SHL);
				return null;
			}
		}

		public static object RightShiftObjInt (object a, int b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if ((a == null) || (a == PlayScript.Undefined._undefined))
			{
				return (uint)0;
			}
			if (a is int) {
				return ((int)a) >> (int)b;
			}
			if (a is uint) {
				return ((uint)a) >> (int)b;
			}
			return Convert.ToInt32(a) >> b;
		}

		public static object RightShiftIntObj (int a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if ((b == null) || (b == PlayScript.Undefined._undefined))
			{
				return a;
			}
			return a >> Convert.ToInt32(b);
		}

		public static object RightShiftObjUInt (object a, uint b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if ((a == null) || (a == PlayScript.Undefined._undefined))
			{
				return (uint)0;
			}
			if (a is int) {
				return ((int)a) >> (int)b;
			}
			if (a is uint) {
				return ((uint)a) >> (int)b;
			}
			return Convert.ToInt32(a) >> (int)b;
		}

		public static object RightShiftUIntObj (uint a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if ((b == null) || (b == PlayScript.Undefined._undefined))
			{
				return a;
			}
			return a >> Convert.ToInt32(b);
		}

		public static object RightShiftObjDouble (object a, double b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if ((a == null) || (a == PlayScript.Undefined._undefined))
			{
				return (int)0;
			}
			if (a is int) {
				return ((int)a) >> (int)b;
			}
			if (a is uint) {
				return ((uint)a) >> (int)b;
			}
			return Convert.ToInt32(a) >> (int)b;
		}

		public static object RightShiftDoubleObj (double a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if ((b == null) || (b == PlayScript.Undefined._undefined))
			{
				return a;
			}
			return (int)a >> Convert.ToInt32(b);
		}

		public static object RightShiftObjObj (object a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (a is int) {
				return RightShiftIntObj ((int)a, b);
			} else if (a is uint) {
				return RightShiftUIntObj ((uint)a, b);
			} else if (a is double) {
				return RightShiftDoubleObj ((double)a, b);
			} else if (a is float) {
				return RightShiftDoubleObj ((float)a, b);
			} else {
				ThrowOnInvalidOp (a, SHR);
				return null;
			}
		}

		public static bool LessThanObjInt (object a, int b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (a is int)
			{
				return (int)a < b;
			}
			if ((a == null) || (a == PlayScript.Undefined._undefined))
			{
				return false;
			}
			return Convert.ToDouble(a) < (double)b;
		}

		public static bool LessThanIntObj (int a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (b is int)
			{
				return a < (int)b;
			}
			if ((b == null) || (b == PlayScript.Undefined._undefined))
			{
				return false;
			}
			return a < Convert.ToDouble(b);
		}

		public static bool LessThanObjUInt (object a, uint b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (a is uint)
			{
				return (uint)a < b;
			}
			if ((a == null) || (a == PlayScript.Undefined._undefined))
			{
				return false;
			}
			return Convert.ToDouble(a) < (double)b;
		}

		public static bool LessThanUIntObj (uint a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (b is uint)
			{
				return a < (uint)b;
			}
			if ((b == null) || (b == PlayScript.Undefined._undefined))
			{
				return false;
			}
			return (double)a < Convert.ToDouble(b);
		}

		public static bool LessThanObjDouble (object a, double b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (a is double) 
			{
				return (double)a < b;
			}
			if ((a == null) || (a == PlayScript.Undefined._undefined))
			{
				return false;
			}
			return Convert.ToDouble(a) < b;
		}

		public static bool LessThanDoubleObj (double a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (b is double) 
			{
				return a < (double)b;
			}
			if ((b == null) || (b == PlayScript.Undefined._undefined))
			{
				return false;
			}
			return a < Convert.ToDouble(b);
		}

		public static bool LessThanObjString (object a, string b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (a is string) {
				return String.CompareOrdinal((string)a, b) < 0;
			} else {
				ThrowOnInvalidOp (a, LT);
				return false;
			}
		}

		public static bool LessThanStringObj (string a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (b is string) {
				return String.CompareOrdinal(a, (string)b) < 0;
			} else {
				ThrowOnInvalidOp (b, LT);
				return false;
			}
		}

		public static bool LessThanObjObj (object a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (a is int) {
				return LessThanIntObj ((int)a, b);
			} else if (a is string) {
				return LessThanStringObj ((string)a, b);
			} else if (a is double) {
				return LessThanDoubleObj ((double)a, b);
			} else if (a is float) {
				return LessThanDoubleObj ((float)a, b);
			} else if (a is uint) {
				return LessThanUIntObj ((uint)a, b);
			} else {
				ThrowOnInvalidOp (a, LT);
				return false;
			}
		}

		public static bool GreaterThanObjInt (object a, int b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (a is int)
			{
				return (int)a > b;
			}
			if ((a == null) || (a == PlayScript.Undefined._undefined))
			{
				return false;
			}
			return Convert.ToDouble(a) > (double)b;
		}

		public static bool GreaterThanIntObj (int a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (b is int)
			{
				return a > (int)b;
			}
			if ((b == null) || (b == PlayScript.Undefined._undefined))
			{
				return false;
			}
			return (double)a > Convert.ToDouble(b);
		}

		public static bool GreaterThanObjUInt (object a, uint b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (a is uint)
			{
				return (uint)a > b;
			}
			if ((a == null) || (a == PlayScript.Undefined._undefined))
			{
				return false;
			}
			return Convert.ToDouble(a) > (double)b;
		}

		public static bool GreaterThanUIntObj (uint a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (b is uint)
			{
				return a > (uint)b;
			}
			if ((b == null) || (b == PlayScript.Undefined._undefined))
			{
				return false;
			}
			return (double)a > Convert.ToDouble(b);
		}

		public static bool GreaterThanObjDouble (object a, double b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (a is double) 
			{
				return (double)a > b;
			}
			if ((a == null) || (a == PlayScript.Undefined._undefined))
			{
				return false;
			}
			return Convert.ToDouble(a) > b;
		}

		public static bool GreaterThanDoubleObj (double a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (b is double) 
			{
				return a > (double)b;
			}
			if ((b == null) || (b == PlayScript.Undefined._undefined))
			{
				return false;
			}
			return a > Convert.ToDouble(b);
		}

		public static bool GreaterThanObjString (object a, string b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (a is string) {
				return String.CompareOrdinal((string)a, b) > 0;
			} else {
				ThrowOnInvalidOp (a, GT);
				return false;
			}
		}

		public static bool GreaterThanStringObj (string a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (b is string) {
				return String.CompareOrdinal(a, (string)b) > 0;
			} else {
				ThrowOnInvalidOp (b, GT);
				return false;
			}
		}

		public static bool GreaterThanObjObj (object a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (a is int) {
				return GreaterThanIntObj ((int)a, b);
			} else if (a is string) {
				return GreaterThanStringObj ((string)a, b);
			} else if (a is double) {
				return GreaterThanDoubleObj ((double)a, b);
			} else if (a is float) {
				return GreaterThanDoubleObj ((float)a, b);
			} else if (a is uint) {
				return GreaterThanUIntObj ((uint)a, b);
			} else {
				ThrowOnInvalidOp (a, GT);
				return false;
			}
		}

		public static bool LessThanOrEqualObjInt (object a, int b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (a is int)
			{
				return (int)a <= b;
			}
			if (a is uint)
			{
				return (uint)a <= b;
			}
			if ((a == null) || (a == PlayScript.Undefined._undefined))
			{
				return false;
			}
			return Convert.ToDouble(a) <= (double)b;
		}

		public static bool LessThanOrEqualIntObj (int a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (b is int)
			{
				return a <= (int)b;
			}
			if ((b == null) || (b == PlayScript.Undefined._undefined))
			{
				return false;
			}
			return (double)a <= Convert.ToDouble(b);
		}

		public static bool LessThanOrEqualObjUInt (object a, uint b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (a is uint)
			{
				return (uint)a <= b;
			}
			if ((a == null) || (a == PlayScript.Undefined._undefined))
			{
				return false;
			}
			return Convert.ToDouble(a) <= (double)b;
		}

		public static bool LessThanOrEqualUIntObj (uint a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (b is uint)
			{
				return a <= (uint)b;
			}
			if ((b == null) || (b == PlayScript.Undefined._undefined))
			{
				return false;
			}
			return (double)a <= Convert.ToDouble(b);
		}

		public static bool LessThanOrEqualObjDouble (object a, double b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (a is double) 
			{
				return (double)a <= b;
			}
			if ((a == null) || (a == PlayScript.Undefined._undefined))
			{
				return false;
			}
			return Convert.ToDouble(a) <= (double)b;
		}

		public static bool LessThanOrEqualDoubleObj (double a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (b is double) 
			{
				return a <= (double)b;
			}
			if ((b == null) || (b == PlayScript.Undefined._undefined))
			{
				return false;
			}
			return (double)a <= Convert.ToDouble(b);
		}

		public static bool LessThanOrEqualObjString (object a, string b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (a is string) {
				return String.CompareOrdinal((string)a, b) <= 0;
			} else {
				ThrowOnInvalidOp (a, LTE);
				return false;
			}
		}

		public static bool LessThanOrEqualStringObj (string a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (b is string) {
				return String.CompareOrdinal(a, (string)b) <= 0;
			} else {
				ThrowOnInvalidOp (b, LTE);
				return false;
			}
		}

		public static bool LessThanOrEqualObjObj (object a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (a is int) {
				return LessThanOrEqualIntObj ((int)a, b);
			} else if (a is string) {
				return LessThanOrEqualStringObj ((string)a, b);
			} else if (a is double) {
				return LessThanOrEqualDoubleObj ((double)a, b);
			} else if (a is float) {
				return LessThanOrEqualDoubleObj ((float)a, b);
			} else if (a is uint) {
				return LessThanOrEqualUIntObj ((uint)a, b);
			} else {
				ThrowOnInvalidOp (a, LTE);
				return false;
			}
		}

		public static bool GreaterThanOrEqualObjInt (object a, int b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (a is int)
			{
				return (int)a >= b;
			}
			if ((a == null) || (a == PlayScript.Undefined._undefined))
			{
				return false;
			}
			return Convert.ToDouble(a) >= (double)b;
		}

		public static bool GreaterThanOrEqualIntObj (int a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (b is int)
			{
				return a >= (int)b;
			}
			if ((b == null) || (b == PlayScript.Undefined._undefined))
			{
				return false;
			}
			return (double)a >= Convert.ToDouble(b);
		}

		public static bool GreaterThanOrEqualObjUInt (object a, uint b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (a is uint)
			{
				return (uint)a >= b;
			}
			if ((a == null) || (a == PlayScript.Undefined._undefined))
			{
				return false;
			}
			return Convert.ToDouble(a) >= (double)b;
		}

		public static bool GreaterThanOrEqualUIntObj (uint a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (b is uint)
			{
				return a >= (uint)b;
			}
			if ((b == null) || (b == PlayScript.Undefined._undefined))
			{
				return false;
			}
			return (double)a >= Convert.ToDouble(b);
		}

		public static bool GreaterThanOrEqualObjDouble (object a, double b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if ((a == null) || (a == PlayScript.Undefined._undefined))
			{
				return false;
			}
			return Convert.ToDouble(a) >= b;
		}

		public static bool GreaterThanOrEqualDoubleObj (double a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if ((b == null) || (b == PlayScript.Undefined._undefined))
			{
				return false;
			}
			return a >= Convert.ToDouble(b);
		}

		public static bool GreaterThanOrEqualObjString (object a, string b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (a is string) {
				return String.CompareOrdinal((string)a, b) >= 0;
			} else {
				ThrowOnInvalidOp (a, GTE);
				return false;
			}
		}

		public static bool GreaterThanOrEqualStringObj (string a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (b is string) {
				return String.CompareOrdinal(a, (string)b) >= 0;
			} else {
				ThrowOnInvalidOp (b, GTE);
				return false;
			}
		}

		public static bool GreaterThanOrEqualObjObj (object a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (a is int) {
				return GreaterThanOrEqualIntObj ((int)a, b);
			} else if (a is string) {
				return GreaterThanOrEqualStringObj ((string)a, b);
			} else if (a is double) {
				return GreaterThanOrEqualDoubleObj ((double)a, b);
			} else if (a is float) {
				return GreaterThanOrEqualDoubleObj ((float)a, b);
			} else if (a is uint) {
				return GreaterThanOrEqualUIntObj ((uint)a, b);
			} else {
				ThrowOnInvalidOp (a, GTE);
				return false;
			}
		}

		public static bool EqualityObjInt (object a, int b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (a is int)
			{
				return (int)a == b;
			}
			if ((a == null) || (a == PlayScript.Undefined._undefined))
			{
				return false;
			}
			return Convert.ToDouble(a) == (double)b;	// Should we compare with an epsilon here?
		}

		public static bool EqualityIntObj (int a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (b is int)
			{
				return a == (int)b;
			}
			if ((b == null) || (b == PlayScript.Undefined._undefined))
			{
				return false;
			}
			return (double)a == Convert.ToDouble(b);	// Should we compare with an epsilon here?
		}

		public static bool EqualityObjUInt (object a, uint b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (a is uint)
			{
				return (uint)a == b;
			}
			if ((a == null) || (a == PlayScript.Undefined._undefined))
			{
				return false;
			}
			return Convert.ToDouble(a) == (double)b;	// Should we compare with an epsilon here?
		}

		public static bool EqualityUIntObj (uint a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (b is uint)
			{
				return a == (uint)b;
			}
			if ((b == null) || (b == PlayScript.Undefined._undefined))
			{
				return false;
			}
			return (double)a == Convert.ToDouble(b);	// Should we compare with an epsilon here?
		}

		public static bool EqualityObjDouble (object a, double b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (a is double) 
			{
				return (double)a == b;
			}
			if ((a == null) || (a == PlayScript.Undefined._undefined))
			{
				return false;
			}
			return Convert.ToDouble(a) == b;	// Should we compare with an epsilon here?
		}

		public static bool EqualityDoubleObj (double a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (b is double) 
			{
				return a == (double)b;
			}
			if ((b == null) || (b == PlayScript.Undefined._undefined))
			{
				return false;
			}
			return a == Convert.ToDouble(b);	// Should we compare with an epsilon here?
		}

		public static bool EqualityObjString (object a, string b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (a is string) {
				return String.CompareOrdinal((string)a, b) == 0;
			} else {
				ThrowOnInvalidOp (a, EQ);
				return false;
			}
		}

		public static bool EqualityStringObj (string a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (b is string) {
				return String.CompareOrdinal(a, (string)b) == 0;
			} else if (b == null) {
				return a == null;
			} else {
				ThrowOnInvalidOp (b, EQ);
				return false;
			}
		}

		public static bool EqualityObjBool (object a, bool b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			return Dynamic.CastObjectToBool(a) == b;
		}

		public static bool EqualityBoolObj (bool a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			return a == Dynamic.CastObjectToBool(b);
		}

		public static bool EqualityObjObj (object a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (a == PlayScript.Undefined._undefined) a = null;
			if (b == PlayScript.Undefined._undefined) b = null;

			if (a is int) {
				return EqualityIntObj ((int)a, b);
			} else if (a is string) {
				return EqualityStringObj ((string)a, b);
			} else if (a is double) {
				return EqualityDoubleObj ((double)a, b);
			} else if (a is float) {
				return EqualityDoubleObj ((float)a, b);
			} else if (a is bool) {
				return EqualityBoolObj ((bool)a, b);
			} else if (a is uint) {
				return EqualityUIntObj ((uint)a, b);
			} else if (a == b) {
				return true;
			} else if (a == null) {
				return false;
			} else {
				return a.Equals(b);
			}
		}

		public static bool InequalityObjInt (object a, int b)
		{
			return !EqualityObjInt(a, b);
		}

		public static bool InequalityIntObj (int a, object b)
		{
			return !EqualityIntObj(a, b);
		}

		public static bool InequalityObjUInt (object a, uint b)
		{
			return !EqualityObjUInt(a, b);
		}

		public static bool InequalityUIntObj (uint a, object b)
		{
			return !EqualityUIntObj(a, b);
		}

		public static bool InequalityObjDouble (object a, double b)
		{
			return !EqualityObjDouble(a, b);
		}

		public static bool InequalityDoubleObj (double a, object b)
		{
			return !EqualityDoubleObj(a, b);
		}

		public static bool InequalityObjString (object a, string b)
		{
			return !EqualityObjString(a, b);
		}

		public static bool InequalityStringObj (string a, object b)
		{
			return !EqualityStringObj(a, b);
		}

		public static bool InequalityObjBool (object a, bool b)
		{
			return !EqualityObjBool(a, b);
		}

		public static bool InequalityBoolObj (bool a, object b)
		{
			return !EqualityBoolObj(a, b);
		}

		public static bool InequalityObjObj (object a, object b)
		{
			return !EqualityObjObj(a, b);
		}

		// Logical bit operations are all using integer math (should not upconvert to double to improve accuracy)
		// However we still have to test for null and undefined
		// Here are the checks these operators have to do:
		//	1. If null and undefined, assume the value is 0 and apply the operation.
		//	2. Convert to Int32 (or UInt32) both operands and apply the operation.
		// Boolean uses Dynamic.CastObjectToBool() instead of Convert.ToXYZ().

		public static object BitwiseAndObjInt (object a, int b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if ((a == null) || (a == PlayScript.Undefined._undefined))
			{
				return (int)0;
			}
			return Convert.ToInt32(a) & b;
		}

		public static object BitwiseAndIntObj (int a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if ((b == null) || (b == PlayScript.Undefined._undefined))
			{
				return (int)0;
			}
			return a & Convert.ToInt32(b);
		}

		public static object BitwiseAndObjUInt (object a, uint b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if ((a == null) || (a == PlayScript.Undefined._undefined))
			{
				return (uint)0;
			}
			return Convert.ToUInt32(a) & b;
		}

		public static object BitwiseAndUIntObj (uint a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if ((b == null) || (b == PlayScript.Undefined._undefined))
			{
				return (uint)0;
			}
			return a & Convert.ToUInt32(b);
		}

		public static object BitwiseAndObjDouble (object a, double b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if ((a == null) || (a == PlayScript.Undefined._undefined))
			{
				return (int)0;
			}
			return Convert.ToInt32(a) & (int)b;
		}

		public static object BitwiseAndDoubleObj (double a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if ((b == null) || (b == PlayScript.Undefined._undefined))
			{
				return (int)0;
			}
			return (int)a & Convert.ToInt32(b);
		}

		public static object BitwiseAndObjBool (object a, bool b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (a is bool) {
				return (bool)a && b;
			} else {
				return Dynamic.CastObjectToBool(a) && b;
			}
		}

		public static object BitwiseAndBoolObj (bool a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (b is bool) {
				return a && (bool)b;
			} else {
				return a && Dynamic.CastObjectToBool(b);
			}
		}
		public static object BitwiseAndObjObj (object a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (a is int) {
				return BitwiseAndIntObj ((int)a, b);
			} else if (a is double) {
				return BitwiseAndDoubleObj ((double)a, b);
			} else if (a is float) {
				return BitwiseAndDoubleObj ((float)a, b);
			} else if (a is bool) {
				return BitwiseAndBoolObj ((bool)a, b);
			} else if (a is uint) {
				return BitwiseAndUIntObj ((uint)a, b);
			} else {
				return Dynamic.CastObjectToBool(a) && Dynamic.CastObjectToBool(b);
			}
		}

		public static object BitwiseOrObjInt (object a, int b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if ((a == null) || (a == PlayScript.Undefined._undefined))
			{
				return b;
			}
			return Convert.ToInt32(a) | b;
		}

		public static object BitwiseOrIntObj (int a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if ((b == null) || (b == PlayScript.Undefined._undefined))
			{
				return a;
			}
			return a | Convert.ToInt32(b);
		}

		public static object BitwiseOrObjUInt (object a, uint b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if ((a == null) || (a == PlayScript.Undefined._undefined))
			{
				return b;
			}
			return Convert.ToUInt32(a) | b;
		}

		public static object BitwiseOrUIntObj (uint a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if ((b == null) || (b == PlayScript.Undefined._undefined))
			{
				return a;
			}
			return a | Convert.ToUInt32(b);
		}

		public static object BitwiseOrObjDouble (object a, double b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if ((a == null) || (a == PlayScript.Undefined._undefined))
			{
				return b;
			}
			return Convert.ToInt32(a) | (int)b;
		}

		public static object BitwiseOrDoubleObj (double a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if ((b == null) || (b == PlayScript.Undefined._undefined))
			{
				return a;
			}
			return (int)a | Convert.ToInt32(b);
		}

		public static object BitwiseOrObjBool (object a, bool b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (a is bool) {
				return (bool)a || b;
			} else {
				return Dynamic.CastObjectToBool(a) || b;
			}
		}

		public static object BitwiseOrBoolObj (bool a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (b is bool) {
				return a || (bool)b;
			} else {
				return a || Dynamic.CastObjectToBool(b);
			}
		}

		public static object BitwiseOrObjObj (object a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (a is int) {
				return BitwiseOrIntObj ((int)a, b);
			} else if (a is double) {
				return BitwiseOrDoubleObj ((double)a, b);
			} else if (a is float) {
				return BitwiseOrDoubleObj ((float)a, b);
			} else if (a is bool) {
				return BitwiseOrBoolObj ((bool)a, b);
			} else if (a is uint) {
				return BitwiseOrUIntObj ((uint)a, b);
			} else {
				return Dynamic.CastObjectToBool(a) || Dynamic.CastObjectToBool(b);
			}
		}

		public static object ExclusiveOrObjInt (object a, int b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if ((a == null) || (a == PlayScript.Undefined._undefined))
			{
				return b;
			}
			return Convert.ToInt32(a) ^ b;
		}

		public static object ExclusiveOrIntObj (int a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if ((b == null) || (b == PlayScript.Undefined._undefined))
			{
				return a;
			}
			return a ^ Convert.ToInt32(b);
		}

		public static object ExclusiveOrObjUInt (object a, uint b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if ((a == null) || (a == PlayScript.Undefined._undefined))
			{
				return b;
			}
			return Convert.ToUInt32(a) ^ b;
		}

		public static object ExclusiveOrUIntObj (uint a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if ((b == null) || (b == PlayScript.Undefined._undefined))
			{
				return a;
			}
			return a ^ Convert.ToUInt32(b);
		}

		public static object ExclusiveOrObjDouble (object a, double b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if ((a == null) || (a == PlayScript.Undefined._undefined))
			{
				return b;
			}
			return Convert.ToInt32(a) ^ (int)b;
		}

		public static object ExclusiveOrDoubleObj (double a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if ((b == null) || (b == PlayScript.Undefined._undefined))
			{
				return a;
			}
			return (int)a ^ Convert.ToInt32(b);
		}

		public static object ExclusiveOrObjBool (object a, bool b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			return Dynamic.CastObjectToBool(a) ^ b;
		}

		public static object ExclusiveOrBoolObj (bool a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			return a ^ Dynamic.CastObjectToBool(b);
		}

		public static object ExclusiveOrObjObj (object a, object b)
		{
			Stats.Increment(StatsCounter.BinaryOperationBinderInvoked);

			if (a is int) {
				return ExclusiveOrIntObj ((int)a, b);
			} else if (a is double) {
				return ExclusiveOrDoubleObj ((double)a, b);
			} else if (a is float) {
				return ExclusiveOrDoubleObj ((float)a, b);
			} else if (a is bool) {
				return ExclusiveOrBoolObj ((bool)a, b);
			} else if (a is uint) {
				return ExclusiveOrUIntObj ((uint)a, b);
			} else {
				ThrowOnInvalidOp (b, XOR);
				return null;
			}
		}
	}

}


#endif
