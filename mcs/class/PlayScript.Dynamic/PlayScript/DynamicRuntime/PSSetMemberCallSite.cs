//
// PSSetMemberCallSite.cs
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
using System.Reflection;
using System.Collections;

namespace PlayScript.DynamicRuntime
{
	public class PSSetMemberCallSite
	{
		public PSSetMemberCallSite(string name)
		{
			mName = name;
		}

		public void SetNamedMember<T>( object o, string name, T value)
		{
			// if name has changed then invalidate type
			if (mName != name)
			{
				mName = name;
				mType = null;
			}

			SetMemberAsObject(o, (object)value, typeof(T) != typeof(System.Object) );
		}


		public void SetMember<T>(object o, T value)
		{
			// box value as object
			SetMemberAsObject(o, (object)value, true);
		}

		public void SetMemberAsObject(object o, object value, bool valueTypeIsConstant)
		{
			// resolve as dictionary 
			var dict = o as IDictionary;
			if (dict != null) 
			{
				// special case this since it happens so much in object initialization
				dict[mName] = value;
				return;
			}

			// determine if this is a instance member or a static member
			bool isStatic;
			Type otype;
			if (o is System.Type) {
				// static member
				otype = (System.Type)o;
				o = null;
				isStatic = true;
			} else {
				// instance member
				otype = o.GetType();
				isStatic = false;
			}

			// see if binding type is the same
			if (otype == mType)
			{
				// perform (optional) value conversion
				object newValue;
				if (mValueConverter != null) {
					newValue = mValueConverter(value, mTargetType);
				} else {
					newValue = value;
				}

				// use cached resolve
				if (mProperty != null) {
					//					mProperty.SetValue(o, newValue);
					mArgs[0] = newValue;
					mPropertySetter.Invoke(o, BindingFlags.SuppressChangeType, null, mArgs, null);
					//					Exception ex = null;
					//					mPropertySetter.InternalInvoke(o, mArgs, out ex);
					return;
				}

				if (mField != null) {
					mField.SetValue(o, newValue);
					return;
				}

				// resolve as dynamic class
				var dc = o as IDynamicClass;
				if (dc != null) 
				{
					dc.__SetDynamicValue(mName, newValue);
					return;
				}

				throw new System.InvalidOperationException("Unhandled member type in PSSetMemberBinder");
			}

			// resolve name

			// resolve as property
			var property = otype.GetProperty(mName);
			if (property != null)
			{
				// found property
				var setter = property.GetSetMethod();
				if (setter != null && setter.IsPublic && setter.IsStatic == isStatic) 
				{
					// setup binding to property
					mType     = otype;
					mProperty = property;
					mPropertySetter = property.GetSetMethod();
					mField    = null;
					mTargetType = mProperty.PropertyType;

					// resolve conversion function
					mValueConverter = PSConverter.GetConversionFunction(value, mTargetType, valueTypeIsConstant);
					if (mValueConverter != null) {
						mArgs[0] = mValueConverter(value, mTargetType);
					} else {
						mArgs[0] = value;
					}
					mPropertySetter.Invoke(o, mArgs);
					return;
				}
			}

			// resolve as field
			var field = otype.GetField(mName);
			if (field != null)
			{
				// found field
				if (field.IsPublic && field.IsStatic == isStatic) {
					// setup binding to field
					mType     = otype;
					mProperty = null;
					mField    = field;
					mTargetType = mField.FieldType;

					// resolve conversion function
					mValueConverter = PSConverter.GetConversionFunction(value, mTargetType, valueTypeIsConstant);
					object newValue;
					if (mValueConverter != null) {
						newValue = mValueConverter(value, mTargetType);
					} else {
						newValue = value;
					}

					mField.SetValue(o, newValue);
					return;
				}
			}

			if (o is IDynamicClass)
			{
				// dynamic class
				mType     = otype;
				mProperty = null;
				mField    = null;
				((IDynamicClass)o).__SetDynamicValue(mName, value);
				return;
			}		
		}



		private string 		   mName;
		private Type 		   mType;
		private PropertyInfo   mProperty;
		private FieldInfo      mField;
		private MethodInfo     mPropertySetter;
		private Func<object, Type, object> mValueConverter;
		private Type 		   mTargetType;
		private object[]       mArgs = new object[1];

	};
}
#endif
