namespace pbuddy.TypeScriptingUtility.RuntimeScripts
{
    public interface IAPI
    {
        IClrToTsNameMapper NameMapper { get; }

        ILink[] Links { get; }
    }
}