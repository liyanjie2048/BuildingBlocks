using System;
using System.Collections.Generic;
using System.Linq;

namespace Liyanjie.EnglishPluralization.Internals;

internal static class EnglishPluralizationServiceUtil
{
    public static bool DoesWordContainSuffix(
        string word,
        IEnumerable<string> suffixes)
    {
        return suffixes.Any(s => word.EndsWith(s, StringComparison.OrdinalIgnoreCase));
    }

    public static bool TryGetMatchedSuffixForWord(
        string word,
        IEnumerable<string> suffixes,
        out string? matchedSuffix)
    {
        matchedSuffix = null;
        if (DoesWordContainSuffix(word, suffixes))
        {
            matchedSuffix = suffixes.First(s => word.EndsWith(s, StringComparison.OrdinalIgnoreCase));
            return true;
        }
        return false;
    }

    public static bool TryInflectOnSuffixInWord(
        string word,
        IEnumerable<string> suffixes,
        Func<string, string> operationOnWord,
        out string? newWord)
    {
        newWord = null;
        if (TryGetMatchedSuffixForWord(word, suffixes, out _))
        {
            newWord = operationOnWord(word);
            return true;
        }
        return false;
    }
}
