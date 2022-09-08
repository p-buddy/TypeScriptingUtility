namespace pbuddy.TypeScriptingUtility.RuntimeScripts
{
    public interface IClrToTsNameMapper
    {
        ClrToTsNameMapper.Delegate ToTs { get; }
        ClrToTsNameMapper.Delegate ToClr { get; }
    }
}