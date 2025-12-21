// See https://aka.ms/new-console-template for more information

using ChameRozAP.ServiceManager;

try
{
    ChameRozManager chameRoz = new ChameRozManager();

    chameRoz.Start();

    Console.ReadLine();

}
catch (Exception e)
{
    Console.WriteLine(e.ToString());
    Console.ReadLine();
}