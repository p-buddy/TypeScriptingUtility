namespace pbuddy.TypeScriptingUtility.RuntimeScripts
{
    /// <summary>
    /// Indicates a type that can be instantiated either as itself,
    /// or with only a single value, which will be set to the 'Value' property.
    /// This requires for all other fields in the type to default to appropriate values.
    /// This allows you to accomplish a behavior similar to the typescript equivalent:
    /// <code lang="ts">
    /// type X = number | { value: number, otherInfo: number };
    /// </code>
    /// <exception cref="">
    /// Important, do NOT have the Generic Type parameter also be an IValuable.
    /// That will prevent us from discriminating between the type and the value passed.
    /// </exception>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IValuable<out T> where T: new()
    {
        T Value { get; }
    }
}