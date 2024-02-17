.assembly extern mscorlib {}
.assembly OddOrEven {}
.module OddOrEven2.exe

.namespace Odd.or
{
    .class public auto ansi Even extends [mscorlib]System.Object
    {
        .field public static int32 val

        .method public static void Check() cil managed
        {
            .entrypoint
            .locals init (int32 Retval)
   
        AskForNumber:
            ldstr "Enter a number"
            call void [mscorlib]System.Console::WriteLine(string)
            call string [mscorlib]System.Console::ReadLine()
            ldstr "%d"
            ldsflda int32 Odd.or.Even::val
            call vararg int32 sscanf(string, string, ..., int32*)
            // IL为列表上0-3的局部变量定义了特殊的操作码
            stloc.0
            ldloc.0
            brfalse.s Error
            ldsfld int32 Odd.or.Even::val
            // IL为整数的0-8定义了特殊的操作码
            ldc.i4.1
            and
            brfalse.s ItsEven
            ldstr "Odd"
            br.s PrintAndReturn
        ItsEven:
            ldstr "Even"
            br.s PrintAndReturn
        Error:
            ldstr "How rude"
        PrintAndReturn:
            call void [mscorlib]System.Console::WriteLine(string)
            ldloc.0
            brtrue.s AskForNumber
            ret
        }
    }
}

.method public static pinvokeimpl("msvcrt.dll" cdecl) vararg int32 sscanf(string, string) cil managed
{
}