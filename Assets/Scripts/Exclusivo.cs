using UnityEngine;
using TMPro;
using System.Diagnostics;

public class Exclusivo : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI nome;

    // Start is called before the first frame update
    void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            nome.text = "Temp: " + GetTemperature();
        } else {
            nome.text = "PC!";
        }
    }
    public static string GetTemperature()
    {
        Process process = new Process();
        process.StartInfo.FileName = "cat"; // usar o shell do bash
        process.StartInfo.Arguments = "/sys/class/thermal/thermal_zone0/temp"; // executar o comando cat com o nome do arquivo como argumento
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true; // redirecionar o output padrão para que possamos lê-lo
        process.Start();

        string output = process.StandardOutput.ReadToEnd(); // ler todo o output

        process.WaitForExit(); // aguardar a conclusão do processo

        return output;
    }
}
