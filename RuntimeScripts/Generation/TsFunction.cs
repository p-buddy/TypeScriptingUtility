using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using pbuddy.TypeScriptingUtility.RuntimeScripts;

namespace pbuddy.TypeScriptingUtility.RuntimeScripts
{
    public readonly struct TsFunction: ITsThing
    {
        public string Declaration { get; }
        public string Reference { get; }

        public TsFunction(IShared shared, in TypeReferenceMap referenceMap)
        {
            MethodInfo method = (shared.NonSpecificClrObject as MulticastDelegate)?.Method ?? throw new Exception("");
            ParameterInfo[] parameters = method.GetParameters();
            ParameterInfo @return = method.ReturnParameter;
            
            TypeReferenceMap localMap = referenceMap;

            string name = shared.TsType.Name;
            string argsText = String.Join(", ", parameters.Select(GetParameterName));
            string paramsText = String.Join(", ", parameters.Select(GetParameterDeclaration));
            
            Declaration = @$"
{TsGenerator.ExportConst} {name} = ({paramsText}): {TsParam(@return, in localMap)} => {{
    {TsGenerator.TsIgnore}
    return {TsType.Internalize(name)}({argsText});
}};";
            Reference = shared.TsType.Name;

            static string TsParam(ParameterInfo param, in TypeReferenceMap referenceMap)
            {
                return referenceMap.GetReference(param.ParameterType);
            }

            static string GetParameterName(ParameterInfo param) => param.Name;

            string GetParameterDeclaration(ParameterInfo param) => $"{param.Name}: {TsParam(param, localMap)}";
        }
    }
}