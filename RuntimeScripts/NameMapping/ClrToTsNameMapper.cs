namespace pbuddy.TypeScriptingUtility.RuntimeScripts
{
    public static class ClrToTsNameMapper
    {
        public delegate string Delegate(string input);
        public static readonly IClrToTsNameMapper Default = new DefaultMapper();
        public static readonly IClrToTsNameMapper PascalToCamelCase = new PascalToCamelCaseMapper();

        private struct DefaultMapper: IClrToTsNameMapper
        {
            public Delegate ToTs => input => input;
            public Delegate ToClr => input => input;

        }

        private struct PascalToCamelCaseMapper: IClrToTsNameMapper
        {
            public Delegate ToTs => input => $"{char.ToLowerInvariant(input[0])}{input.Substring(1)}";
            public Delegate ToClr => input => $"{char.ToUpperInvariant(input[0])}{input.Substring(1)}";
        }
        
        public static string MapToTs(this IClrToTsNameMapper mapper, string input)
        {
            mapper ??= Default;
            return mapper.ToTs(input);
        }
        
        public static string MapToClr(this IClrToTsNameMapper mapper, string input)
        {
            mapper ??= Default;
            return mapper.ToClr(input);
        }
    }
}