using MDRed.Classes;
using System.Text;

namespace MDRed
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]


        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            //MDCartridge cart = new MDCartridge("Empty".PadRight(10));

            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());

            
            //cart.MDCartridgeOpen("Test.mdr");
            //cart.MDFileExport("C:\\Users\\Panagis\\source\\repos\\MDRed\\bin\\Debug\\cc-lib.c");

            //MDCartridge cart = new MDCartridge("Cart 1".PadRight(10));
            //cart.MDCartridgeAddFile("C:\\Users\\Panagis\\source\\repos\\MDRed\\bin\\Debug\\net8.0-windows\\guess.c");
            //cart.MDCartridgeAddFile("C:\\Users\\Panagis\\source\\repos\\MDRed\\bin\\Debug\\net8.0-windows\\hello.c");
            //cart.MDCartridgeAddFile("C:\\Users\\Panagis\\source\\repos\\MDRed\\bin\\Debug\\net8.0-windows\\real.c");
            //cart.MDCartridgeAddFile("C:\\Users\\Panagis\\source\\repos\\MDRed\\bin\\Debug\\net8.0-windows\\stdio.c");
            //cart.MDCartridgeAddFile("C:\\Users\\Panagis\\source\\repos\\MDRed\\bin\\Debug\\net8.0-windows\\stdio.h");
            //cart.MDCartridgeAddFile("C:\\Users\\Panagis\\source\\repos\\MDRed\\bin\\Debug\\net8.0-windows\\c2tap.c");
            //cart.MDCartridgeAddFile("C:\\Users\\Panagis\\source\\repos\\MDRed\\bin\\Debug\\net8.0-windows\\cc-lib.c");
            //cart.MDCartridgeAddFile("C:\\Users\\Panagis\\source\\repos\\MDRed\\bin\\Debug\\net8.0-windows\\example.c");
            //cart.MDCartridgeSave("HisoftC.mdr");



            //Application.Run(new BlockGridForm(cart));
            //cart.MDCartridgeSave("Test_out.mdr");
            /*
            To write binary data to a file:
            // Open a file stream with write access
            using (FileStream fs = new FileStream("data.bin", FileMode.Create))
            {
            // Create a binary writer with the file stream
            using (BinaryWriter bw = new BinaryWriter(fs))
            {
            // Write some data to the file
            bw.Write(42); // an integer
            bw.Write(3.14); // a double
            bw.Write("Hello World"); // a string
            }
            }
             
            To read binary data from a file:
            // Open a file stream with read access
            using (FileStream fs = new FileStream("data.bin", FileMode.Open))
            {
                // Create a binary reader with the file stream
                using (BinaryReader br = new BinaryReader(fs))
                {
                    // Read the data from the file
                    int myInt = br.ReadInt32();
                    double myDouble = br.ReadDouble();
                    string myString = br.ReadString();

                    // Use the data
                    Console.WriteLine("My int: " + myInt);
                    Console.WriteLine("My double: " + myDouble);
                    Console.WriteLine("My string: " + myString);
                }
            } 
            */

        }
    }
}