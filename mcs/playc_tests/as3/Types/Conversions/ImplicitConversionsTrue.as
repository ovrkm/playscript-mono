/* ***** BEGIN LICENSE BLOCK *****
 * Version: MPL 1.1/GPL 2.0/LGPL 2.1
 *
 * The contents of this file are subject to the Mozilla Public License Version
 * 1.1 (the "License"); you may not use this file except in compliance with
 * the License. You may obtain a copy of the License at
 * http://www.mozilla.org/MPL/
 *
 * Software distributed under the License is distributed on an "AS IS" basis,
 * WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License
 * for the specific language governing rights and limitations under the
 * License.
 *
 * The Original Code is [Open Source Virtual Machine.].
 *
 * The Initial Developer of the Original Code is
 * Adobe System Incorporated.
 * Portions created by the Initial Developer are Copyright (C) 2005-2006
 * the Initial Developer. All Rights Reserved.
 *
 * Contributor(s):
 *   Adobe AS3 Team
 *
 * Alternatively, the contents of this file may be used under the terms of
 * either the GNU General Public License Version 2 or later (the "GPL"), or
 * the GNU Lesser General Public License Version 2.1 or later (the "LGPL"),
 * in which case the provisions of the GPL or the LGPL are applicable instead
 * of those above. If you wish to allow use of your version of this file only
 * under the terms of either the GPL or the LGPL, and not to allow others to
 * use your version of this file under the terms of the MPL, indicate your
 * decision by deleting the provisions above and replace them with the notice
 * and other provisions required by the GPL or the LGPL. If you do not delete
 * the provisions above, a recipient may use your version of this file under
 * the terms of any one of the MPL, the GPL or the LGPL.
 *
 * ***** END LICENSE BLOCK ***** */

package {
	public class ImplicitConversionsTrueTest extends BaseTest {
		public static function Main():int {
			var SECTION:* = "Types: Conversions";
			var VERSION:* = "as3";
			var TITLE:*   = "implicit type conversions";
			
			startTest();
			
			
			// Value = true 
			
			var thisError:* = "no exception thrown";
			try{
				var string:String = true;
			} catch (e0:*) {
				thisError = e0.toString();
			} finally {
				AddTestCase("string:String = true", "no exception thrown", typeError(thisError) );
				AddTestCase("string:String = true", "true", string);
			}
			
			thisError = "no exception thrown";
			try{
				var number:Number = true;
			} catch (e:*) {
				thisError = e.toString();
			} finally {
				AddTestCase("number:Number = true", "no exception thrown", typeError(thisError) );
				AddTestCase("number:Number = true", 1, number);
			}
			
			thisError = "no exception thrown";
			try{
				var myInt:int = true;
			} catch(e1) {
				thisError = e1.toString();
			} finally {
				AddTestCase("myInt:int = true", "no exception thrown", typeError(thisError) );
				AddTestCase("myInt:int = true", 1, myInt);
			}
			
			thisError = "no exception thrown";
			try{
				var myUint:uint = true;
			} catch(e2) {
				thisError = e2.toString();
			} finally {
				AddTestCase("myUInt:uint = true", "no exception thrown", typeError(thisError) );
				AddTestCase("myUInt:uint = true", 1, myUint);
			}
			
			thisError = "no exception thrown";
			try{
				var boolean:Boolean = true;
			} catch(e3) {
				thisError = e3.toString();
			} finally {
				AddTestCase("boolean:Boolean = true", "no exception thrown", typeError(thisError) );
			}
			
			var object:Object = true;
			AddTestCase( "var object:Object = true", true, object);
			
			
			test();
			
			
			
			return results();
		}
	}
}
