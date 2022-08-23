using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using pbuddy.TypeScriptingUtility.RuntimeScripts;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace pbuddy.TypeScriptingUtility.EditorScripts
{
    public static class TsGenerator
    {
        private struct LinkSorter : IComparer<ILink>
        {
            public int Compare(ILink x, ILink y)
            {
                if (x.TsType.Spec == TsType.Specification.Class) return -1;
                if (y.TsType.Spec == TsType.Specification.Class) return 1;
                return 0;
            }
        }
        
        private const string ExportConst = "export const";
        private const string TsIgnore = "// @ts-ignore";

        public static BindingFlags PermissiveFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static;

        public static string Content(IAPI api)
        {
            Dictionary<Type, TsDeclaration> typeMap = new Dictionary<Type, TsDeclaration>();
            List<ILink> links = api.Links.ToList();
            links.Sort(new LinkSorter());
            StringBuilder builder = new StringBuilder(links.Count);
            foreach (ILink link in links)
            {
                string declaration = link.TsType.Match(() => GenerateVariableDeclaration(link, typeMap),
                                                       () => GenerateClassDeclaration(link, typeMap),
                                                       () => GenerateFunctionDeclaration(link, typeMap));

                if (declaration is null) continue;
                builder.Append(declaration);
            }

            foreach (KeyValuePair<Type, TsDeclaration> kvp in typeMap)
            {
                AddDependencies(kvp.Key, typeMap);
            }

            foreach (KeyValuePair<Type, TsDeclaration> kvp in typeMap.Where(kvp => kvp.Value.NeedsDeclaration))
            {
                //builder.Append(GenerateInterfaceOrEnumDeclaration());
            }
            
            return builder.ToString();
        }

        private static string GenerateFunctionDeclaration(ILink link, Dictionary<Type, TsDeclaration> typeMap)
        {
            MethodInfo method = (link.NonSpecificClrObject as MulticastDelegate)?.Method ?? throw new Exception("");
            List<ParameterInfo> parameters = method.GetParameters().ToList();
            ParameterInfo @return = method.ReturnParameter;

            string name = link.TsType.Name;
            string argsText = String.Join(", ", parameters.Select(GetParameterName));
            string paramsText = String.Join(", ", parameters.Select(GetParameterDeclaration));
            
            return @$"
{ExportConst} {name} = ({paramsText}): {TsParam(@return)} => {{
    {TsIgnore}
    return {TsType.Internalize(name)}({argsText});
}};";

            string TsParam(ParameterInfo param) => param.ParameterType.TsName(typeMap);
            string GetParameterName(ParameterInfo param) => param.Name;
            string GetParameterDeclaration(ParameterInfo param) => $"{param.Name}: {TsParam(param)}";
        }
        
        private static string GenerateVariableDeclaration(ILink link, Dictionary<Type, TsDeclaration> typeMap)
        {
            string name = link.TsType.Name;
            return $"{ExportConst} {name}: {link.ClrType.TsName(typeMap)} = {TsType.Internalize(name)};";
        }


        private static string GenerateClassDeclaration(ILink link, Dictionary<Type, TsDeclaration> typeMap)
        {
            // TODO
            return "";
        }
        
        private static void AddDependencies(this Type type, Dictionary<Type, TsDeclaration> typeMap)
        {
            
        }

        private static string TsName(this Type type, Dictionary<Type, TsDeclaration> typeMap)
        {
            if (Primitives.TryGetTsName(type, out string name))
            {
                return name;
            }
            
            if (typeMap.TryGetValue(type, out TsDeclaration declaration))
            {
                return declaration.Name;
            }

            if (TryGetTsName(type, out name))
            {
                typeMap[type] = new TsDeclaration(name);
                return name;
            }

            throw new Exception();
        }

        private static bool TryGetTsName(this Type type, out string name)
        {
            if (type.IsArray)
            {
                
            }
            else if (type.IsEnum)
            {
                // Needs Checking
                name = type.Name;
                return true;
            }
            else if (type.IsGenericType)
            {
                // TODO
                name = null;
                return false;
            }
            else
            {
                name = type.Name;
                return true;
            }

            name = null;
            return false;
        }
        
    }
}