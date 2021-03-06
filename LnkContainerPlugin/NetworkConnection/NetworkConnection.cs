﻿using System;
using System.ComponentModel;
using System.Net;
using System.Runtime.InteropServices;

namespace Nomad.Plugins.NetworkConnection
{
  public class NetworkConnection : IDisposable
  {
    string _networkName;

    public NetworkConnection(string networkName,
        NetworkCredential credentials)
    {
      _networkName = networkName;

      var netResource = new NetResource()
      {
        Scope = ResourceScope.GlobalNetwork,
        ResourceType = ResourceType.Disk,
        DisplayType = ResourceDisplaytype.Share,
        RemoteName = networkName
      };

      var userName = string.IsNullOrEmpty(credentials.Domain)
          ? credentials.UserName
          : string.Format(@"{0}\{1}", credentials.Domain, credentials.UserName);

      var result = WNetAddConnection2(
          netResource,
          credentials.Password,
          userName,
          0);

      if (result != 0)
      {
        throw new Win32Exception(result, "Error connecting to remote share");
      }
    }

    ~NetworkConnection()
    {
      Dispose(false);
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
      WNetCancelConnection2(_networkName, 0, true);
    }

    [DllImport("mpr.dll")]
    private static extern int WNetAddConnection2(NetResource netResource,
        string password, string username, int flags);

    [DllImport("mpr.dll")]
    private static extern int WNetCancelConnection2(string name, int flags,
        bool force);
  }
}
