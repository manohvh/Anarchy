using System.Collections.Generic;

namespace Discord
{
    public enum Language
    {
        Danish,
        German,
        EnglishUK,
        EnglishUS,
        Spanish,
        French,
        Croatian,
        Italian,
        Lithuanian,
        Hungarian,
        Dutch,
        Norwegian,
        Polish,
        Portuguese,
        Romanian,
        Finnish,
        Swedish,
        Viatnamese,
        Turkish,
        Czech,
        Greek,
        Bulgarian,
        Russian,
        Ukranian,
        Thai,
        Chinese,
        Japanese,
        ChineseThai,
        Korean
    }

    public static class LanguageUtils
    {
        private static readonly Dictionary<Language, string> Languages = new Dictionary<Language, string>()
        {
            { Language.Danish, "da" },
            { Language.German, "de" },
            { Language.EnglishUK, "en-GB" },
            { Language.EnglishUS, "en-US" },
            { Language.Spanish, "es-ES" },
            { Language.French, "fr" },
            { Language.Croatian, "hr" },
            { Language.Italian, "it" },
            { Language.Hungarian, "hu" },
            { Language.Dutch, "nl" },
            { Language.Norwegian, "no" },
            { Language.Polish, "pl" },
            { Language.Portuguese, "pt-BR" },
            { Language.Romanian, "ro" },
            { Language.Finnish, "fi" },
            { Language.Swedish, "sv-SE" },
            { Language.Viatnamese, "vi" },
            { Language.Turkish, "tr" },
            { Language.Czech, "cs" },
            { Language.Greek, "el" },
            { Language.Bulgarian, "bl" },
            { Language.Russian, "ru" },
            { Language.Ukranian, "uk" },
            { Language.Thai, "th" },
            { Language.Chinese, "zh-CN" },
            { Language.Japanese, "ja" },
            { Language.Korean, "ko" }
        };

        public static string LanguageToString(Language lang)
        {
            return Languages[lang];
        }

        public static Language StringToLanguage(string langStr)
        {
            foreach (var language in Languages)
            {
                if (language.Value == langStr)
                    return language.Key;
            }

            return Language.EnglishUS;
        }
    }
}
