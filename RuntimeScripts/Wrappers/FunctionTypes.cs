using System;
using System.Linq;
using System.Reflection;

namespace pbuddy.TypeScriptingUtility.RuntimeScripts
{
	public static class FunctionTypes
	{
		public static bool UsesParams(this ParameterInfo param) => param.GetCachedParamsArrayAttributes().Length > 0;

		public static MethodInfo[] GetMatchingMethods(this Type type, string name, bool withParams = false)
		{
			static Type[] ArgTypes(int count) => Enumerable.Repeat(typeof(object), count).ToArray();
			
			static Type[] ArgTypesWithParams(int count)
			{
				if (count == 0) return Type.EmptyTypes;
				Type[] collection = Enumerable.Repeat(typeof(object), count).ToArray();
				collection[^1] = typeof(object[]);
				return collection;
			}

			static MethodInfo GetMethodForArgCount(Type type, string name, int count, bool withParams) =>
				type.GetMethod(name, withParams ? ArgTypesWithParams(count) : ArgTypes(count));
			
			return Enumerable.Range(0, Count)
				.Select(i => GetMethodForArgCount(type, name, i, withParams))
				.ToArray();
		}

		#region Generated Content
		public static int Count => 17;

		public static readonly Type[] GenericActions = { typeof(Action), typeof(Action<>), typeof(Action<,>), typeof(Action<,,>), typeof(Action<,,,>), typeof(Action<,,,,>), typeof(Action<,,,,,>), typeof(Action<,,,,,,>), typeof(Action<,,,,,,,>), typeof(Action<,,,,,,,,>), typeof(Action<,,,,,,,,,>), typeof(Action<,,,,,,,,,,>), typeof(Action<,,,,,,,,,,,>), typeof(Action<,,,,,,,,,,,,>), typeof(Action<,,,,,,,,,,,,,>), typeof(Action<,,,,,,,,,,,,,,>), typeof(Action<,,,,,,,,,,,,,,,>) };

		public static readonly Type[] GenericFunctions = { typeof(Func<>), typeof(Func<,>), typeof(Func<,,>), typeof(Func<,,,>), typeof(Func<,,,,>), typeof(Func<,,,,,>), typeof(Func<,,,,,,>), typeof(Func<,,,,,,,>), typeof(Func<,,,,,,,,>), typeof(Func<,,,,,,,,,>), typeof(Func<,,,,,,,,,,>), typeof(Func<,,,,,,,,,,,>), typeof(Func<,,,,,,,,,,,,>), typeof(Func<,,,,,,,,,,,,,>), typeof(Func<,,,,,,,,,,,,,,>), typeof(Func<,,,,,,,,,,,,,,,>), typeof(Func<,,,,,,,,,,,,,,,,>) };

		public static readonly Type[] ObjectActions = { typeof(Action), typeof(Action<object>), typeof(Action<object,object>), typeof(Action<object,object,object>), typeof(Action<object,object,object,object>), typeof(Action<object,object,object,object,object>), typeof(Action<object,object,object,object,object,object>), typeof(Action<object,object,object,object,object,object,object>), typeof(Action<object,object,object,object,object,object,object,object>), typeof(Action<object,object,object,object,object,object,object,object,object>), typeof(Action<object,object,object,object,object,object,object,object,object,object>), typeof(Action<object,object,object,object,object,object,object,object,object,object,object>), typeof(Action<object,object,object,object,object,object,object,object,object,object,object,object>), typeof(Action<object,object,object,object,object,object,object,object,object,object,object,object,object>), typeof(Action<object,object,object,object,object,object,object,object,object,object,object,object,object,object>), typeof(Action<object,object,object,object,object,object,object,object,object,object,object,object,object,object,object>), typeof(Action<object,object,object,object,object,object,object,object,object,object,object,object,object,object,object,object>) };

		public static readonly Type[] ObjectFunctions = { typeof(Func<object>), typeof(Func<object,object>), typeof(Func<object,object,object>), typeof(Func<object,object,object,object>), typeof(Func<object,object,object,object,object>), typeof(Func<object,object,object,object,object,object>), typeof(Func<object,object,object,object,object,object,object>), typeof(Func<object,object,object,object,object,object,object,object>), typeof(Func<object,object,object,object,object,object,object,object,object>), typeof(Func<object,object,object,object,object,object,object,object,object,object>), typeof(Func<object,object,object,object,object,object,object,object,object,object,object>), typeof(Func<object,object,object,object,object,object,object,object,object,object,object,object>), typeof(Func<object,object,object,object,object,object,object,object,object,object,object,object,object>), typeof(Func<object,object,object,object,object,object,object,object,object,object,object,object,object,object>), typeof(Func<object,object,object,object,object,object,object,object,object,object,object,object,object,object,object>), typeof(Func<object,object,object,object,object,object,object,object,object,object,object,object,object,object,object,object>), typeof(Func<object,object,object,object,object,object,object,object,object,object,object,object,object,object,object,object,object>) };

