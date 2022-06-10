using System;
using System.Collections.Generic;
using System.Reflection;
using pbuddy.TypeScriptingUtility.RuntimeScripts;
using UnityEngine;
using UnityEngine.Assertions;

namespace pbuddy.TypeScriptingUtility.EditorScripts
{
    public static class TsGenerator
    {
        private const string ExportConst = "export const";
        private enum FunctionType
        {
            Func,
            Action,
            Invalid
        }
        private const string ActionTypeName = "Action";
        private const string GenericIndicator = "`";
        private const string FuncTypeName = "Func" + GenericIndicator;

        public static BindingFlags PermissiveFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static;
        
        public static string Content(API api)
        {
            TypescriptType[] types = api.RootTypes;
            foreach (TypescriptType type in types)
            {
                switch (type.Spec)
                {
                    case TypescriptType.Specification.Class:
                        break;
                    case TypescriptType.Specification.Function:
                        string name = type.ClrType.Name;
                        string[] paramNames = type.ParameterNames;
                        FunctionType functionType = name == ActionTypeName || name.StartsWith(ActionTypeName + GenericIndicator)
                            ? FunctionType.Action
                            : name.StartsWith(FuncTypeName + GenericIndicator)
                                ? FunctionType.Func
                                : FunctionType.Invalid;
                        Assert.IsFalse(functionType == FunctionType.Invalid);
                        switch (functionType)
                        {
                            case FunctionType.Action:
                                break;
                            case FunctionType.Func:
                                break;
                        }
                        break;
                    case TypescriptType.Specification.Interface:
                        break;
                }
            }
            
            return "";
        }

        private static string GenerateFunctionDefinition(TypescriptType function, FunctionType type)
        {
            Type[] genericArguments = function.ClrType.GetGenericArguments();
            int argLength = genericArguments.Length;
            string declaration = @$"
{ExportConst} {function.Name} = (TODO): {(type == FunctionType.Action ? "void" : genericArguments[argLength - 1].AsTs())} => {{
    // @ts-ignore
    internal_{function.Name}();
}};";
            return declaration;
        }
        

        private static string AsTs(this Type type)
        {
            // TODO
            return "";
        }
        
    }
}