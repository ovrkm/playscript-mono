package
{
	import flash.display.DisplayObject;
	import System.*;

	public class inlineAttribute extends System.Attribute {
	}

	public class Texture
	{
		public function Texture(i:int, j:int=2)
		{
			mProp = i;
		}
//
//		public function Texture(i:Number)
//		{
//			mValue = int(i);
//		}

		public function doWork():void
		{
			trace("doWork");
		}

		public function doWork2(i:int):void
		{
			trace("doWork");
		}

		public var field:int  = 5;
		public function get prop(): int {
			return mProp;
		}


		public static var staticVal:int = 100;

		private var mProp:int;

	}

	
	public class Shader
	{
		public function Shader(i:int)
		{
			mProp = i;
		}
		
		public var field:int  = 5;
		public function get prop(): int {
			return mProp;
		}
		
		
		private var mProp:int;
		
	}

	public class Test 
	{
//		public static function Test2(t:Texture):void {
//		}

		public static function Test2(i:int):void {
		}

		public static function Test2(b:Boolean):void {
		}

		private static function OnGetError(o:System.Object, name:String, type:System.Type):System.Object
		{
			return undefined;
		}

		private static function OnSetError(o:System.Object, name:String, value:System.Object):void
		{
		}

		public static function Main():void {

			PlayScript.RuntimeBinder.Binder.OnGetMemberError = Test.OnGetError;
			PlayScript.RuntimeBinder.Binder.OnSetMemberError = Test.OnSetError;

			var expando:Object = {a:5, b:6, c:7};
			expando.a = 12;
			expando["a"] = 15;
			trace(expando["a"]);

			var ai:int = expando.a;
			var zi:int = expando.z;

			trace(ai);

			var texture:Texture = new Texture(5);

			var myobj:Object = texture;
			for (var i:int = 0; i < 10; i++) {
				myobj.doWork();
			}
			myobj.doWork2(5);
			myobj.doWork3();


			var func:Function = texture["doWork"];
			func();
			texture["field"] = 777;
			texture["field"] = 1999.3423423;
			trace(texture["field"]);
			texture["foop"] = 1999;


			trace(texture);

			var typeo:Object = Texture;
			trace(typeo.staticVal);

			var array:Array = new Array();
			for (var i:int = 0; i < 10; i++) {
				array.push (new Texture(i));
				array.push (new Shader(i+100));
			}

			var ot:Object = texture;


			for (var i:int = 0; i < 10; i++) {
				var x:Number = array[i].field;
				trace(x);

				array[i].field = 1012312 + i;
				trace(array[i].field);
				trace(array[i].prop);
//				trace(array[i].xss);

//				System.Console.WriteLine("{0} {1}", ot.field, ot.prop);
			}



//			var test:PlayScript.Expando.ExpandoObject = new Object({a:5, b:6, c:7});
//			trace(test.a);
//			var i:int  = 5;
//			var o:Object = new Array(5);
//
//			var texture:Texture = new Texture(o);
//			trace(texture);
//
//			Test2 (o);

//
//			Test2(otexture);

//			for each (var x:Object in o)
//			{
//				trace(x);
//			}

//			
//			var list:Vector.<int> = new Vector.<int>;
//			var list2:Vector.<int> = new Vector.<int>();
//			trace(list);
//			trace(list2);
//
//			var list3:System.Collections.Generic.List.<int> = new System.Collections.Generic.List.<int>;
//			var list4:System.Collections.Generic.List.<int> = new System.Collections.Generic.List.<int>();
//			trace(list3);
//			trace(list4);
//

//			var a:Array = new Array();
//			var cl:Class = Array;
//			trace(a is cl);
		}

//		[inline]
//		public function set texture(texture:Texture):void { _texture = texture; }


	}

}
