namespace pbuddy.TypeScriptingUtility.RuntimeScripts
{
    public interface IAPI
    {
        IClrToTsNameMapper NameMapper { get; }

        IShared[] Shared { get; }
    }
}