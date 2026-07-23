namespace GrayTempest.Cards.Pool;

public static class RomanNumeral
{
    private static readonly string[] Romans = ["I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X"];

    public static string ToRoman(int num)
    {
        if (num < 1 || num > Romans.Length)
            return num.ToString();
        return Romans[num - 1];
    }
}