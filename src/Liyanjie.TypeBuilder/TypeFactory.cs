namespace Liyanjie.TypeBuilder;

/// <summary>
/// 
/// </summary>
public static class TypeFactory
{
    static readonly ConcurrentDictionary<string, Type> generatedTypes = new();
    static readonly ModuleBuilder moduleBuilder = AssemblyBuilder
        .DefineDynamicAssembly(new AssemblyName("Liyanjie.DynamicTypes"), AssemblyBuilderAccess.Run)
        .DefineDynamicModule("Liyanjie.DynamicTypes");

    static readonly ConstructorInfo objectConstructor = typeof(object).GetTypeInfo().GetConstructor([])!;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="properties"></param>
    /// <returns></returns>
    public static Type CreateType(IDictionary<string, Type> properties)
    {
        var id = GetMD5(string.Join("|", properties.Select(_ => $"{_.Key}={_.Value.FullName}")));
        if (generatedTypes.TryGetValue(id, out var type))
            return type;

        var typeBuilder = moduleBuilder.DefineType($"Liyanjie.DynamicTypes.Dynamic_{Guid.NewGuid().ToString("N").ToUpper()}",
            TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.Sealed,
            typeof(DynamicBase));

        var fields = new Dictionary<string, FieldBuilder>(properties.Count);

        #region Define Properties

        foreach (var item in properties)
        {
            var field = typeBuilder.DefineField($"field_{item.Key}", item.Value, FieldAttributes.Private);
            fields[item.Key] = field;

            var property = typeBuilder.DefineProperty(item.Key, PropertyAttributes.None, item.Value, null);

            var getter = typeBuilder.DefineMethod(
                $"get_{item.Key}",
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                item.Value,
                []);
            var getterILGenerator = getter.GetILGenerator();
            getterILGenerator.Emit(OpCodes.Ldarg_0);
            getterILGenerator.Emit(OpCodes.Ldfld, field);
            getterILGenerator.Emit(OpCodes.Ret);
            property.SetGetMethod(getter);

            var setter = typeBuilder.DefineMethod(
                $"set_{item.Key}",
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName,
                null,
                [item.Value]);
            setter.DefineParameter(1, ParameterAttributes.In, "value");
            var setterILGenerator = setter.GetILGenerator();
            setterILGenerator.Emit(OpCodes.Ldarg_0);
            setterILGenerator.Emit(OpCodes.Ldarg_1);
            setterILGenerator.Emit(OpCodes.Stfld, field);
            setterILGenerator.Emit(OpCodes.Ret);
            property.SetSetMethod(setter);
        }

        #endregion

        #region Define Constructor

        var defaultConstructor = typeBuilder.DefineConstructor(
            MethodAttributes.Public | MethodAttributes.HideBySig,
            CallingConventions.HasThis,
            []);
        var defaultConstructorILGenerator = defaultConstructor.GetILGenerator();
        defaultConstructorILGenerator.Emit(OpCodes.Ldarg_0);
        defaultConstructorILGenerator.Emit(OpCodes.Call, objectConstructor);
        defaultConstructorILGenerator.Emit(OpCodes.Ret);

        var parametersConstructor = typeBuilder.DefineConstructor(
            MethodAttributes.Public | MethodAttributes.HideBySig,
            CallingConventions.HasThis,
            [.. properties.Values]);
        var parametersConstructorILGenerator = parametersConstructor.GetILGenerator();
        parametersConstructorILGenerator.Emit(OpCodes.Ldarg_0);
        parametersConstructorILGenerator.Emit(OpCodes.Call, objectConstructor);
        for (int i = 0; i < properties.Count; i++)
        {
            var item = properties.ElementAt(i);
            parametersConstructor.DefineParameter(i + 1, ParameterAttributes.None, item.Key);
            parametersConstructorILGenerator.Emit(OpCodes.Ldarg_0);

            if (i == 0)
                parametersConstructorILGenerator.Emit(OpCodes.Ldarg_1);
            else if (i == 1)
                parametersConstructorILGenerator.Emit(OpCodes.Ldarg_2);
            else if (i == 2)
                parametersConstructorILGenerator.Emit(OpCodes.Ldarg_3);
            else if (i < 255)
                parametersConstructorILGenerator.Emit(OpCodes.Ldarg_S, (byte)(i + 1));
            else
                parametersConstructorILGenerator.Emit(OpCodes.Ldarg, unchecked((short)(i + 1)));
            parametersConstructorILGenerator.Emit(OpCodes.Stfld, fields[item.Key]);
        }
        parametersConstructorILGenerator.Emit(OpCodes.Ret);

        #endregion

        type = typeBuilder.CreateTypeInfo()!.UnderlyingSystemType;

        generatedTypes.TryAdd(id, type);

        return type;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public static object CreateObject(Dictionary<string, object?> values)
    {
        var properties = values.ToDictionary(_ => _.Key, _ => _.Value is null ? typeof(object) : _.Value.GetType());
        var type = CreateType(properties);
        var instance = (DynamicBase)Activator.CreateInstance(type)!;
        foreach (var item in values)
        {
            if (item.Value is not null)
                instance.SetPropertyValue(item.Key, item.Value);
        }

        return instance;
    }

    static string GetMD5(string input)
    {
        using var md5 = MD5.Create();
        var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
        return BitConverter.ToString(hash);
    }
}
