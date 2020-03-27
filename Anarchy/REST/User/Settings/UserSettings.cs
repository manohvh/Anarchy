using Newtonsoft.Json;
using System;

namespace Discord
{
    public class UserSettings
    {
#pragma warning disable IDE1006
        private readonly Property<string> ThemeProperty = new Property<string>();
        [JsonProperty("theme")]
        private string _theme
        {
            get { return ThemeProperty.Value; }
        }

        public Theme Theme
        {
            get { return (Theme)Enum.Parse(typeof(Theme), _theme, true); }
            set { ThemeProperty.Value = value.ToString().ToLower(); }
        }


        public bool ShouldSerializeTheme()
        {
            return ThemeProperty.Set;
        }


        private readonly Property<ExplicitContentFilter> ExplicityProperty = new Property<ExplicitContentFilter>();
        [JsonProperty("explicit_content_filter")]
        public ExplicitContentFilter ExplicitContentFilter
        {
            get { return ExplicityProperty; }
            set
            { ExplicityProperty.Value = value; }
        }


        public bool ShouldSerializeExplicitContentFilter()
        {
            return ExplicityProperty.Set;
        }


        private readonly Property<bool> DevProperty = new Property<bool>();
        [JsonProperty("developer_mode")]
        public bool DeveloperMode
        {
            get { return DevProperty; }
            set { DevProperty.Value = value; }
        }


        public bool ShouldSerializeDeveloperMode()
        {
            return DevProperty.Set;
        }


        private readonly Property<bool> CompactProperty = new Property<bool>();
        [JsonProperty("message_display_compact")]
        public bool CompactMessages
        {
            get { return CompactProperty; }
            set { CompactProperty.Value = value; }
        }


        public bool ShouldSerializeCompactMessages()
        {
            return CompactProperty.Set;
        }


        private readonly Property<Language> LocaleProperty = new Property<Language>();
        [JsonProperty("locale")]
#pragma warning disable IDE0051
        private string _locale
        {
            get { return LanguageUtils.LanguageToString(LocaleProperty); }
        }
#pragma warning restore IDE0051

        public Language Language
        {
            get { return LocaleProperty.Value; }
            set { LocaleProperty.Value = value; }
        }


        public bool ShouldSerializeLanguage()
        {
            return LocaleProperty.Set;
        }


        private readonly Property<bool> TtsProperty = new Property<bool>();
        [JsonProperty("enable_tts_playback")]
        public bool EnableTts
        {
            get { return TtsProperty; }
            set { TtsProperty.Value = value; }
        }


        public bool ShouldSerializeEnableTts()
        {
            return TtsProperty.Set;
        }


        private readonly Property<bool> GifProperty = new Property<bool>();
        [JsonProperty("gif_auto_play")]
        public bool PlayGifsAutomatically
        {
            get { return GifProperty; }
            set { GifProperty.Value = value; }
        }


        public bool ShouldSerializePlayGifsAutomatically()
        {
            return GifProperty.Set;
        }


        [JsonProperty("custom_status")]
        public CustomStatus CustomStatus { get; set; }
#pragma warning restore IDE1006
    }
}
