#if NETSTANDARD2_0
namespace Liyanjie.ValueObjects;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class DisallowNullAttribute : Attribute{}
#endif