using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using ManagedCommon;
using Microsoft.PowerToys.Settings.UI.Library;
using Wox.Plugin;

namespace Community.PowerToys.Run.Plugin.PowerToys_Run_Plugin_MB
{
    public class Main : IPlugin, IPluginI18n, IContextMenu, ISettingProvider, IReloadable, IDisposable, IDelayedExecutionPlugin
    {
        private const string Setting = nameof(Setting);

        // current value of the setting
        private bool _setting;

        private PluginInitContext _context;

        private string _iconPath;

        private bool _disposed;

        public string Name => Properties.Resources.plugin_name;

        public string Description => Properties.Resources.plugin_description;

        // TODO: remove dash from ID below and inside plugin.json
        public static string PluginID => "5341e9c16dd94d6ea75a75e840d994d9";

        // TODO: add additional options (optional)
        public IEnumerable<PluginAdditionalOption> AdditionalOptions => new List<PluginAdditionalOption>()
        {
            new PluginAdditionalOption()
            {
                PluginOptionType= PluginAdditionalOption.AdditionalOptionType.Checkbox,
                Key = Setting,
                DisplayLabel = Properties.Resources.plugin_setting,
            },
        };

        public void UpdateSettings(PowerLauncherPluginSettings settings)
        {
            _setting = settings?.AdditionalOptions?.FirstOrDefault(x => x.Key == Setting)?.Value ?? false;
        }

        // return context menus for each Result (optional)
        public List<ContextMenuResult> LoadContextMenus(Result selectedResult)
        {
            return new List<ContextMenuResult>(0);
        }

        // return query results
        public List<Result> Query(Query query)
        {
            ArgumentNullException.ThrowIfNull(query);

            var results = new List<Result>();

            // empty query
            if (string.IsNullOrEmpty(query.Search))
            {
                results.Add(new Result
                {
                    Title = $"Open save destination",
                    SubTitle = Description,
                    QueryTextDisplay = string.Empty,
                    IcoPath = _iconPath,
                    Action = action =>
                    {
                        return OpenSaveFolder();
                    },
                });
                return results;
            }
            else
            {
                results.Add(new Result
                {
                    Title = $"Save clipboard as '{query.Search}' in 'Downloads' folder and open it",
                    SubTitle = Description,
                    QueryTextDisplay = string.Empty,
                    IcoPath = _iconPath,
                    Action = action =>
                    {
                        return SaveClipboardAsFileAndOpenIt(query.Search);
                    },
                });
                results.Add(new Result
                {
                    Title = $"Save clipboard as '{query.Search}' in 'Downloads' folder",
                    SubTitle = Description,
                    QueryTextDisplay = string.Empty,
                    IcoPath = _iconPath,
                    Action = action =>
                    {
                        return SaveClipboardAsFile(query.Search);
                    },
                });

                return results;
            }
        }

        // return delayed query results (optional)
        public List<Result> Query(Query query, bool delayedExecution)
        {
            ArgumentNullException.ThrowIfNull(query);

            var results = new List<Result>();

            // empty query
            if (string.IsNullOrEmpty(query.Search))
            {
                return results;
            }

            return results;
        }

        public void Init(PluginInitContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _context.API.ThemeChanged += OnThemeChanged;
            UpdateIconPath(_context.API.GetCurrentTheme());
        }

        public string GetTranslatedPluginTitle()
        {
            return Properties.Resources.plugin_name;
        }

        public string GetTranslatedPluginDescription()
        {
            return Properties.Resources.plugin_description;
        }

        private void OnThemeChanged(Theme oldTheme, Theme newTheme)
        {
            UpdateIconPath(newTheme);
        }

        private void UpdateIconPath(Theme theme)
        {
            if (theme == Theme.Light || theme == Theme.HighContrastWhite)
            {
                _iconPath = "Images/MB_logo.png";
            }
            else
            {
                _iconPath = "Images/MB_logo.png";
            }
        }

        public Control CreateSettingPanel()
        {
            throw new NotImplementedException();
        }

        public void ReloadData()
        {
            if (_context is null)
            {
                return;
            }

            UpdateIconPath(_context.API.GetCurrentTheme());
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                if (_context != null && _context.API != null)
                {
                    _context.API.ThemeChanged -= OnThemeChanged;
                }

                _disposed = true;
            }
        }

        private static bool SaveClipboardAsFile(string? value)
        {
            var clipboardContent = Clipboard.GetText();
            if (clipboardContent == null) return false;

            File.WriteAllText(GetFilePath(value), clipboardContent);
            return true;
        }

        private static bool SaveClipboardAsFileAndOpenIt(string? value)
        {
            if (SaveClipboardAsFile(value))
            {
                using Process fileopener = new Process();

                fileopener.StartInfo.FileName = "explorer";
                fileopener.StartInfo.Arguments = GetFilePath(value);
                fileopener.Start();
            }

            return true;
        }

        private static string GetFilePath(string? value)
        {
            var filename = value ?? "PowerToys Run Plugin by Michael Brand.txt";
            var path = KnownFolders.GetPath(KnownFolder.Downloads);

            return Path.Combine(path, filename);
        }

        private bool OpenSaveFolder()
        {
            Process.Start("explorer", KnownFolders.GetPath(KnownFolder.Downloads));
            return true;
        }
    }
}
