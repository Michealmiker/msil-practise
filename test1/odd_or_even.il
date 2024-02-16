// 程序集引用
// 加载mscorlib.dll
.assembly extern mscorlib {}
// 定义程序集
// 程序集是.net应用的基本构成单元
.assembly OddOrEven {}
// 定义模块
// 一个模块是一个托管文件，可以是exe，也可以是dll
// 一个或者多个模块构成一个程序集
.module OddOrEven.exe

// 定义名称空间
.namespace Odd.or
{
    // 定义类
    // public - 公共可见性
    // 可选项：public、private
    // auto - 自动类布局
    // 可选项：auto、sequential、explicit
    // ansi - 类中字符串转换的模式
    // 可选项：ansi、unicode、autochar
    // extends指定当前类的基类
    // [mscorlib] - 使用程序集引用作为解析作用域
    // class默认从[mscorlib]System.Object中派生而来
    // struct从[mscorlib]System.ValueType中派生而来，可以通过使用value关键字省略extends子句
    // enum从[mscorlib]System.Enum中派生而来，可以通过使用enum关键字省略extends子句
    .class public auto ansi Even extends [mscorlib]System.Object
    {
        // 定义字段
        // public - 公共可见性
        // 可选项：
        //  assembly - 可以在当前程序集范围内访问
        //  family - 可以从由当前类所派生的类中访问这个字段
        //  famandassem - 可以从当前程序集中定义的有当前类所派生的类中访问这个字段
        //  famorassem - 可以从当前程序集中的任何地方，以及由当前类所派生的类中访问这个字段，即便这个派生类在当前程序集以外被定义
        //  private - 仅当前类可以访问该字段
        //  privatescope - 默认设置，不建议使用这个标志
        // static - 静态成员
        .field public static int32 val
        
        // 定义方法
        // public - 公共可见性，与字段相同
        // static - 静态成员，与字段相同
        // void - 方法返回类型
        // cil managed - 标记方法使用IL表示
        // 可选项：cil managed、native unmanaged
        .method public static void Check() cil managed
        {
            // 指定当前方法为程序的入口点
            .entrypoint
            // 定义当前方法的局部变量
            .locals init (int32 Retval)
            
        // 跳转标签    
        AskForNumber:
            // 将字符串常量载入到堆栈中
            ldstr "Enter a number"
            // 调用库函数
            call void [mscorlib]System.Console::WriteLine(string)
            // 调用库函数
            call string [mscorlib]System.Console::ReadLine()
            // 将类型为valuetype CharArray8的静态字段Format的地址载入堆栈
            ldsflda valuetype CharArray8 Format
            // 将静态字段val的地址载入堆栈
            ldsflda int32 Odd.or.Even::val
            // 调用全局静态方法
            // vararg表示这个方法拥有可变参数列表
            call vararg int32 sscanf(string, int8*, ..., int32*)
            // 从堆栈中获取对sscanf的调用结果，并将其存储在局部变量Retval中
            stloc Retval
            // 将Retval的值复制到堆栈
            ldloc Retval
            // 从堆栈中获取一个值，如果它是0，则跳转到Error标签
            brfalse Error
            // 将静态字段val的值载入堆栈
            ldsfld int32 Odd.or.Even::val
            // 将常量1载入堆栈
            ldc.i4 1
            // 从堆栈中获取两项，并执行逻辑与运算，并将结果放回到堆栈
            and
            // 从堆栈中获取一个值，如果它是0，则跳转到ItsEven标签
            brfalse ItsEven
            // 将字符串常量载入到堆栈中
            ldstr "Odd"
            // 无条件跳转到PrintAndReturn标签
            br PrintAndReturn
        ItsEven:
            ldstr "Even"
            br PrintAndReturn
        Error:
            ldstr "How rude"
        PrintAndReturn:
            call void [mscorlib]System.Console::WriteLine(string)
            ldloc Retval
            brtrue AskForNumber
            ret
        }
    }
}

// 声明一个类型为valuetype CharArray8类型名为Format的静态字段
// 它是一个全局项，不属于任何特定的类，仅属于包含该声明的模块
.field public static valuetype CharArray8 Format at FormatData
// 定义一个名为FormatData的数据段
// bytearray - 字节数组，它是IL中最常用的描述数据的方法，除了用于.data外，IL指令中也可直接使用
// 如：ldstr bytearray(6F 00 64 00 64 00 21 00 00 00)
// 即：ldstr "odd!"
// 括号中的数字代表十六进制字节值
// 也可以定义为：.data FormatData = bytearray(0x0000000000006425)
.data FormatData = bytearray(25 64 00 00 00 00 00 00)
// 声明一个值类型CharArray8
.class public explicit CharArray8 extends [mscorlib]System.ValueType
{
    // 指定类的大小：8个字节
    .size 8
}
// 声明一个将要从托管代码中调用的非托管方法
// pinvoke - 即平台调用，用于指出这个方法是一个非托管方法
// cdecl - 调用约定，意味着非托管方法使用与ANSI C函数相同的方式处理参数
.method public static pinvokeimpl("msvcrt.dll" cdecl) vararg int32 sscanf(string, int8*) cil managed
{
}