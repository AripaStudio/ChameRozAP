// See https://aka.ms/new-console-template for more information

using ChameRozAP.ServiceManager;

try
{
    
    ChameRozManager chameRoz = new ChameRozManager();

    chameRoz.Start();

    Console.ReadKey();

}
catch (Exception e)
{
    ShowMessageAP.ShowMessageBoxAP(e.Message , "debug");
    Console.ReadKey();
}
