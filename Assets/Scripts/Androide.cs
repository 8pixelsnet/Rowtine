using System;
using System.Collections;
using System.Collections.Generic;
using System.Management;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;


public class Androide : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI nome;
    //[SerializeField] RawImage rawImage;

    void Start()
    {
        float screenWidth = Mathf.RoundToInt(Screen.width);
        float screenHeight = Mathf.RoundToInt(Screen.height);

        float scaleFactor = Screen.dpi / 160f;
        float fontScale = scaleFactor * (Screen.width / Screen.height);

        float screenDpi = Screen.dpi;
        string deviceId = SystemInfo.deviceUniqueIdentifier;
        string deviceName = SystemInfo.deviceName;
        string deviceModel = SystemInfo.deviceModel;
        DeviceType deviceType = SystemInfo.deviceType;
        string processorType = SystemInfo.processorType;
        int processorCount = SystemInfo.processorCount;
        float processorFrequency = SystemInfo.processorFrequency;
        int systemMemorySize = SystemInfo.systemMemorySize;
        string operatingSystem = SystemInfo.operatingSystem;

        bool gyroscope = SystemInfo.supportsGyroscope;
        string giroscopio = "";
        if (gyroscope) { giroscopio = "Sim"; } else { giroscopio = "Não"; }

        string temNet = "";
        if (IsInternetAvailable()) { temNet = "Sim"; } else { temNet = "Não"; }

        nome.text = "Versão do Android: " + GetAndroidVersion() + "\n" +
                    /*
                    "Tam. de partição: " + GetSystemPartitionSize() + "\n" +
                    "Espaço livre: " + GetSystemPartitionFreeSpace() + "\n" +
                    */

                    "IPv4: " + GetLocalIPv4() + "\n" +
                    "IPv6: " + GetLocalIPv6() + "\n" +
                    "Wifi IPv4: " + GetWifiIPv4() + "\n" +
                    "Wifi IPv6: " + GetWifiIPv6() + "\n" +
                    "Compilação: " + GetBuildNumber() + "\n" +

                    "DHCP DNS 1: " + GetDhcpDns1() + "\n" +
                    "DHCP DNS 2: " + GetDhcpDns2() + "\n" +
                    "IP: " + GetIpAddress() + "\n" +


                    "Arquitetura: " + GetCpuArchitecture() + "\n" +
                    "RAM: " + systemMemorySize + "\n" +

                    "Bateria do Android: " + GetBatteryLevel() + "% \n" +
                    "Estado da bateria: " + BatteryState() + "\n" +
                    "Suporte a giroscópio: " + giroscopio + "\n" +
                    "SDK: " + GetSDKLevel() + "\n" +
                    "Kernel: " + KernelVersion() + "\n" +

                    "Orientação: " + Screen.orientation + "\n" +
                    "DPI da tela: " + screenDpi + "\n" +
                    //"Escala da fonte: " + fontScale + "x" + "\n" +
                    "Altura e largura da tela: " + screenWidth + " x " + screenHeight + "\n" +
                    "Nome do dispositivo: " + deviceName + "\n" +
                    "Modelo: " + deviceModel + "\n" +
                    "Tipo de processador: " + processorType + "\n" +
                    "Núcleos: " + processorCount + "\n" +
                    "Frequência de GHz: " + processorFrequency + "\n" +
                    "OS: " + operatingSystem + "\n" +
                    "Tipo de aparelho: " + deviceType + "\n" +
                    "Ligado à internet? " + temNet + "\n" +
                    "Idioma: " + GetDeviceLanguage() + "\n" +
                    "Resolução da câmera: " + GetCameraResolution() + "\n" +
                    "ID do dispositivo: " + deviceId;


        //nome.text += "mais coisa";
    }

    public int GetSDKLevel()
    {
        var clazz = AndroidJNI.FindClass("android/os/Build$VERSION");
        var fieldID = AndroidJNI.GetStaticFieldID(clazz, "SDK_INT", "I");
        var sdkLevel = AndroidJNI.GetStaticIntField(clazz, fieldID);
        return sdkLevel;
    }
    
    public float GetBatteryLevel() {
        if (Application.platform == RuntimePlatform.Android)
        {
            // Cria um objeto Java que representa a classe UnityPlayer do Android
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

            // Obtém o contexto atual do Android a partir da classe UnityPlayer
            AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity").Call<AndroidJavaObject>("getApplicationContext");

            // Cria um objeto Java que representa a classe BatteryManager do Android
            AndroidJavaObject batteryManager = context.Call<AndroidJavaObject>("getSystemService", "batterymanager");

            // Obtém o nível atual da bateria
            int level = batteryManager.Call<int>("getIntProperty", 4); // 4 é o valor de BATTERY_PROPERTY_CAPACITY em Android 9 (API 28)

            // Retorna a porcentagem da bateria
            return level / 100f;
        }
        else
        {
            // Se a plataforma não for Android, retorna -1
            return -1f;
        }
    }
    public string GetAndroidVersion() {
        string version = "";
        if (Application.platform == RuntimePlatform.Android) {
            AndroidJavaClass buildClass = new AndroidJavaClass("android.os.Build$VERSION");
            version = buildClass.GetStatic<int>("SDK_INT").ToString();
        }
        return version;
    }
    public bool IsInternetAvailable() {
        return Application.internetReachability != NetworkReachability.NotReachable;
    }
    public string GetDeviceLanguage() {
        return Application.systemLanguage.ToString();
    }
    string BatteryState() {
        BatteryStatus batteryStatus = SystemInfo.batteryStatus;

        switch (batteryStatus) {
            case BatteryStatus.Charging:
                return "Bateria está carregando.";
            case BatteryStatus.Discharging:
                return "Bateria está descarregando.";
            case BatteryStatus.Full:
                return "Bateria está totalmente carregada.";
            case BatteryStatus.NotCharging:
                return "Bateria não está carregando.";
            case BatteryStatus.Unknown:
                return "Estado da bateria desconhecido.";
            default:
                return null;
        }
    }
    string GetCameraResolution() {
        WebCamTexture webcamTexture = new WebCamTexture();
        webcamTexture.requestedFPS = 30; // definir a taxa de quadros para a câmera

        webcamTexture.Play();

        string textoRetornavel = "A resolução da câmera é " + webcamTexture.width + " x " + webcamTexture.height;

        //rawImage.texture = webcamTexture;

        webcamTexture.Stop();
        return textoRetornavel;
    }

    string KernelVersion()
    {
        string kernelVersion = Environment.OSVersion.Version.ToString();
        return kernelVersion;
    }

    string GetLocalIPv4() {
        string localIPv4 = "";

        foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces()) {
            if (networkInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || networkInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet) {
                foreach (UnicastIPAddressInformation ipInfo in networkInterface.GetIPProperties().UnicastAddresses) {
                    if (ipInfo.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork) {
                        localIPv4 = ipInfo.Address.ToString();
                        break;
                    }
                }
            }

            if (!string.IsNullOrEmpty(localIPv4)) {
                break;
            }
        }

        return localIPv4;
    }

    string GetLocalIPv6()
    {
        string localIPv6 = "";

        foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (networkInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || networkInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
            {
                foreach (UnicastIPAddressInformation ipInfo in networkInterface.GetIPProperties().UnicastAddresses)
                {
                    if (ipInfo.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                    {
                        localIPv6 = ipInfo.Address.ToString();
                        break;
                    }
                }
            }

            if (!string.IsNullOrEmpty(localIPv6))
            {
                break;
            }
        }

        return localIPv6;
    }

    string GetWifiIPv4()
    {
        string wifiIPv4 = "";

        NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

        foreach (NetworkInterface networkInterface in networkInterfaces.Where(ni => ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211))
        {
            IPInterfaceProperties ipProperties = networkInterface.GetIPProperties();

            foreach (UnicastIPAddressInformation ipAddressInfo in ipProperties.UnicastAddresses)
            {
                if (ipAddressInfo.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    wifiIPv4 = ipAddressInfo.Address.ToString();
                    break;
                }
            }

            if (!string.IsNullOrEmpty(wifiIPv4))
            {
                break;
            }
        }

        return wifiIPv4;
    }

    string GetWifiIPv6()
    {
        string wifiIPv6 = "";

        NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

        foreach (NetworkInterface networkInterface in networkInterfaces.Where(ni => ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211))
        {
            foreach (UnicastIPAddressInformation ipAddressInfo in networkInterface.GetIPProperties().UnicastAddresses)
            {
                if (ipAddressInfo.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                {
                    wifiIPv6 = ipAddressInfo.Address.ToString();
                    break;
                }
            }

            if (!string.IsNullOrEmpty(wifiIPv6))
            {
                break;
            }
        }

        return wifiIPv6;
    }

    public string GetDhcpDns1()
    {
        string dns1 = "";

#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
        string ipAddress = GetIpAddress();
        if (!string.IsNullOrEmpty(ipAddress))
        {
            IPAddress[] dnsAddresses = Dns.GetHostAddresses(ipAddress);
            if (dnsAddresses.Length > 0)
            {
                dns1 = dnsAddresses[0].ToString();
            }
        }
#else
        NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
        foreach (NetworkInterface iface in interfaces)
        {
            if (iface.NetworkInterfaceType == NetworkInterfaceType.Ethernet || iface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
            {
                IPInterfaceProperties ifaceProperties = iface.GetIPProperties();
                if (ifaceProperties != null && ifaceProperties.DhcpServerAddresses.Count > 0)
                {
                    foreach (IPAddress dhcpServerAddress in ifaceProperties.DhcpServerAddresses)
                    {
                        IPHostEntry host = Dns.GetHostEntry(dhcpServerAddress);
                        if (host != null && host.AddressList.Length > 0)
                        {
                            dns1 = host.AddressList[0].ToString();
                            break;
                        }
                    }
                }
            }
        }
#endif

        return dns1;
    }

    public string GetDhcpDns2()
    {
        string dns2 = "";

#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
        string ipAddress = GetIpAddress();
        if (!string.IsNullOrEmpty(ipAddress))
        {
            IPAddress[] dnsAddresses = Dns.GetHostAddresses(ipAddress);
            if (dnsAddresses.Length > 1)
            {
                dns2 = dnsAddresses[1].ToString();
            }
        }
#else
        NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
        foreach (NetworkInterface iface in interfaces)
        {
            if (iface.NetworkInterfaceType == NetworkInterfaceType.Ethernet || iface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
            {
                IPInterfaceProperties ifaceProperties = iface.GetIPProperties();
                if (ifaceProperties != null && ifaceProperties.DnsAddresses.Count > 1)
                {
                    dns2 = ifaceProperties.DnsAddresses[1].ToString();
                    break;
                }
            }
        }
#endif

        return dns2;
    }

    private string GetIpAddress()
    {
        string ipAddress = "";

        IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress address in hostEntry.AddressList)
        {
            if (address.AddressFamily == AddressFamily.InterNetwork)
            {
                ipAddress = address.ToString();
                break;
            }
        }

        return ipAddress;
    }

    public string GetBuildNumber()
    {
        string version = Application.version;
        string buildGUID = Application.buildGUID;

        string[] buildGUIDParts = buildGUID.Split('-');
        if (buildGUIDParts.Length < 2)
        {
            return version;
        }

        string buildNumber = buildGUIDParts[1];
        return $"{version} ({buildNumber})";
    }

    public long GetSystemPartitionSize()
    {
        string drive = Path.GetPathRoot(Environment.SystemDirectory);
        DriveInfo driveInfo = new DriveInfo(drive);
        return driveInfo.TotalSize;
    }

    public long GetSystemPartitionFreeSpace()
    {
        string drive = Path.GetPathRoot(Environment.SystemDirectory);
        DriveInfo driveInfo = new DriveInfo(drive);
        return driveInfo.AvailableFreeSpace;
    }

    public string GetCpuArchitecture()
    {
        string processorType = SystemInfo.processorType.ToLower();
        int processorCount = SystemInfo.processorCount;

        if (processorType.Contains("arm") || processorType.Contains("aarch64"))
        {
            // ARM-based processor
            if (processorType.Contains("v7"))
            {
                return $"ARMv7 ({processorCount} cores)";
            }
            else if (processorType.Contains("v8"))
            {
                return $"ARMv8 ({processorCount} cores)";
            }
            else if (processorType.Contains("64")) {
                return $"Unknown ARM ({processorCount} cores)";
            }
            else
            {
                return $"Unknown ({processorCount} cores)";
            }
        }
        else if (processorType.Contains("x86"))
        {
            // x86-based processor
            if (processorType.Contains("64"))
            {
                return $"x86-64 ({processorCount} cores)";
            }
            else
            {
                return $"x86 ({processorCount} cores)";
            }
        }
        else if (processorType.Contains("mips"))
        {
            // MIPS-based processor
            return $"MIPS ({processorCount} cores)";
        }
        else
        {
            // Unknown processor
            return $"Unknown ({processorCount} cores)";
        }
    }

}