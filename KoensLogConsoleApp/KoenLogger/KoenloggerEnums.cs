
internal enum LogLevel
{
    Trace = 0,
    Debug = 1, 
    Information = 2,
    Warning = 3,
    Error = 4,
    Critical = 5
}

internal enum OutputTarget 
{
    Screen ,
    TextFile ,
    Email ,
    Database ,
    ExcelFile,
    EventLog ,
    Azure ,
    Custom
}

internal enum OutputFormat
{
    PlainText ,
    Json ,
    Xml ,
    Csv ,
    Yaml ,
    Custom
}
