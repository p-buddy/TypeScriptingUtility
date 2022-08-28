const indent1 = "\n\t\t";
const alphabet = 'abcdefghijklmnopqrstuvwxyz'.split("").slice(0, 16);

const makeSignature = (opener: string, params: string, invocations: string) =>
    `${opener}(${params}) => wrappedDelegate.DynamicInvoke(${invocations});`;

const generateMethod = (opener: string) => {
    const withParams = alphabet.map((_, index) => {
        const all: string[] = [...alphabet].splice(0, index + 1);
        const params = all.map(l => `object ${l}`).join(", ");
        const invocations = all.map((l, i) => `${l}.As(parameterTypes[${i}])`).join(", ");
        return makeSignature(opener, params, invocations);
    });
    return [makeSignature(opener, "", ""), ...withParams].join(indent1);
};

const actionMethod = 'public void ActionCastAndInvoke';
const funcMethod = 'public object FuncCastAndInvoke';

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

const genericActionsDeclaration = `private static readonly Type[] GenericActionTypes = { ${makeActions(GenericParamsText.Comma)} };`
const genericFuncDeclaration = `private static readonly Type[] GenericFuncTypes = { ${makeFuncs(GenericParamsText.Comma)} };`

const nonGenericActionsDeclaration = `private static readonly Type[] NonSpecificActionTypes = { ${makeActions(GenericParamsText.Object)} };`;
const nonGenericFuncDeclaration = `private static readonly Type[] NonSpecificFuncTypes = { ${makeFuncs(GenericParamsText.Object)} };`;

const content = `
${indent1}#region Generated Content
${indent1}${genericActionsDeclaration}
${indent1}${genericFuncDeclaration}
${indent1}${nonGenericActionsDeclaration}
${indent1}${nonGenericFuncDeclaration}
${indent1}#region Actions
${indent1}${generateMethod(actionMethod)}
${indent1}#endregion Actions
${indent1}#region Funcs
${indent1}${generateMethod(funcMethod)}
${indent1}#endregion Funcs
${indent1}#endregion Generated Content
`;

console.log(content);