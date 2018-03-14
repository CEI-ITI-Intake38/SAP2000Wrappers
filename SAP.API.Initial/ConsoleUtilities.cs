using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SAP.API.Initial
{
    public static class ConsoleUtilities
    {

        public static void Header(this string s, ConsoleColor forecolor, ConsoleColor backColor)
        {
            int x = 0;
            int y = 0;
            int txtSize = s.Count();
            int height = 10;
            //top
            for (int i = 0; i < Console.WindowWidth; i++)
            {
                Console.SetCursorPosition(x + i, y);
                Console.WriteLine("*");
            }
            //bottom
            for (int i = 0; i < Console.WindowWidth; i++)
            {
                Console.SetCursorPosition(x + i, y + height);
                Console.WriteLine("*");
            }
            //right
            for (int i = 0; i < height; i++)
            {
                Console.SetCursorPosition(x, y + i);
                Console.WriteLine("*");
            }
            //left
            for (int i = 0; i < height; i++)
            {
                Console.SetCursorPosition(x + Console.WindowWidth - 1, y + i);
                Console.WriteLine("*");
            }


            Console.ForegroundColor = forecolor;
            Console.BackgroundColor = backColor;
            Console.SetCursorPosition(Console.WindowWidth / 2 - txtSize / 2, y + height / 2);

            Console.WriteLine(s);
            Console.ResetColor();
            Console.SetCursorPosition(x, y + height + 1);
        }
        public static void Print(this string s, ConsoleColor foreColor)
        {
            Console.ForegroundColor = foreColor;
            Console.WriteLine(s);
            Console.ResetColor();
        }
        public static void PrintCenter(this string s, ConsoleColor foreColor = ConsoleColor.White)
        {
            Console.ForegroundColor = foreColor;
            Console.SetCursorPosition(Console.WindowWidth / 2 - s.Count() / 2, Console.CursorTop);
            Console.WriteLine(s);
            Console.ResetColor();
        }
        public static void PrintAtPosition(this string s, [Optional]int x, [Optional]int y, ConsoleColor foreColor)
        {
            if (y == 0 && x != 0)
            {
                Console.SetCursorPosition(x, Console.CursorTop);

            }
            else if (x == 0 && y != 0)
            {
                Console.SetCursorPosition(Console.CursorLeft, y);
            }
            else if (x != 0 && y != 0)
            {
                Console.SetCursorPosition(x, y);
            }
            Console.ForegroundColor = foreColor;
            Console.WriteLine(s);
            Console.ResetColor();
        }
       
    }
}
