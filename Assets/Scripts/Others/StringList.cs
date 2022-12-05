public static class StringList
{
    public static string Inventory_PickItemText_Right = "Tekan arah panah bawah dan atas untuk memilih item";
    public static string Inventory_PickItemText_Left = "Tekan tombol Z pada keyboard untuk menggunakan item";
    public static string Inventory_YesNoText = "Apakah ingin menggunakan item ini?";

    // Dialog String List
    public static string DialogItemCode = "<Item>";

    //Print 1 Scene Saved Object
    public static string Item_Print1 = "Item Print 1";
    public static string CodeMachine1 = "Code Machine - 1";
    public static string CodeMachine1_Text_After = "PRINT(\"Hello World\")";
    public static string CodeMachine1_Text_Before = "PRINT//(\"Hello World\")";
    public static string CodeMachine2_Text_Before = "PRINT//(\"729\")";

    // Code Analog
    public static string CommentCode = "//";
    public static string PrintString = "PRINT";
    public static string HtmlCode = "<color=023020>";

    // Compiler
    public static string Code_Print_Start = "PRINT(\"";
    public static string Code_Print_End = "\")";

    // HTML texting
    public static string DarkGreenHex = "023020";
    public static string Color_LightGreen = "90EE90";
    public static string Color_Red = "e91c1c";
    public static string ColorString(string NewString, string Color) => $"<color=#{Color}>{NewString}</color>";
}