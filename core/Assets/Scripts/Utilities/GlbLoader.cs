using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

#if GLTF_UTILITY
using Siccity.GLTFUtility;
#endif

namespace Barbara.Core
{
    /// <summary>
    /// Faz download e importa modelos GLB em tempo de execução.
    /// </summary>
    public class GlbLoader : MonoBehaviour
    {
        private static GlbLoader _instance;

        public static GlbLoader Instance
        {
            get
            {
                if (_instance == null)
                {
                    var go = new GameObject("GlbLoader");
                    _instance = go.AddComponent<GlbLoader>();
                    DontDestroyOnLoad(go);
                }

                return _instance;
            }
        }

        public void Load(string url, Action<GameObject> onSuccess, Action<string> onError)
        {
            if (string.IsNullOrEmpty(url))
            {
                onError?.Invoke("URL do modelo 3D não informada.");
                return;
            }

            StartCoroutine(LoadRoutine(url, onSuccess, onError));
        }

        private IEnumerator LoadRoutine(string url, Action<GameObject> onSuccess, Action<string> onError)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                request.downloadHandler = new DownloadHandlerBuffer();
                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    onError?.Invoke($"Falha ao baixar GLB: {request.error}");
                    yield break;
                }

                byte[] glbBytes = request.downloadHandler.data;

#if GLTF_UTILITY
                try
                {
                    var settings = new ImportSettings
                    {
                        materials = true,
                        generateTerrainNormals = true
                    };

                    GameObject loaded = Importer.LoadFromBytes(glbBytes, settings);
                    if (loaded == null)
                    {
                        onError?.Invoke("GLB retornou objeto nulo.");
                        yield break;
                    }

                    onSuccess?.Invoke(loaded);
                }
                catch (Exception ex)
                {
                    onError?.Invoke($"Erro ao importar GLB: {ex.Message}");
                }
#else
                Debug.LogWarning("GLTFUtility não está configurado. Defina a Scripting Define GLTF_UTILITY após instalar Siccity.GLTFUtility.");
                onError?.Invoke("Importador GLB indisponível. Consulte a documentação para habilitar GLTFUtility.");
#endif
            }
        }
    }
}
