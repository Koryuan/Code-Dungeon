public static class StringList
{
    // Dialog String List
    public const string DialogItemCode = "<Item>";
    public const string DialogHelpCode = "<Help>";

    // Compiler
    public const string Pseudocode_Print = "PRINT";
    public const string Pseudocode_Scan = "INPUT";

    // HTML texting
    public static string Color_LightGreen = "90EE90";
    public static string Color_Red = "e91c1c";
    public static string Color_Grey = "808080";
    public const string HTML_Underline_Front = "<u color=#FFF500>";
    public static string ColorString(string NewString, string Color) => $"<color=#{Color}>{NewString}</color>";
    public static string ColorStringNoBack(string NewString, string Color) => $"<color=#{Color}>{NewString}";
}