namespace Nomad.Plugins.NetworkConnection
{
  public enum ResourceScope : int
  {
    Connected = 1,
    GlobalNetwork,
    Remembered,
    Recent,
    Context
  };
}
