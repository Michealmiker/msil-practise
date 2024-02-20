const string AssemblyName = "OddOrEven";
const string ModuleName = "OddOrEven.dll";
const string ClassName = "Even";
const string ExternMethodName = "sscanf";
const string StaticFieldName = "val";
const string MethodName = "Check";
const string DllName = "msvcrt.dll";

var intType = typeof(int);
var stringType = typeof(string);
var intPointerType = typeof(int*);
var exceptionType = typeof(Exception);

var readLineMethod = typeof(Console)
    .GetMethod("ReadLine", BindingFlags.Public | BindingFlags.Static)!;
var writeLineMethod = typeof(Console)
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

        return parameters[0].ParameterType == stringType;
    });

var oddOrEvenAssembly = AssemblyBuilder.DefineDynamicAssembly(
    new(AssemblyName),
    AssemblyBuilderAccess.RunAndCollect);
var oddOrEvenModule = oddOrEvenAssembly.DefineDynamicModule(ModuleName);
var evenClass = oddOrEvenModule.DefineType(
    ClassName,
    TypeAttributes.Public | TypeAttributes.AnsiClass);
var externMethod = evenClass.DefinePInvokeMethod(
    ExternMethodName,
    DllName,
    MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.PinvokeImpl,
    CallingConventions.VarArgs,
    intType,
    [stringType, stringType],
    CallingConvention.Cdecl,
    CharSet.None);
externMethod.SetImplementationFlags(MethodImplAttributes.PreserveSig);
var val = evenClass.DefineField(
    StaticFieldName,
    intType,
    FieldAttributes.Public | FieldAttributes.Static);
var checkMethod = evenClass.DefineMethod(
    MethodName,
    MethodAttributes.Public | MethodAttributes.Static);
var il = checkMethod.GetILGenerator();

var askForNumberLabel = il.DefineLabel();
var didntBlowUpLabel = il.DefineLabel();
var itsEvenLabel = il.DefineLabel();
var errorLabel = il.DefineLabel();
var printAndReturnLabel = il.DefineLabel();
var returnLabel = il.DefineLabel();

il.DeclareLocal(intType);

il.MarkLabel(askForNumberLabel);
il.EmitWriteLine("Enter a number");
il.BeginExceptionBlock();
il.Emit(OpCodes.Call, readLineMethod);
il.Emit(OpCodes.Ldstr, "%d");
il.Emit(OpCodes.Ldsflda, val);
il.EmitCall(OpCodes.Call, externMethod, [intPointerType]);
il.Emit(OpCodes.Stloc_0);
il.Emit(OpCodes.Leave_S, didntBlowUpLabel);
il.BeginCatchBlock(exceptionType);
il.Emit(OpCodes.Pop);
il.EmitWriteLine("KABOOM");
il.Emit(OpCodes.Leave_S, returnLabel);
il.EndExceptionBlock();
// -----------------------------------------------------------------------
il.MarkLabel(didntBlowUpLabel);
il.Emit(OpCodes.Ldloc_0);
il.Emit(OpCodes.Brfalse_S, errorLabel);
il.Emit(OpCodes.Ldsfld, val);
il.Emit(OpCodes.Ldc_I4_1);
il.Emit(OpCodes.And);
il.Emit(OpCodes.Brfalse_S, itsEvenLabel);
il.Emit(OpCodes.Ldstr, "Odd");
il.Emit(OpCodes.Br_S, printAndReturnLabel);
// -----------------------------------------------------------------------
il.MarkLabel(itsEvenLabel);
il.Emit(OpCodes.Ldstr, "Even");
il.Emit(OpCodes.Br_S, printAndReturnLabel);
// -----------------------------------------------------------------------
il.MarkLabel(errorLabel);
il.Emit(OpCodes.Ldstr, "How rude");
// -----------------------------------------------------------------------
il.MarkLabel(printAndReturnLabel);
il.Emit(OpCodes.Call, writeLineMethod);
il.Emit(OpCodes.Ldloc_0);
il.Emit(OpCodes.Brtrue_S, askForNumberLabel);
// -----------------------------------------------------------------------
il.MarkLabel(returnLabel);
il.Emit(OpCodes.Ret);

var typeEven = evenClass.CreateType();

var method = typeEven.GetMethod("Check", BindingFlags.Public | BindingFlags.Static);

method?.Invoke(null, null);