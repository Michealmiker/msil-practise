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
            // 异常捕获
            .try
            {
                call string [mscorlib]System.Console::ReadLine()
                ldstr "%d"
                ldsflda int32 Odd.or.Even::val
                call vararg int32 sscanf(string, string, ..., int32*)
                stloc.0
                // 离开当前块，切换到DidntBlowUp标签处
                // leave指令在切换计算流之前会清空堆栈
                leave.s DidntBlowUp
            }
            catch [mscorlib]System.Exception
            {
                pop
                ldstr "KABOOM"
                call void [mscorlib]System.Console::WriteLine(string)
                leave.s Return
            }
        DidntBlowUp:
            ldloc.0
            brfalse.s Error
            ldsfld int32 Odd.or.Even::val
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
        Return:
            ret
        }
    }
}

.method public static pinvokeimpl("msvcrt.dll" cdecl) vararg int32 sscanf(string, string) cil managed
{
}