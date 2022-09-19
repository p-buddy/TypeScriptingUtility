using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using pbuddy.TypeScriptingUtility.RuntimeScripts;

namespace pbuddy.TypeScriptingUtility.EditorScripts
{
    public readonly struct TsFunction: ITsThing
    {
        public string Declaration { get; }
        public string Reference { get; }

        public TsFunction(IShared shared, Dictionary<Type, ITsThing> typeMap)
        {
            MethodInfo method = (shared.NonSpecificClrObject as MulticastDelegate)?.Method ?? throw new Exception("");
            ParameterInfo[] parameters = method.GetParameters();
            ParameterInfo @return = method.ReturnParameter;

            string name = shared.TsType.Name;
            string argsText = String.Join(", ", parameters.Select(GetParameterName));
            string paramsText = String.Join(", ", parameters.Select(GetParameterDeclaration));
            
            Declaration = @$"
{TsGenerator.ExportConst} {name} = ({paramsText}): {TsParam(@return)} => {{
    {TsGenerator.TsIgnore}
    return {TsType.Internalize(name)}({argsText});
}};";
            Reference = shared.TsType.Name;

            string TsParam(ParameterInfo param) => typeMap[param.ParameterType].Reference;
            static string GetParameterName(ParameterInfo param) => param.Name;
            string GetParameterDeclaration(ParameterInfo param) => $"{param.Name}: {TsParam(param)}";
        }
    }
}