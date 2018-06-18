using NpoComputer.Nomad.Contract;
using NpoComputer.Nomad.Contract.Logging;
using ParseLnk.Interop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Nomad.Plugins.LnkContainerPlugin
{
  /// <summary>
  /// Контейнер Lnk-файлов.
  /// </summary>
  public class LnkContainer : IContainer
  {
    #region Поля и свойства
    /// <summary>
    /// Логгер.
    /// </summary>
    internal static ILog Log => LogManager.GetLogger<LnkContainerReaderPlugin>();

    /// <summary>
    /// Расширение Lnk-файлов.
    /// </summary>
    public static readonly string Extension = "lnk";

    /// <summary>
    /// Парсер ярлыков.
    /// </summary>
    private readonly ParseLnk.Parser _lnkParser;

    /// <summary>
    /// Путь к каталогу на который ссылается ярлык.
    /// </summary>
    private readonly string _networkPath;

    /// <summary>
    /// Сетевое соединение, если требуется дополнительная аутентификация.
    /// </summary>
    private readonly NetworkConnection.NetworkConnection _connection;

    #endregion

    #region IContainer

    /// <summary>
    /// Получить список корневых элементов.
    /// </summary>
    public IEnumerable<IContainerEntry> GetEntries()
    {
      return GetDirectoryList(new DirectoryInfo(_networkPath), "*");
    }

    /// <summary>
    /// Получить список дочерних элементов.
    /// </summary>
    /// <param name="entryKey"></param>
    /// <returns></returns>
    public IEnumerable<IContainerEntry> GetChildEntries(string entryKey)
    {
      Log.TraceFormat("GetChildEntries: {0}", entryKey);

      return GetDirectoryList(new DirectoryInfo(Path.Combine(_networkPath, entryKey)), "*");
    }

    /// <summary>
    /// Обход папки.
    /// </summary>
    /// <param name="directory"></param>
    /// <param name="searchPattern"></param>
    /// <returns></returns>
    private IEnumerable<IContainerEntry> GetDirectoryList(DirectoryInfo directory, string searchPattern)
    {
      foreach (var fileInfo in directory.GetFiles(searchPattern))
      {
        Log.TraceFormat("File: {0}", fileInfo.FullName);

        yield return new LnkContainerEntry(
          NormalizePath(fileInfo.FullName, _networkPath),
          fileInfo.Name,
          fileInfo.CreationTimeUtc,
          fileInfo.LastWriteTimeUtc,
          fileInfo.Length);
      }

      foreach (var directoryInfo in directory.GetDirectories())
      {
        Log.TraceFormat("Directory: {0}", directoryInfo.FullName);

        yield return new LnkContainerEntry(
          NormalizePath(directoryInfo.FullName, _networkPath),
          directoryInfo.Name,
          directoryInfo.CreationTimeUtc,
          directoryInfo.LastWriteTimeUtc);
      }
    }

    /// <summary>
    /// Получить содержимое объекта по его ключу.
    /// </summary>
    /// <param name="entryKey">Ключ объекта.</param>
    public Stream GetEntryStream(string entryKey)
    {
      Log.TraceFormat("GetEntryStream: {0}", entryKey);

      var fullpath = Path.Combine(_networkPath, entryKey);
      if (!File.Exists(fullpath))
        throw new ArgumentException($"Entry with key {entryKey} not found.");

      return new FileStream(fullpath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
    }

    /// <summary>
    /// Приводит путь объекта в контейнере к единому виду для всех типов контейнеров.
    /// </summary>
    /// <param name="path">Путь к объекту в контейнере.</param>
    protected static string NormalizePath(string fullPath, string rootPath)
    {
      return fullPath.Replace(rootPath, string.Empty).Replace('/', '\\').Trim('\\');
    }
    #endregion

    #region IDisposable

    /// <summary>
    /// Освобождение неуправляемых ресурсов.
    /// </summary>
    public void Dispose()
    {
      _connection?.Dispose();
    }

    #endregion

    #region Конструкторы

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="stream">Содержимое контейнера.</param>
    public LnkContainer(Stream stream)
    {
      _lnkParser = new ParseLnk.Parser(stream);
      _lnkParser.Parse();

      // Читать только ссылки на сетевые ресурсы. Сключая прямые ссылки на файлы.
      if (_lnkParser.LinkInfo.Header.Flags.HasFlag(Enums.LinkInfoFlags.CommonNetworkRelativeLinkAndPathSuffix))
      {
        _networkPath = _lnkParser.LinkInfo.CommonNetworkRelativeLink.NetName;
        Log.TraceFormat("CommonNetworkRelativeLink: {0}, Flags:{1}", _lnkParser.LinkInfo.CommonNetworkRelativeLink.NetName, _lnkParser.LinkInfo.Header.Flags);

        var configuration = PluginConfiguration.Credentials.FirstOrDefault(c => _networkPath.IndexOf(c.Share, StringComparison.OrdinalIgnoreCase) >= 0);

        if (configuration != null)
        {
          Log.TraceFormat("Using credentials settings for '{0}' share", _networkPath);
          _connection = new NetworkConnection.NetworkConnection(_networkPath, new System.Net.NetworkCredential(configuration.User, configuration.Password));
        }
      }
    }
    #endregion
  }
}