using KoensLogConsoleApp.KLogger;
using KoensLogConsoleApp.Models;

string input1, input2;
int number1, number2;
var result = "";    


Console.WriteLine("Calculate Number1/Number2.");
Console.Write("Number1:");
input1 = Console.ReadLine();
Console.Write("Number2:");
input2 = Console.ReadLine();

IKoenLog kLog = new KoenLog();

try
{
    kLog.OutputTarget = OutputTarget.File; 
    bool input1IsNumeric = int.TryParse(input1, out number1);
    
    if (!input1IsNumeric)
    {
        result = "Input1 is not a valid number.";
        kLog.Log(result, OutputType.Error);
    }

    bool input2IsNumeric = int.TryParse(input2, out number2);

    if (!input2IsNumeric) {
        result = "Input2 is not a valid number.";
        kLog.Log(result, OutputType.Error);
    }

    if (input1IsNumeric && input2IsNumeric)
    {
        kLog.Log("Performing division...", OutputType.Info);
        result = number1 + "/" + number2 + " = " + (double)number1 / number2;
        kLog.Log(result, OutputType.Info);
    }
   
    Console.WriteLine(result);

}
catch (Exception oEx)
{
    kLog.Log(oEx.Message,OutputType.Error);
}

Console.WriteLine("Press Enter to stop program.");
Console.ReadLine();

