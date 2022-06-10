namespace pbuddy.TypeScriptingUtility.RuntimeScripts
{
    public abstract class API
    {
        public void Apply()
        {
            TypescriptType[] types = Define();
        }

        public TypescriptType[] RootTypes => Define();

        public abstract TypescriptType[] Define();
    }
}