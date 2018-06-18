using NpoComputer.Nomad.Contract;
using NpoComputer.Nomad.Contract.Extensions;
using NpoComputer.Nomad.Contract.Logging;
using System.Collections.Generic;
using System.IO;

namespace Nomad.Plugins.LnkContainerPlugin
{
  /// <summary>
  /// Контейнер Lnk-файлов.
  /// </summary>
  public class LnkContainerReaderPlugin : IContainerReaderPlugin
  {
    /// <summary>
    /// Логгер.
    /// </summary>
    internal static ILog Log => LogManager.GetLogger<LnkContainerReaderPlugin>();

    /// <summary>
    /// Получает файловый контейнер.
    /// </summary>
    /// <param name="stream">Содержимое файла.</param>
    public IContainer GetContainer(Stream stream)
    {
      return new LnkContainer(stream);
    }

    /// <summary>
    /// Получает поддерживаемые расширения файлов.
    /// </summary>
    public IEnumerable<string> GetSupportedExtensions()
    {
      return new string[] { LnkContainer.Extension };
    }

    /// <summary>
    /// Выполняет инициализацию.
    /// </summary>
    public void Initialize()
    {
      Log.Trace("LnkContainerReaderPlugin initialization");
    }
  }
}