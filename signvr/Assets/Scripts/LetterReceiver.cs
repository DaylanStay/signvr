using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class LetterReceiver : MonoBehaviour {
    private TcpClient client;
    private NetworkStream stream;
    private Thread thread;
    private string receivedLetter = " ";
    public string Letter => receivedLetter;

    void Start() {
        DontDestroyOnLoad(gameObject);
        thread = new Thread(ConnectToPython);
        thread.IsBackground = true;
        thread.Start();
    }

    void ConnectToPython() {
        try {
            client = new TcpClient("127.0.0.1", 12345);
            stream = client.GetStream();

            byte[] buffer = new byte[1];

            while (true) {
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead > 0) {
                    lock (this) {
                        receivedLetter = Encoding.UTF8.GetString(buffer).ToUpper();
                    }
                }
            }
        } catch {
            Debug.Log("No se pudo conectar al reconocedor de señas.");
        }
    }
    
    // --- MÉTODO AÑADIDO ---
    // Reinicia la letra para asegurar que se procesa solo una vez.
    public void ClearLetter() {
        lock (this) {
            receivedLetter = " ";
        }
    }

    void OnApplicationQuit() {
        stream?.Close();
        client?.Close();
        thread?.Abort();
    }
}