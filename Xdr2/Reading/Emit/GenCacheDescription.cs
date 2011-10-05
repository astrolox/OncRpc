﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;
using System.Reflection;

namespace Xdr2.Reading.Emit
{
	public class GenCacheDescription
	{
		public readonly Type Result;

		public GenCacheDescription(ModuleBuilder modBuilder, DelegateCacheDescription delegCacheDesc, string name, OpaqueType mType)
		{
			TypeBuilder typeBuilder = modBuilder.DefineType(name,
				TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.Abstract | TypeAttributes.Sealed);
			
			GenericTypeParameterBuilder genTypeParam = typeBuilder.DefineGenericParameters("T")[0];
			
			Type instanceType;
			if (mType == OpaqueType.One)
				instanceType = typeof(ReadOneDelegate<>);
			else
				instanceType = typeof(ReadManyDelegate<>);
				
			typeBuilder.DefineField("Instance",
				instanceType.MakeGenericType(genTypeParam),
				FieldAttributes.Public | FieldAttributes.Static);

			ConstructorBuilder ctor = typeBuilder.DefineConstructor(MethodAttributes.Static, CallingConventions.Standard, new Type[0]);
			ILGenerator il = ctor.GetILGenerator();
			
			il.Emit(OpCodes.Ldsfld, delegCacheDesc.BuildRequest);
			il.Emit(OpCodes.Ldtoken, genTypeParam);
			il.Emit(OpCodes.Call, typeof(Type).GetMethod("GetTypeFromHandle"));
			il.Emit(OpCodes.Ldc_I4_S, (int)mType);
			il.Emit(OpCodes.Callvirt, typeof(Action<Type, OpaqueType>).GetMethod("Invoke"));
			il.Emit(OpCodes.Ret);

			Result = typeBuilder.CreateType();
		}
		
		public FieldInfo Instance(Type genType)
		{
			return TypeBuilder.GetField(
				Result.MakeGenericType(genType),
				Result.GetField("Instance"));
		}
	}

}
