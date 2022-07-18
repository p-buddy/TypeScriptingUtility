namespace pbuddy.TypeScriptingUtility.EditorScripts
{
    public struct TsDeclaration
    {
        public string Name { get; }
        public string Declaration { get; private set; }
        
        public bool NeedsDeclaration => Declaration is null;

        public TsDeclaration(string name, string declaration = null)
        {
            Name = name;
            Declaration = declaration;
        }

        public void SetDeclaration(string declaration)
        {
            Declaration = declaration;
        }
    }
}