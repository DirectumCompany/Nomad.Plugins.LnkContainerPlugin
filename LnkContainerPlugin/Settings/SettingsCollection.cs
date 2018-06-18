using System.Collections.Generic;
using System.Xml.Serialization;

namespace Nomad.Plugins.LnkContainerPlugin.Settings
{
  /// <summary>
  /// Корневой элемент правил авторизации.
  /// </summary>
  [XmlType("settings")]
  public class SettingsCollection
  {
    /// <summary>
    /// Правила авторизации: запрет/разрешение доступа.
    /// </summary>
    [XmlElement("credentials")]
    public List<Credentials> Credentials { get; set; }
  }
}

