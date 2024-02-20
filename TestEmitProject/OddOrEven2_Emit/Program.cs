const string AssemblyName = "OddOrEven";
const string ModuleName = "OddOrEven.dll";
const string ClassName = "Even";
const string ExternMethodName = "sscanf";
const string StaticFieldName = "val";
const string MethodName = "Check";
const string DllName = "msvcrt.dll";

var typeInt = typeof(int);
var typeString = typeof(string);
var typeIntPointer = typeof(int*);

var methodWriteLine = typeof(Console)
    .GetMethods(BindingFlags.Public | BindingFlags.Static)
    .First(methodInfo =>
    {
        if (string.CompareOrdinal(methodInfo.Name, "WriteLine") != 0)
        {
            return false;
        }
        
        var parameters = methodInfo.GetParameters();

        if (parameters.Length != 1)
        {
            return false;
        }

        return parameters[0].ParameterType == typeString;
    });
var methodReadLine = typeof(Console)
    .GetMethod("ReadLine", BindingFlags.Public | BindingFlags.Static)!;

var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(
    new(AssemblyName),
    AssemblyBuilderAccess.RunAndCollect);
var moduleBuilder = assemblyBuilder.DefineDynamicModule(ModuleName);
var evenTypeBuilder = moduleBuilder.DefineType(
    ClassName,
    TypeAttributes.Public | TypeAttributes.AnsiClass);
var externMethod = evenTypeBuilder.DefinePInvokeMethod(
    ExternMethodName,
    DllName,
    MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.PinvokeImpl,
    CallingConventions.VarArgs,
    typeInt,
    [typeString, typeString],
    CallingConvention.Cdecl,
    CharSet.None);
externMethod.SetImplementationFlags(MethodImplAttributes.PreserveSig);
var staticFieldBuilder = evenTypeBuilder.DefineField(
    StaticFieldName,
    typeInt,
    FieldAttributes.Public | FieldAttributes.Static);
var checkMethodBuilder = evenTypeBuilder.DefineMethod(
    MethodName,
    MethodAttributes.Public | MethodAttributes.Static);
var ilGenerator = checkMethodBuilder.GetILGenerator();
var askForNumberLabel = ilGenerator.DefineLabel();
var itsEvenLabel = ilGenerator.DefineLabel();
var errorLabel = ilGenerator.DefineLabel();
var printAdReturnLabel = ilGenerator.DefineLabel();

ilGenerator.DeclareLocal(typeInt);

ilGenerator.MarkLabel(askForNumberLabel);
ilGenerator.EmitWriteLine("Enter a number");
ilGenerator.Emit(OpCodes.Call, methodReadLine);
ilGenerator.Emit(OpCodes.Ldstr, "%d");
ilGenerator.Emit(OpCodes.Ldsflda, staticFieldBuilder);
ilGenerator.EmitCall(OpCodes.Call, externMethod, [typeIntPointer]);
ilGenerator.Emit(OpCodes.Stloc_0);
ilGenerator.Emit(OpCodes.Ldloc_0);
ilGenerator.Emit(OpCodes.Brfalse_S, errorLabel);
ilGenerator.Emit(OpCodes.Ldsfld, staticFieldBuilder);
ilGenerator.Emit(OpCodes.Ldc_I4_1);
ilGenerator.Emit(OpCodes.And);
ilGenerator.Emit(OpCodes.Brfalse_S, itsEvenLabel);
ilGenerator.Emit(OpCodes.Ldstr, "Odd");
ilGenerator.Emit(OpCodes.Br_S, printAdReturnLabel);
// -----------------------------------------------------------------------
ilGenerator.MarkLabel(itsEvenLabel);
ilGenerator.Emit(OpCodes.Ldstr, "Even");
ilGenerator.Emit(OpCodes.Br_S, printAdReturnLabel);
// -----------------------------------------------------------------------
ilGenerator.MarkLabel(errorLabel);
ilGenerator.Emit(OpCodes.Ldstr, "How rude");
// -----------------------------------------------------------------------
ilGenerator.MarkLabel(printAdReturnLabel);
ilGenerator.Emit(OpCodes.Call, methodWriteLine);
ilGenerator.Emit(OpCodes.Ldloc_0);
ilGenerator.Emit(OpCodes.Brtrue_S, askForNumberLabel);
ilGenerator.Emit(OpCodes.Ret);

var typeEven = evenTypeBuilder.CreateType();

var checkMethod = typeEven.GetMethod("Check", BindingFlags.Public | BindingFlags.Static);

checkMethod?.Invoke(null, null);
