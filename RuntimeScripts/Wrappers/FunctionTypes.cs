using System;

namespace pbuddy.TypeScriptingUtility.RuntimeScripts
{
	public static class FunctionTypes
	{
		#region Generated Content

		public static int Count => 17;

		public static readonly Type[] GenericActions =
		{
			typeof(Action), typeof(Action<>), typeof(Action<,>), typeof(Action<,,>), typeof(Action<,,,>),
			typeof(Action<,,,,>), typeof(Action<,,,,,>), typeof(Action<,,,,,,>), typeof(Action<,,,,,,,>),
			typeof(Action<,,,,,,,,>), typeof(Action<,,,,,,,,,>), typeof(Action<,,,,,,,,,,>),
			typeof(Action<,,,,,,,,,,,>), typeof(Action<,,,,,,,,,,,,>), typeof(Action<,,,,,,,,,,,,,>),
			typeof(Action<,,,,,,,,,,,,,,>), typeof(Action<,,,,,,,,,,,,,,,>)
		};

		public static readonly Type[] GenericFunctions =
		{
			typeof(Func<>), typeof(Func<,>), typeof(Func<,,>), typeof(Func<,,,>), typeof(Func<,,,,>),
			typeof(Func<,,,,,>), typeof(Func<,,,,,,>), typeof(Func<,,,,,,,>), typeof(Func<,,,,,,,,>),
			typeof(Func<,,,,,,,,,>), typeof(Func<,,,,,,,,,,>), typeof(Func<,,,,,,,,,,,>), typeof(Func<,,,,,,,,,,,,>),
			typeof(Func<,,,,,,,,,,,,,>), typeof(Func<,,,,,,,,,,,,,,>), typeof(Func<,,,,,,,,,,,,,,,>),
			typeof(Func<,,,,,,,,,,,,,,,,>)
		};

		public static readonly Type[] ObjectActions =
		{
			typeof(Action), typeof(Action<object>), typeof(Action<object, object>),
			typeof(Action<object, object, object>), typeof(Action<object, object, object, object>),
			typeof(Action<object, object, object, object, object>),
			typeof(Action<object, object, object, object, object, object>),
			typeof(Action<object, object, object, object, object, object, object>),
			typeof(Action<object, object, object, object, object, object, object, object>),
			typeof(Action<object, object, object, object, object, object, object, object, object>),
			typeof(Action<object, object, object, object, object, object, object, object, object, object>),
			typeof(Action<object, object, object, object, object, object, object, object, object, object, object>),
			typeof(Action<object, object, object, object, object, object, object, object, object, object, object,
				object>),
			typeof(Action<object, object, object, object, object, object, object, object, object, object, object, object
			  , object>),
			typeof(Action<object, object, object, object, object, object, object, object, object, object, object, object
			  , object, object>),
			typeof(Action<object, object, object, object, object, object, object, object, object, object, object, object
			  , object, object, object>),
			typeof(Action<object, object, object, object, object, object, object, object, object, object, object, object
			  , object, object, object, object>)
		};

		public static readonly Type[] ObjectFunctions =
		{
			typeof(Func<object>), typeof(Func<object, object>), typeof(Func<object, object, object>),
			typeof(Func<object, object, object, object>), typeof(Func<object, object, object, object, object>),
			typeof(Func<object, object, object, object, object, object>),
			typeof(Func<object, object, object, object, object, object, object>),
			typeof(Func<object, object, object, object, object, object, object, object>),
			typeof(Func<object, object, object, object, object, object, object, object, object>),
			typeof(Func<object, object, object, object, object, object, object, object, object, object>),
			typeof(Func<object, object, object, object, object, object, object, object, object, object, object>),
			typeof(Func<object, object, object, object, object, object, object, object, object, object, object,
				object>),
			typeof(Func<object, object, object, object, object, object, object, object, object, object, object, object,
				object>),
			typeof(Func<object, object, object, object, object, object, object, object, object, object, object, object,
				object, object>),
			typeof(Func<object, object, object, object, object, object, object, object, object, object, object, object,
				object, object, object>),
			typeof(Func<object, object, object, object, object, object, object, object, object, object, object, object,
				object, object, object, object>),
			typeof(Func<object, object, object, object, object, object, object, object, object, object, object, object,
				object, object, object, object, object>)
		};

		#endregion Generated Content
		
		#region Generation Code (Typescript)
/*
const indent1 = "\n\t\t";
const alphabet = 'abcdefghijklmnopqrstuvwxyz'.split("").slice(0, 16);

const enum GenericParamsText {
    Object = "object",
    Comma = ","
}

const makeActions = (text: GenericParamsText) => {
    const indices = Array.from(Array(alphabet.length + 1).keys());
    return indices.map(i => {
        const elements = text === GenericParamsText.Object
            ? i === 0 ? '' : `<${Array(i).fill(text).join(",")}>`
            : i === 0 ? '' : `<${Array(i - 1).fill(text).join("")}>`;
        return ` typeof(Action${elements})`;
    }).join(", ");
}

const makeFuncs = (text: GenericParamsText) => {
    const indices = Array.from(Array(alphabet.length + 1).keys());
    return indices.map(i => {
        const elements = text === GenericParamsText.Object
            ? Array(i + 1).fill(text).join(",")
            : Array(i).fill(text).join("");
        return `typeof(Func<${elements}>)`
    }).join(", ");
}

const declaration = "public static readonly Type[]";
const genericActionsDeclaration = `${declaration} GenericActions = { ${makeActions(GenericParamsText.Comma)} };`
const genericFuncDeclaration = `${declaration}  GenericFunctions = { ${makeFuncs(GenericParamsText.Comma)} };`

const nonGenericActionsDeclaration = `${declaration}  ObjectActions = { ${makeActions(GenericParamsText.Object)} };`;
const nonGenericFuncDeclaration = `${declaration}  ObjectFunctions = { ${makeFuncs(GenericParamsText.Object)} };`;

const content = `
${indent1}#region Generated Content
${indent1}public static int Count => ${alphabet.length + 1};
${indent1}${genericActionsDeclaration}
${indent1}${genericFuncDeclaration}
${indent1}${nonGenericActionsDeclaration}
${indent1}${nonGenericFuncDeclaration}
${indent1}#endregion Generated Content
`;

console.log(content);
*/
		#endregion Generation Code (Typescript)
	}
}