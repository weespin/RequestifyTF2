// RequestifyTF2
// Copyright (C) 2018  Villiam Nmerukini
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System.Runtime.InteropServices;

public enum ERole
{
    eConsole,
    eMultimedia,
    eCommunications,
    ERole_enum_count
}

[Guid("f8679f50-850a-41cf-9c72-430f290290c8")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal interface IPolicyConfig
{
    [PreserveSig]
    int GetMixFormat();

    [PreserveSig]
    int GetDeviceFormat();

    [PreserveSig]
    int SetDeviceFormat();

    [PreserveSig]
    int GetProcessingPeriod();

    [PreserveSig]
    int SetProcessingPeriod();

    [PreserveSig]
    int GetShareMode();

    [PreserveSig]
    int SetShareMode();

    [PreserveSig]
    int GetPropertyValue();

    [PreserveSig]
    int SetPropertyValue();

    [PreserveSig]
    int SetDefaultEndpoint([MarshalAs(UnmanagedType.LPWStr)] string wszDeviceId, ERole eRole);

    [PreserveSig]
    int SetEndpointVisibility();
}

[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("568b9108-44bf-40b4-9006-86afe5b5a620")]
internal interface IPolicyConfigVista
{
    [PreserveSig]
    int GetMixFormat();

    [PreserveSig]
    int GetDeviceFormat();

    [PreserveSig]
    int SetDeviceFormat();

    [PreserveSig]
    int GetProcessingPeriod();

    [PreserveSig]
    int SetProcessingPeriod();

    [PreserveSig]
    int GetShareMode();

    [PreserveSig]
    int SetShareMode();

    [PreserveSig]
    int GetPropertyValue();

    [PreserveSig]
    int SetPropertyValue();

    [PreserveSig]
    int SetDefaultEndpoint([MarshalAs(UnmanagedType.LPWStr)] string wszDeviceId, ERole eRole);

    [PreserveSig]
    int SetEndpointVisibility();
}

[ComImport]
[Guid("870af99c-171d-4f9e-af0d-e63df40c2bc9")]
internal class _CPolicyConfigClient
{
}

[ComImport]
[Guid("294935CE-F637-4E7C-A41B-AB255460B862")]
internal class _CPolicyConfigVistaClient
{
}

public class CPolicyConfigClient
{
    private readonly IPolicyConfig _policyConfigClient = new _CPolicyConfigClient() as IPolicyConfig;

    public int SetDefaultDevice(string deviceID)
    {
        _policyConfigClient.SetDefaultEndpoint(deviceID, ERole.eConsole);
        _policyConfigClient.SetDefaultEndpoint(deviceID, ERole.eMultimedia);
        _policyConfigClient.SetDefaultEndpoint(deviceID, ERole.eCommunications);
        return 0;
    }
}

public class CPolicyConfigVistaClient
{
    private readonly IPolicyConfigVista
        _policyConfigVistaClient = new _CPolicyConfigVistaClient() as IPolicyConfigVista;

    public int SetDefaultDevice(string deviceID)
    {
        _policyConfigVistaClient.SetDefaultEndpoint(deviceID, ERole.eConsole);
        _policyConfigVistaClient.SetDefaultEndpoint(deviceID, ERole.eMultimedia);
        _policyConfigVistaClient.SetDefaultEndpoint(deviceID, ERole.eCommunications);
        return 0;
    }
}