using NpoComputer.Nomad.Contract;
using System;

namespace Nomad.Plugins.LnkContainerPlugin
{
  /// <summary>
  /// Объект из контейнера <see cref="LnkContainer"/>.
  /// </summary>
  public class LnkContainerEntry : IContainerEntry
  {
    /// <summary>
    /// Уникальный ключ объекта в контейнере.
    /// </summary>
    public string Key { get; }

    /// <summary>
    /// Имя объекта с расширением.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Признак того, что объект является каталогом.
    /// </summary>
    public bool IsDirectory { get; }

    /// <summary>
    /// Время создания объекта.
    /// </summary>
    public DateTime Created { get; }

    /// <summary>
    /// Время изменения объекта.
    /// </summary>
    public DateTime Modified { get; }

    /// <summary>
    /// Размер объекта.
    /// </summary>
    public long? Size { get; }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="key">Ключ объекта.</param>
    /// <param name="name">Имя объекта.</param>
    /// <param name="created">Время создания объекта.</param>
    /// <param name="modified">Время изменения объекта.</param>
    public LnkContainerEntry(string key, string name, DateTime created, DateTime modified)
    {
      Key = key;
      Name = name;
      Created = created;
      Modified = modified;
      Size = null;
      IsDirectory = true;
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="key">Ключ объекта.</param>
    /// <param name="name">Имя объекта.</param>
    /// <param name="created">Время создания объекта.</param>
    /// <param name="modified">Время изменения объекта.</param>
    /// <param name="size">Размер объекта.</param>
    public LnkContainerEntry(string key, string name, DateTime created, DateTime modified, long? size)
    {
      Key = key;
      Name = name;
      Created = created;
      Modified = modified;
      Size = size;
      IsDirectory = false;
    }
  }
}