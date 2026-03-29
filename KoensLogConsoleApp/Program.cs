using ConsoleLoggingApp.KoenLogger;

string input1, input2;
int number1, number2; //todo laat ook komma getallen toe
var result = "";    


Console.WriteLine("Calculate Number1/Number2.");
Console.Write("Number1:");
input1 = Console.ReadLine();
Console.Write("Number2:");
input2 = Console.ReadLine();

IKoenLog kLog = new KoenLog()
{
    OutputTarget = OutputTarget.File,
    Emailserver = "smtp.contoso.com",
    EmailFrom = "testFrom@contoso.com",
    EmailTo = "testTo@contoso.com",
    PathToLogFile = @"C:\Users\koenv\source\repos\KoensLogConsoleApp\KoensLogConsoleApp\logFiles\"
};

try
{
    kLog.DeleteOldLogFiles(2);
    bool input1IsNumeric = int.TryParse(input1, out number1);
    
    if (!input1IsNumeric)
    {
        result = $"Input1 = '{input1}' is not a valid number. Devision is not performed.";
        kLog.Log(result, OutputType.Warning);
    }

    bool input2IsNumeric = int.TryParse(input2, out number2);

    if (!input2IsNumeric) {
        result = $"Input2 = '{input2}' is not a valid number. Devision is not performed.";
        kLog.Log(result, OutputType.Warning);
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

