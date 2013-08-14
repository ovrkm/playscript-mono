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
 * Portions created by the Initial Developer are Copyright (C) 2004-2006
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




package DefaultClass {

        import DefaultClass.*

	public final class PubFinExtDefaultClass extends DefaultClass {

	    // ************************************
	    // access default method of parent
	    // from default method of sub class
	    // ************************************

	    function subGetArray() : Array { return this.getArray():*; }
	    function subSetArray(a:Array):* { this.setArray(a):*; }

	    // ************************************
	    // access default method of parent
	    // from public method of sub class
	    // ************************************

	    public function pubSubGetArray() : Array { return this.getArray():*; }
	    public function pubSubSetArray(a:Array):* { this.setArray(a):*; }
	    
	    // this is needed so that the test cases can access this from
	    // outside the class.  This way the test case itself preserved
	    public function testSubGetSetArray(a:Array) : Array {
	    	this.subSetArray(a);
	    	return this.subGetArray();
	    }

	    // ************************************
	    // access default method of parent
	    // from private method of sub class
	    // ************************************

	    private function privSubGetArray() : Array { return this.getArray():*; }
	    private function privSubSetArray(a:Array):* { this.setArray(a):*; }

	    // function to test above from test scripts
	    public function testPrivSubArray(a:Array) : Array {
		this.privSubSetArray(a);
		return this.privSubGetArray();
	    }

	    // ************************************
	    // access default method of parent
	    // from final method of sub class
	    // ************************************

	    final function finGetArray() : Array { return this.getArray():*; }
	    final function finSetArray(a:Array):* { this.setArray(a):*; }
	
	    // this is needed so that the test cases can access this from
	    // outside the class.  This way the test case itself preserved
	    public function testFinSubArray(a:Array) : Array {
	    	this.subSetArray(a);
	    	return this.subGetArray();
	    }
	    
	    // ************************************
	    // access default method of parent
	    // from static method of sub class
	    // ************************************

	    static function statGetArray() : Array { return getArray():*; }
	    static function statSetArray(a:Array):* { setArray(a):*; }	    
	    // this is needed so that the test cases can access this from
	    // outside the class.  This way the test case itself preserved
	    public function testStatSubArray(a:Array) : Array {
	    	this.statSetArray(a);
	    	return this.statGetArray();
	    }	    
	    
	    // ************************************
	    // access default method of parent
	    // from public final method of sub class
	    // ************************************
	    
	    public final function pubFinSubGetArray() : Array { return this.getArray():*; }
	    final public function pubFinSubSetArray(a:Array):* { this.setArray(a):*; }
	    
	    // ************************************
	    // access default method of parent
	    // from private final method of sub class
	    // ************************************
	    
	    private final function privFinSubGetArray() : Array { return this.getArray():*; }
	    final private function privFinSubSetArray(a:Array):* { this.setArray(a):*; }
	   
	    public function testPrivFinSubArray(a:Array):Array {
	    	this.privFinSubSetArray(a);
	    	return this.privFinSubGetArray();
	    }
	   
	    
	    // ************************************
	    // access default method of parent
	    // from virtual method of sub class
	    // ************************************
	    
	    virtual function virtSubGetArray() : Array { return this.getArray():*; }
	    virtual function virtSubSetArray(a:Array):* { this.setArray(a):*; }
	    
	    public function testVirtSubArray(a:Array) : Array {
	    	this.virtSubSetArray(a);
	    	return this.virtSubGetArray();
	    }
	    
	    // ************************************
	    // access default method of parent
	    // from public virtual method of sub class
	    // ************************************
	    
	    virtual public function pubVirtSubGetArray() : Array { return this.getArray():*; }
	    public virtual function pubVirtSubSetArray(a:Array):* { this.setArray(a):*; }
	    
	    // ************************************
	    // access default method of parent
	    // from private virtual method of sub class
	    // ************************************
	    
	    virtual private function privVirtSubGetArray() : Array { return this.getArray():*; }
	    private virtual function privVirtSubSetArray(a:Array):* { this.setArray(a):*; }
	    
	    public function testPrivVirtSubArray(a:Array) : Array {
	    	this.privVirtSubSetArray(a);
	    	return this.privVirtSubGetArray();
	    }


	    // ***************************************
	    // access default property from
	    // default method of sub class
	    // ***************************************

	    function subGetDPArray() : Array { return array; }
	    function subSetDPArray(a:Array):* { array = a; }
	
	    // this is needed so that the test cases can access this from
	    // outside the class.  This way the test case itself preserved
	    public function testSubGetSetDPArray(a:Array) : Array {
		this.subSetDPArray(a);
		return this.subGetDPArray();
	    }

	    // ***************************************
	    // access default property from
	    // public method of sub class
	    // ***************************************

	    public function pubSubGetDPArray() : Array { return array; }
	    public function pubSubSetDPArray(a:Array):* { array = a; }

	    // ***************************************
	    // access default property from
	    // private method of sub class
	    // ***************************************

	    private function privSubGetDPArray() : Array { return array; }
	    private function privSubSetDPArray(a:Array):* { array = a; }

	    public function testPrivSubDPArray(a:Array) : Array {
		this.privSubSetDPArray(a);
		return this.privSubGetDPArray();
            }


	    // ***************************************
	    // access default property from
	    // final method of sub class
	    // ***************************************

	    final function finSubGetDPArray() : Array { return array; }
	    final function finSubSetDPArray(a:Array):* { array = a; }
	    
	    public function testFinSubDPArray(a:Array) : Array {
	    	this.finSubSetDPArray(a);
	    	return this.finSubGetDPArray();
	    }
	    
	    // ***************************************
	    // access default property from 
	    // virtual method of sub class
	    // ***************************************
	    
	    virtual function virtSubGetDPArray() : Array { return array; }
	    virtual function virtSubSetDPArray(a:Array):* { array = a; }
	    
	    public function testVirtSubDPArray(a:Array) : Array {
	    	this.virtSubSetDPArray(a);
	    	return this.virtSubGetDPArray();
	    }
	    
	    // ***************************************
	    // access default property from
	    // public virtual method of sub class
	    // ***************************************
	    
	    public virtual function pubVirtSubGetDPArray() : Array { return array; }
	    virtual public function pubVirtSubSetDPArray(a:Array):* { array = a; }
	    
	    // ***************************************
	    // access default property from
	    // private final method of sub class
	    // ***************************************
	    
	    private final function privFinSubGetDPArray() : Array { return array; }
	    final private function privFinSubSetDPArray(a:Array):* { array = a; }
	    
	    public function testPrivFinSubDPArray(a:Array):Array {
	    	this.privFinSubSetDPArray(a);
	    	return this.privFinSubGetDPArray();
	    }
	    
	    // ***************************************
	    // access default property from
	    // private virtual method of sub class
	    // ***************************************
	    
	    private virtual function privVirtSubGetDPArray() : Array { return array; }
	    virtual private function privVirtSubSetDPArray(a:Array):* { array = a; }
	    
	    public function testPrivVirtSubDPArray(a:Array):Array {
	    	this.privVirtSubSetDPArray(a);
	    	return this.privVirtSubGetDPArray();
	    }
	    
	    // ***************************************
	    // access default property from
	    // public final method of sub class
	    // ***************************************
	    
	    public final function pubFinSubGetDPArray() : Array { return array; }
            final public function pubFinSubSetDPArray(a:Array):* { array = a; }
	   

	}

}
