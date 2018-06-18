using System.Xml.Serialization;

namespace Nomad.Plugins.LnkContainerPlugin.Settings
{
  /// <summary>
  /// Реквизиты доступа к сетевому ресурсу.
  /// </summary>
  [XmlType("credentials")]
  public class Credentials
  {
    /// <summary>
    /// Сетевой ресурс.
    /// </summary>
    /// <remarks>Сетевой ресурс доступ к которому требует отдельные реквизиты.</remarks>
    [XmlAttribute("share")]
    public string Share { get; set; }

    /// <summary>
    /// Имя пользователя.
    /// </summary>
    /// <remarks>Имя пользователя для доступа к сетевому ресурсу.</remarks>
    [XmlAttribute("user")]
    public string User { get; set; }

    /// <summary>
    /// Пароль пользователя.
    /// </summary>
    /// <remarks>Пароль пользователя для доступа к сетевому ресурсу.</remarks>
    [XmlAttribute("password")]
    public string Password { get; set; }
  }
}
