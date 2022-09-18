using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using pbuddy.TypeScriptingUtility.RuntimeScripts;

namespace pbuddy.TypeScriptingUtility.EditorScripts
{
    public readonly struct TsFunction
    {
        public string Declaration { get; }

        public TsFunction(IShared shared, Dictionary<Type, TsDeclaration> declarations)
        {
            MethodInfo method = (shared.NonSpecificClrObject as MulticastDelegate)?.Method ?? throw new Exception("");
            ParameterInfo[] parameters = method.GetParameters();
            ParameterInfo @return = method.ReturnParameter;

            string name = shared.TsType.Name;
            string argsText = String.Join(", ", parameters.Select(GetParameterName));
            string paramsText = String.Join(", ", parameters.Select(GetParameterDeclaration));
            
            Declaration = @$"
export const {name} = ({paramsText}): {TsParam(@return)} => {{
    {TsGenerator.TsIgnore}
    return {TsType.Internalize(name)}({argsText});
}};";

            string TsParam(ParameterInfo param) => declarations[param.ParameterType].Reference;
            static string GetParameterName(ParameterInfo param) => param.Name;
            string GetParameterDeclaration(ParameterInfo param) => $"{param.Name}: {TsParam(param)}";
        }
    }
}