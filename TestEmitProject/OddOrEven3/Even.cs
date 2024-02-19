namespace Odd.or;

public class Even
{
    [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern int sscanf(string str, string format, __arglist);
    
    public static int val;

    public static unsafe void Check()
    {
        int Retval;

        do
        {
            try
            {
                Console.WriteLine("Enter a number");
                Retval = sscanf(Console.ReadLine(), "%d", __arglist((int*)Unsafe.AsPointer(ref val)));
            }
            catch (Exception e)
            {
                Console.WriteLine("KABOOM");
                break;
            }
            
            Console.WriteLine(Retval == 0 ? "How rude" : (val & 1) == 0 ? "Even" : "Odd");
        } while (Retval != 0);
    }
}