using System;
using System.Linq;

public class InputTextChecker
{
    public static bool CheckTextEmpty(string inputText, uint warnstring)
    {
        if (string.IsNullOrEmpty(inputText))
        {
            TipsWindow.ShowWindow(warnstring);
            return true;
        }
        return false;
    }

    public static bool CheckTextHasSpecialChar(string inputText, string exceptstr, uint warnstring)
    {
        char ch1 = inputText[0];
        for (int index = 0; index < inputText.Length; ++index)
        {
            char ch2 = inputText[index];
            if (InputTextChecker.CHSymbol.Contains<char>(ch2) && !exceptstr.Contains<char>(ch2))
            {
                TipsWindow.ShowWindow(warnstring);
                return true;
            }
        }
        return false;
    }

    public static bool CheckTextHasSpecialChar(string inputText, uint warnstring)
    {
        char ch1 = inputText[0];
        for (int index = 0; index < inputText.Length; ++index)
        {
            char ch2 = inputText[index];
            if (InputTextChecker.CHSymbol.Contains<char>(ch2))
            {
                TipsWindow.ShowWindow(warnstring);
                return true;
            }
        }
        return false;
    }

    public static bool CheckTextIsAllNum(string inputText, uint warnstring)
    {
        char ch = inputText[0];
        for (int index = 0; index < inputText.Length; ++index)
        {
            switch (inputText[index])
            {
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    continue;
                default:
                    return false;
            }
        }
        TipsWindow.ShowWindow(warnstring);
        return true;
    }

    public static bool CheckTextHasBlank(string inputText, uint warnstring)
    {
        if (inputText.Contains(" "))
        {
            TipsWindow.ShowWindow(warnstring);
            return true;
        }
        return false;
    }

    public static bool CheckTextLength(string inputText, int MaxLength, int MinLength, uint warnstring)
    {
        int num = 0;
        foreach (char c in inputText)
        {
            if (c >= 'a' && c <= 'z')
            {
                num++;
            }
            else if (c >= 'A' && c <= 'Z')
            {
                num++;
            }
            else if (c >= '0' && c <= '9')
            {
                num++;
            }
            else
            {
                num += 2;
            }
        }
        bool flag = num < MinLength || num > MaxLength;
        if (flag)
        {
            TipsWindow.ShowWindow(warnstring);
        }
        return flag;
    }

    public static bool CheckNumOrEngTextLength(string inputText, int MaxLength, int MinLength, uint warnstring)
    {
        int num = 0;
        foreach (char c in inputText)
        {
            if (c >= 'a' && c <= 'z')
            {
                num++;
            }
            else if (c >= 'A' && c <= 'Z')
            {
                num++;
            }
            else
            {
                if (c < '0' || c > '9')
                {
                    return true;
                }
                num++;
            }
        }
        bool flag = num < MinLength || num > MaxLength;
        if (flag)
        {
            TipsWindow.ShowWindow(warnstring);
        }
        return flag;
    }

    public static bool CheckTextHasDirtyWord(string inputText, uint warnstring)
    {
        KeyWordFilter.InitFilter();
        if (string.Compare(inputText, KeyWordFilter.TextFilter(inputText)) != 0)
        {
            TipsWindow.ShowWindow(warnstring);
            return true;
        }
        return false;
    }

