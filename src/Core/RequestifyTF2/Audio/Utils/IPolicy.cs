/*
 * Created by SharpDevelop.
 * User: aifdsc
 * Date: 07/31/2011
 * Time: 08:09
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
// Code From https://github.com/aifdsc/AudioChanger
// Thanks Stephan for your work


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