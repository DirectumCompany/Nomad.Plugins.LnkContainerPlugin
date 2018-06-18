using Nomad.Plugins.LnkContainerPlugin.Settings;
using NpoComputer.Nomad.Contract;
using NpoComputer.Nomad.Contract.Logging;
using NpoComputer.Nomad.Utility;
using System.Collections.Generic;
using System.Linq;

namespace Nomad.Plugins.LnkContainerPlugin
{
  /// <summary>
  /// Конфигурация плагина.
  /// </summary>
  class PluginConfiguration
  {
    /// <summary>
    /// Возвращает <see cref="ILog"/> текущего класса.
    /// </summary>
    private static ILog Log
    {
      get { return LogManager.GetLogger(typeof(PluginConfiguration)); }
    }

    /// <summary>
    /// Менеджер пользовательской конфигурации.
    /// </summary>
    private static readonly HotPlugConfigManager _defaultConfigManager;

    /// <summary>
    /// Элементы доступа.
    /// </summary>
    public static Credentials[] Credentials
    {
      get
      {
        return _сredentials;
      }
    }

    private static Credentials[] _сredentials;

    /// <summary>
    /// Конструктор.
    /// </summary>
    static PluginConfiguration()
    {
      var assembly = typeof(PluginConfiguration).Assembly;
      var fullpath = ConfigurationUtils.GetAssemblyConfigurationFilePath(assembly);
      var pluginName = System.IO.Path.GetFileName(fullpath);
      var path = System.IO.Path.GetDirectoryName(fullpath);
      _defaultConfigManager = new HotPlugConfigManager(pluginName, path, "configuration", enableWatch: true);

      _defaultConfigManager.ConfigChanged += (sender, args) =>
        {
          Log.TraceFormat("Configuration reloaded: {0}", pluginName);
          FillSettings();
        };

      FillSettings();
    }

    /// <summary>
    /// Спрятаный конструктор.
    /// </summary>
    /// <remarks>Add a ‘protected’ constructor or the ‘static’ keyword to the class declaration. (S1118)</remarks>
    protected PluginConfiguration()
    {
    }

    /// <summary>
    /// Заполнение настроек.
    /// </summary>
    private static void FillSettings()
    {
      var authorizationSettings = _defaultConfigManager.GetSection<SettingsCollection>("settings");
      _сredentials = authorizationSettings?.Credentials.ToArray();

      if (_сredentials == null)
        _сredentials = new Credentials[0];

      Log.TraceFormat("Credentials Settings Count: {0}", _сredentials.Length);
    }
  }
}