		public static readonly Type[] ObjectActionsWithParams = { typeof(Action), typeof(Action<object[]>), typeof(Action<object,object[]>), typeof(Action<object,object,object[]>), typeof(Action<object,object,object,object[]>), typeof(Action<object,object,object,object,object[]>), typeof(Action<object,object,object,object,object,object[]>), typeof(Action<object,object,object,object,object,object,object[]>), typeof(Action<object,object,object,object,object,object,object,object[]>), typeof(Action<object,object,object,object,object,object,object,object,object[]>), typeof(Action<object,object,object,object,object,object,object,object,object,object[]>), typeof(Action<object,object,object,object,object,object,object,object,object,object,object[]>), typeof(Action<object,object,object,object,object,object,object,object,object,object,object,object[]>), typeof(Action<object,object,object,object,object,object,object,object,object,object,object,object,object[]>), typeof(Action<object,object,object,object,object,object,object,object,object,object,object,object,object,object[]>), typeof(Action<object,object,object,object,object,object,object,object,object,object,object,object,object,object,object[]>), typeof(Action<object,object,object,object,object,object,object,object,object,object,object,object,object,object,object,object[]>) };

		public static readonly Type[] ObjectFunctionsWithParams = { typeof(Func<object>), typeof(Func<object[],object>), typeof(Func<object,object[],object>), typeof(Func<object,object,object[],object>), typeof(Func<object,object,object,object[],object>), typeof(Func<object,object,object,object,object[],object>), typeof(Func<object,object,object,object,object,object[],object>), typeof(Func<object,object,object,object,object,object,object[],object>), typeof(Func<object,object,object,object,object,object,object,object[],object>), typeof(Func<object,object,object,object,object,object,object,object,object[],object>), typeof(Func<object,object,object,object,object,object,object,object,object,object[],object>), typeof(Func<object,object,object,object,object,object,object,object,object,object,object[],object>), typeof(Func<object,object,object,object,object,object,object,object,object,object,object,object[],object>), typeof(Func<object,object,object,object,object,object,object,object,object,object,object,object,object[],object>), typeof(Func<object,object,object,object,object,object,object,object,object,object,object,object,object,object[],object>), typeof(Func<object,object,object,object,object,object,object,object,object,object,object,object,object,object,object[],object>), typeof(Func<object,object,object,object,object,object,object,object,object,object,object,object,object,object,object,object[],object>) };

		#endregion Generated Content
		
		#region Generation Code (Typescript)
/*
const indent1 = "\n\t\t";
const alphabet = 'abcdefghijklmnopqrstuvwxyz'.split("").slice(0, 16);

const enum GenericParamsText {
    Object = "object",
    Comma = ","
}

const makeActions = (text: GenericParamsText, params: boolean = false) => {
    const indices = Array.from(Array(alphabet.length + 1).keys());
    const nonGeneric = text === GenericParamsText.Object;
    return indices.map(i => {
        if (i === 0) return `typeof(Action)`;
        const elements = nonGeneric
            ? Array(i).fill(text)
            : Array(i - 1).fill(text);
        nonGeneric && params ? elements[elements.length - 1] = `object[]` : null;
        return `typeof(Action<${nonGeneric ? elements.join(",") : elements.join("")}>)`;
    }).join(", ");
}

const makeFuncs = (text: GenericParamsText, params: boolean = false) => {
    const indices = Array.from(Array(alphabet.length + 1).keys());
    const nonGeneric = text === GenericParamsText.Object;
    return indices.map(i => {
        const elements = nonGeneric
            ? Array(i + 1).fill(text)
            : Array(i).fill(text);
        nonGeneric && i > 0 && params ? elements[elements.length - 2] = `object[]` : null;
        return `typeof(Func<${nonGeneric ? elements.join(",") : elements.join("")}>)`
    }).join(", ");
}

const declaration = "public static readonly Type[]";
const genericActionsDeclaration = `${declaration} GenericActions = { ${makeActions(GenericParamsText.Comma)} };`
const genericFuncsDeclaration = `${declaration}  GenericFunctions = { ${makeFuncs(GenericParamsText.Comma)} };`

const nonGenericActionsDeclaration = `${declaration}  ObjectActions = { ${makeActions(GenericParamsText.Object)} };`;
const nonGenericFuncsDeclaration = `${declaration}  ObjectFunctions = { ${makeFuncs(GenericParamsText.Object)} };`;

const nonGenericActionsWithParamsDeclaration = `${declaration}  ObjectActionsWithParams = { ${makeActions(GenericParamsText.Object, true)} };`;
const nonGenericFuncsWithParamsDeclaration = `${declaration}  ObjectFunctionsWithParams = { ${makeFuncs(GenericParamsText.Object, true)} };`;

const content = `${indent1}#region Generated Content
${indent1}public static int Count => ${alphabet.length + 1};
${indent1}${genericActionsDeclaration}
${indent1}${genericFuncsDeclaration}
${indent1}${nonGenericActionsDeclaration}
${indent1}${nonGenericFuncsDeclaration}
${indent1}${nonGenericActionsWithParamsDeclaration}
${indent1}${nonGenericFuncsWithParamsDeclaration}
${indent1}#endregion Generated Content
`;

console.log(content);
*/
		#endregion Generation Code (Typescript)
	}
}