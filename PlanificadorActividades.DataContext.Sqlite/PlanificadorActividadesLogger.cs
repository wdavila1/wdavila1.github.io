using System;
using System.IO;
using static System.Environment;

namespace PlanificadorActividades.EntityModels;

public class PlanificadorActividadesLogger
{
    public static void WriteLine(string message)
    {
        string path = Path.Combine(GetFolderPath(SpecialFolder.DesktopDirectory), "PlanificadorActividadesLog.txt");

        using StreamWriter textFile = File.AppendText(path);
        textFile.WriteLine(message);
    }
}
