using Newtonsoft.Json;
using System;

namespace Discord
{
    /// <summary>
    /// This is currently only used in OnUserUpdated.
    /// Due to the structure of Discord's API only settings that have been changed will have a reaction on this, so to make sure that something has been changed check the PropertyNameSet property
    /// </summary>
    public class Settings
    {
        private readonly Property<string> ThemeProperty = new Property<string>();
        [JsonProperty("theme")]
#pragma warning disable IDE1006
        private string _theme
        {
            get { return ThemeProperty.Value; }
            set { ThemeProperty.Value = value; }
        }
#pragma warning restore IDE1006

        public Theme Theme
        {
            get { return (Theme)Enum.Parse(typeof(Theme), _theme, true); }
            internal set { ThemeProperty.Value = value.ToString().ToLower(); }
        }

        public bool ThemeSet
        {
            get { return ThemeProperty.Set; }
        }


        private readonly Property<ExplicitContentFilter> FilterProperty = new Property<ExplicitContentFilter>();
        [JsonProperty("explicit_content_filter")]
        public ExplicitContentFilter ExplicitContentFilter
        {
            get { return FilterProperty; }
            internal set { FilterProperty.Value = value; }
        }


        public bool ExplicitContentFilterSet
        {
            get { return FilterProperty.Set; }
        }


        private readonly Property<bool> DeveloperProperty = new Property<bool>();
        [JsonProperty("developer_mode")]
        public bool DeveloperMode
        {
            get { return DeveloperProperty; }
            internal set { DeveloperProperty.Value = value; }
        }


        public bool DeveloperModeSet
        {
            get { return DeveloperProperty.Set; }
        }


        private readonly Property<bool> CompactProperty = new Property<bool>();
        [JsonProperty("message_display_compact")]
        public bool CompactMessages
        {
            get { return CompactProperty; }
            internal set { CompactProperty.Value = value; }
        }


        public bool CompactMessagesSet
        {
            get { return CompactProperty.Set; }
        }


        private readonly Property<string> LocaleProperty = new Property<string>();
        [JsonProperty("locale")]
        public string Language
        {
            get { return LocaleProperty; }
            internal set { LocaleProperty.Value = value; }
        }


        public bool LanguageSet
        {
            get { return LocaleProperty.Set; }
        }


        private readonly Property<bool> TtsProperty = new Property<bool>();
        [JsonProperty("enable_tts_playback")]
        public bool EnableTts
        {
            get { return TtsProperty; }
            internal set { TtsProperty.Value = value; }
        }


        public bool EnableTtsSet
        {
            get { return TtsProperty.Set; }
        }


        private readonly Property<bool> GifProperty = new Property<bool>();
        [JsonProperty("gif_auto_play")]
        public bool PlayGifsAutomatically
        {
            get { return GifProperty; }
            internal set { GifProperty.Value = value; }
        }


        public bool PlayGifsAutomaticallySet
        {
            get { return GifProperty.Set; }
        }


        private readonly Property<CustomStatus> StatusProperty = new Property<CustomStatus>();
        [JsonProperty("custom_status")]
        public CustomStatus CustomStatus
        {
            get { return StatusProperty; }
            internal set { StatusProperty.Value = value; }
        }


        public bool CustomStatusSet
        {
            get { return StatusProperty.Set; }
        }
    }
}
