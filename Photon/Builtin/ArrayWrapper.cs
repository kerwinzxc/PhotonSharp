﻿// Generated by github.com/davyxu/PhotonSharp
// DO NOT EDIT!!
using Photon;
using System;

namespace Photon
{
	[NativeWrapperClass(typeof(Photon.Array))]
	public class ArrayWrapper
	{
		[NativeEntry(NativeEntryType.Property)]
		public static void Count(object phoIns, ref object value, bool isGet )
		{
			var phoClassIns = phoIns as Array;
			
			if (isGet)
			{
				value = Convertor.Integer32ToValue(phoClassIns.Count);
			}
		}
		
		[NativeEntry(NativeEntryType.ClassMethod)]
		public static int Add( VMachine phoVM )
		{
			var phoClassIns = phoVM.DataStack.GetNativeInstance<Photon.Array>(0);
			
			var v = phoVM.DataStack.GetValue(1);
			
			phoClassIns.Add( v );

			
			return 0;
		}
		
		[NativeEntry(NativeEntryType.ClassMethod)]
		public static int Remove( VMachine phoVM )
		{
			var phoClassIns = phoVM.DataStack.GetNativeInstance<Photon.Array>(0);
			
			var v = phoVM.DataStack.GetValue(1);
			
			phoClassIns.Remove( v );

			
			return 0;
		}
		
		[NativeEntry(NativeEntryType.ClassMethod)]
		public static int RemoveAt( VMachine phoVM )
		{
			var phoClassIns = phoVM.DataStack.GetNativeInstance<Photon.Array>(0);
			
			var index = phoVM.DataStack.GetInteger32(1);
			
			phoClassIns.RemoveAt( index );

			
			return 0;
		}
		
		[NativeEntry(NativeEntryType.ClassMethod)]
		public static int TryGet( VMachine phoVM )
		{
			var phoClassIns = phoVM.DataStack.GetNativeInstance<Photon.Array>(0);
			
			var index = phoVM.DataStack.GetInteger32(1);
			
			Value v = default(Value);
			var phoRetArg = phoClassIns.TryGet( index, out v );

			phoVM.DataStack.PushValue( v );
			phoVM.DataStack.PushBool( phoRetArg );
			
			return 2;
		}
		
		[NativeEntry(NativeEntryType.ClassMethod)]
		public static int TrySet( VMachine phoVM )
		{
			var phoClassIns = phoVM.DataStack.GetNativeInstance<Photon.Array>(0);
			
			var index = phoVM.DataStack.GetInteger32(1);
			var v = phoVM.DataStack.GetValue(2);
			
			var phoRetArg = phoClassIns.TrySet( index, v );

			phoVM.DataStack.PushBool( phoRetArg );
			
			return 1;
		}
		
		[NativeEntry(NativeEntryType.ClassMethod)]
		public static int Get( VMachine phoVM )
		{
			var phoClassIns = phoVM.DataStack.GetNativeInstance<Photon.Array>(0);
			
			var index = phoVM.DataStack.GetInteger32(1);
			
			var phoRetArg = phoClassIns.Get( index );

			phoVM.DataStack.PushValue( phoRetArg );
			
			return 1;
		}
		
		[NativeEntry(NativeEntryType.ClassMethod)]
		public static int Set( VMachine phoVM )
		{
			var phoClassIns = phoVM.DataStack.GetNativeInstance<Photon.Array>(0);
			
			var index = phoVM.DataStack.GetInteger32(1);
			var v = phoVM.DataStack.GetValue(2);
			
			phoClassIns.Set( index, v );

			
			return 0;
		}
		
		[NativeEntry(NativeEntryType.ClassMethod)]
		public static int Contains( VMachine phoVM )
		{
			var phoClassIns = phoVM.DataStack.GetNativeInstance<Photon.Array>(0);
			
			var v = phoVM.DataStack.GetValue(1);
			
			var phoRetArg = phoClassIns.Contains( v );

			phoVM.DataStack.PushBool( phoRetArg );
			
			return 1;
		}
		
		[NativeEntry(NativeEntryType.ClassMethod)]
		public static int IndexOf( VMachine phoVM )
		{
			var phoClassIns = phoVM.DataStack.GetNativeInstance<Photon.Array>(0);
			
			var v = phoVM.DataStack.GetValue(1);
			
			var phoRetArg = phoClassIns.IndexOf( v );

			phoVM.DataStack.PushInteger32( phoRetArg );
			
			return 1;
		}
		
		[NativeEntry(NativeEntryType.ClassMethod)]
		public static int Clear( VMachine phoVM )
		{
			var phoClassIns = phoVM.DataStack.GetNativeInstance<Photon.Array>(0);
			
			phoClassIns.Clear(  );

			
			return 0;
		}
		
	}
}
