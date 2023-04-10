namespace Liyanjie.Pinyin;

public class PinyinJiebaHelper
{
    public static IReadOnlyDictionary<char, string> Phonetics = new Dictionary<char, string>
    {
        ['a'] = "āáǎà",
        ['c'] = "ćč",
        ['e'] = "ēéěè",
        ['i'] = "īíǐì",
        ['m'] = "ḿm̀",
        ['n'] = "ńňǹ",
        ['o'] = "ōóǒò",
        ['s'] = "ŕř",
        ['s'] = "śš",
        ['u'] = "ūúǔù",
        ['v'] = "üǖǘǚǜ",
        ['y'] = "ý",
        ['z'] = "źž",
    };

    public static string[] GetPinyins(string input)
    {
        var segmenter = new JiebaSegmenter();
        var segments = segmenter.Cut(input);

        return PinyinHelper.GetPinyin(segments);
    }
    public static string GetPinyinInitials(string input)
    {
        var initials = GetPinyins(input)
            .Select(_ => _[0])
            .Select(_ =>
            {
                foreach (var phonetic in Phonetics)
                {
                    if (phonetic.Value.Contains(_))
                        return phonetic.Key;
                }
                return _;
            });
        return string.Join(string.Empty, initials);
    }
}
