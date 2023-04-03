// See https://aka.ms/new-console-template for more information

using RayWork.SelfUpdater;

var website = "https://swcreeperking.github.io/mySite/rayWorksTesting/rayworkUpdaterTest.json";

Console.WriteLine("UPDATER");

Updater.CheckIfUpdateFinished();
if (Updater.IsPlatformSupported && Updater.IsVersionJsonLinkOutdated("1.0", website, out var downloadLink))
{
    Console.WriteLine("Program is READY up to date!");
    Console.WriteLine("press enter to start the update!");
    Console.ReadLine();

    Updater.Initialize(downloadLink);
    Updater.RunUpdater();
    Environment.Exit(0);
}
else
{
    Console.WriteLine("Program is up to date!");
    Console.WriteLine("press enter to continue . . . ");
    Console.ReadLine();
}