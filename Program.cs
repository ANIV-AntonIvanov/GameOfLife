using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


namespace gameoflife
{
    class Program
    {
        //{ '\u00FA', '\u00BA', '\u00B1', '\u00B2', '\u00DB', '\u00FE'}\\

        static void Main(string[] args)
        {
            var shapes = new Shapes
            {
            };

            var obj = new RowsColumns
            {
                maxC = 20,
                maxR = 20
            };

            var lD = new LifeDeath
            {
                InitialLive = 10,
                dieAtAge = 5
            };

            int[,] array = new int[obj.maxC, obj.maxR];
            int[,] tmp = new int[obj.maxC, obj.maxR];

            Input(lD.InitialLive, array, obj.maxR, obj.maxC);

            while (true)
            {
                Draw(shapes.shapes, array, obj.maxC, obj.maxR);

                Possibilities(obj.maxR, obj.maxC, array, tmp);

                LivingOrNot(obj.maxR, obj.maxC, array, tmp, lD.dieAtAge);
            }
        }

        static void Draw(char[] sh, int[,] arr, int maxCo,int maxRo)
        {
            //------------------------------show only found boms & tryes
            Console.Clear();
            Gotoxy(30, 0);
            Console.WriteLine("Enter Ctrl-C to stop the app");
            //------------------------------output
            Gotoxy(30, 2);
            Console.WriteLine("|012345678901234567890123456789012345678901234567890123456789");
            Gotoxy(30, 4);
            Console.WriteLine("--+------------------------------------------------------------");
            for (int i = 0; i < maxRo; i++)
            {
                Gotoxy(20, i);
                Console.WriteLine(" |" + i.ToString() + "  ");
                for (int j = 0; j < maxCo; j++)
                {
                    Gotoxy(i, j);
                    Console.WriteLine(sh[arr[i, j]]);
                }
                Console.WriteLine(" ");
            }
            Thread.Sleep(1000);
            Console.WriteLine(" ");
        }

        static void Gotoxy(int column, int line)
        {
            int coordX, coordY;
            coordX = column;
            coordY = line;
            Console.SetCursorPosition(coordX, coordY);
        }
        
        static void Possibilities(int maxRo, int maxCo, int[,] arr, int[,] temp)
        {
            for (int i = 0; i < maxRo; i++)
            {
                for (int j = 0; j < maxCo; j++)
                {
                    int neighbours = 0;
                    temp[i, j] = 0;

                    if (i - 1 >= 0 && j - 1 >= 0 && arr[i - 1, j - 1] != 0)
                        neighbours++;
                    if (i + 1 < maxRo && j + 1 < maxCo && arr[i + 1, j + 1] != 0)
                        neighbours++;
                    if (i - 1 >= 0 && j + 1 < maxCo && arr[i - 1, j + 1] != 0)
                        neighbours++;
                    if (i + 1 < maxRo && j - 1 >= 0 && arr[i + 1, j - 1] != 0)
                        neighbours++;
                    if (i - 1 >= 0 && arr[i - 1, j] != 0)
                        neighbours++;
                    if (i + 1 < maxRo && arr[i + 1, j] != 0)
                        neighbours++;
                    if (j - 1 >= 0 && arr[i, j - 1] != 0)
                        neighbours++;
                    if (j + 1 < maxCo && arr[i, j + 1] != 0)
                        neighbours++;

                    switch (neighbours)
                    {
                        case 0:
                        case 1:
                            temp[i, j] = 0;
                            break; // dead loneliness
                        case 2:
                        case 3:
                            temp[i, j] = 1;
                            break; //new live/preserve live
                        case 4:
                        default:
                            temp[i, j] = 0;
                            break; // dead overfull
                    }
                }
            }
        }
        static void Input(int init,int[,] arr,int maxRo, int maxCo)
        {
            for (int i = 0; i < maxRo; i++)
            {
                for (int j = 0; j < maxCo; j++)
                {
                    arr[i, j] = 0;
                }
            }

            Random rnd = new Random();

            for (int i = 0; i < init; i++)
            {
                bool ok = false;
                do
                {
                    int x = rnd.Next(0, 19) % (maxRo + 1);
                    int y = rnd.Next(0, 19) % (maxCo + 1);
                    if (arr[x, y] == 0)
                    {
                        arr[x, y] = 1;
                        ok = true;
                    }
                } while (!ok);
            }
        }
        static void LivingOrNot(int maxRo, int maxCo, int[,] arr, int[,]temp, int death)
        {
            for (int i = 0; i < maxRo; i++)
            {
                for (int j = 0; j < maxCo; j++)
                {
                    if (arr[i, j] != 0 && temp[i, j] == 1)
                    {
                        arr[i, j]++;
                        if (arr[i, j] >= death) arr[i, j] = 0;
                    }
                    else if (arr[i, j] == 0 && temp[i, j] == 1)
                    {
                        arr[i, j] = 1;
                    }
                    else if (arr[i, j] != 0 && temp[i, j] == 0)
                    {
                        arr[i, j]++;
                        if (arr[i, j] >= death) arr[i, j] = 0;
                    }
                }
            }
        }
    }
    public interface IBaseCap
    {
        int maxC { get; set; }
        int maxR { get; set; }
    }
    public interface IBaseLD
    {
        int InitialLive { get; set; }
        int dieAtAge { get; set; }
    }
    class RowsColumns : IBaseCap
    {
        private int maxRows;
        private int maxColumns;
        public int maxC
        {
            get => maxColumns;
            set => maxColumns = value;
        }

        public int maxR
        {
            get => maxRows;
            set => maxRows = value;
        }

    }
    class Shapes
    {
        public char[] shapes = new char[] { '\u00FA', '\u00BA', '\u00B1', '\u00B2', '\u00DB', '\u00FE' };
    }
    class LifeDeath : IBaseLD
    {
        private int init;
        private int death;

        public int InitialLive
        {
            get => init;
            set => init = value;
        }

        public int dieAtAge
        {
            get => death;
            set => death = value;
        }
    }
}