    private static string CHSymbol = "⊙⊕＠＃＆〓＼㊣℡﹋﹌☆★○●◎◇◆□■▓△▲▼▽◢◣◤◥⊿※§··々‖＄￡¤￠♂♀≈≡∷∮∑∈⊥‖∠⌒⊙≌∽√≈≤≥ΘΞ∏ΦΩξ≈≡≠＝≤≥＜＞≮≯∷±＋－×÷／∫∵∴∠⌒⊙≌∽√）§№☆★○●◎◇◆□■△▲※→←↑↓〓＃＆＠＼＾＿※☆★△▲ⅠⅡⅢⅣⅤⅥⅦⅧⅨⅩⅪⅫ⒈⒉⒊⒋⒌⒍⒎⒏⒐⒑⑴⑵⑶⑷⑸⑹⑺⑻⑼⑽⒒⒓⒔⒕⒖⒗⒘⒙⒚⒛①②③④⑤⑥⑦⑧⑨⑩⑾⑿⒀⒁⒂⒃⒄⒅⒆⒇≈≡≠＝≤≥＜＞≮≯∷±＋－×÷／∫∮∝∞∧∨∑∏∪∩∈∵∴⊥‖∠⌒⊙≌∽√°′〃＄￡￥‰％℃¤￠○┌┍┎┏┐┑┒┓—┄┈├┝┞┟┠┡┢┣│┆┊┬┭┮┯┰┱┲┳┼┽┾┿╀╁╂╃§№☆★○●◎◇◆□■△▲※→←↑↓〓＃＆＠＼＾＿＆＠＼＾абвгдеёжзийклмнопрстуфхцчшщъыьэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯαβγδεζηθικλμνξοπρστυφχψωΑΒΓΔΕΖΗΘΙΚ∧ΜΝΞΟ∏Ρ∑ΤΥΦΧΨΩㄅㄉㄓㄚㄞㄢㄦㄆㄊㄍㄐㄔㄗㄧㄛㄟㄣㄇㄋㄎㄑㄕㄘㄨㄜㄠㄤㄈㄏㄒㄖㄙㄩㄝㄡㄥāáǎàōóǒòêēéěèīíǐìūúǔùǖǘǚǜü¤§°¨±·×÷ˇˉˊˋ˙ΓΔΘΞ∏∑ΥΦΨΩαβγδεζηθικλμνξπρστυφψωЁБГДЕЖЗИЙКЛФУЦЧШЩЪЫЭЮЯабвгджзийклфцчшщъыюяё―‖¨…‰′〃※℃℅℉№℡ⅠⅡⅢⅣⅤⅥⅦⅧⅨⅩⅪⅫⅰⅱⅲⅳⅴⅵⅶⅷⅸⅹ←↑→↓↖↗↘↙∈∏∑∕√∝∞∟∠∣‖∧∨∩∪∫∮∴∵∶∷∽≈≌≈≠≡≤≥≤≥≮≯⊕⊙⊥⊿⌒①②③④⑤⑥⑦⑧⑨⑩⑴⑵⑶⑷⑸⑹⑺⑻⑼⑽⑾⑿⒀⒁⒂⒃⒄⒅⒆⒇—━│┃┄┅┆┇┈┉┊┋┌┍┎┏┐┑┒┓└┕┖┗┘┙┚┛├┝┞┟┠┡┢┣┤┥┦┧┨┩┪┫┬┭┮┯┰┱┲┳┴┵┶┷┸┹┺┻┼┽┾┿╀╁╂╃╄╅╆╇╈╉╊╋═║╒╓╔╕╖╗╘╙╚╛╜╝╞╟╠╡╢╣╤╥╦╧╨╩╪╫╬╭╮╯╰╱╲╳▁▂▃▄▆▇█▉▊▋▌▍▎▏▓▔▕■□▲△▼▽◆◇○◎●◢◣◤◥★☆⊙♀♂々〆〇「」『』【】〒〓〖〗〡〢〣〤〥〦〧〨〩㈱㊣㎎㎏㎜㎝㎞㎡㏄㏎㏑㏒㏕∶｜｜︴（）｛｝〔〕【】《》＾〉「」『』﹉﹊﹋﹌﹍﹎╬▁▂▃▄▆▇█▇▆▄▃▂▁╭╮╰╯▓【】▲╱╲△▼▽◆◇○◎●◢◣◤◥★☆⊙♀♂〇》「」『』〒〓〖〗【】《》¤╳↑↓⊙■◣◥Ψ※→№←㊣∑⌒＠⌒ξζω□∮〓∏卐√∩々∞①ㄨ≡↘↙⊙●○①⊕◎Θ⊙¤㊣★☆♀◆◇◣◢◥▲▼△▽⊿◤◥▂▃▄▆▇██■▓回□〓≡╝╚╔╗╬═╓╩┠┨┯┷┏┓┗┛┳⊥『』┌┐└┘∟「」↑↓→←↘↙♀♂┇┅﹉﹊﹍﹎╭╮╰╯（∵∴‖｜｜︴﹏﹋﹌（）〔〕【】〖〗＠：！·。≈～『』√※卐々∞Ψ∪∩∈∏の℡ぁ§∮＂″ミ灬ξ№∑⌒ξζω＊ㄨ≮≯＋－×÷＋－±／＝∫∮∝∞∧∨∑∏‖∠≌∽≤≥≈＜＞じ☆↑↓⊙●★☆■♀『』◆◣◥▲Ψ※◤◥→№←㊣∑⌒〖〗＠ξζω□∮〓※∴ぷ▂▃▆█∏卐【】△√∩¤々♀♂∞①ㄨ≡↘↙▂▂▃▄▆▇█┗┛╰☆╮じ╰☆╮~！@#￥%……&*（）——+·-={}|：“”《》？【】、；‘'，。、✪";
}
