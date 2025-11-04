using UnityEngine;
using System.Collections;

namespace Barbara.Core
{
    /// <summary>
    /// Gerencia carregamento e estado do avatar 3D.
    /// </summary>
    public class AvatarManager : MonoBehaviour
    {
        [SerializeField] private Transform avatarRoot;
        [SerializeField] private GameObject defaultAvatarPrefab;

        private GameObject currentAvatar;
        private string currentAvatarId;

        private void Start()
        {
            LoadDefaultAvatar();
        }

        /// <summary>
        /// Carrega avatar padrão (placeholder).
        /// </summary>
        public void LoadDefaultAvatar()
        {
            if (currentAvatar != null)
            {
                Destroy(currentAvatar);
            }

            if (defaultAvatarPrefab != null)
            {
                currentAvatar = Instantiate(defaultAvatarPrefab, avatarRoot);
                Debug.Log("Avatar padrão carregado.");
            }
        }

        /// <summary>
        /// Solicita geração de avatar personalizado.
        /// </summary>
        public void RequestCustomAvatar(string userId, string frontImageUrl, string sideImageUrl)
        {
            StartCoroutine(GenerateAvatarCoroutine(userId, frontImageUrl, sideImageUrl));
        }

        private IEnumerator GenerateAvatarCoroutine(string userId, string frontImageUrl, string sideImageUrl)
        {
            Debug.Log("Solicitando geração de avatar...");

            yield return APIClient.Instance.GenerateAvatar(
                userId, frontImageUrl, sideImageUrl,
                onSuccess: (requestId) =>
                {
                    Debug.Log($"Avatar solicitado: {requestId}");
                    currentAvatarId = requestId;
                    StartCoroutine(PollAvatarStatus(requestId));
                },
                onError: (error) =>
                {
                    Debug.LogError($"Erro ao gerar avatar: {error}");
                }
            );
        }

        /// <summary>
        /// Polling de status do avatar até ficar pronto.
        /// </summary>
        private IEnumerator PollAvatarStatus(string avatarId)
        {
            while (true)
            {
                yield return new WaitForSeconds(2f);

                yield return APIClient.Instance.GetAvatarStatus(
                    avatarId,
                    onSuccess: (response) =>
                    {
                        Debug.Log($"Status avatar: {response.status}");
                        if (response.status == "ready" && !string.IsNullOrEmpty(response.glbUrl))
                        {
                            LoadCustomAvatar(response.glbUrl);
                            StopCoroutine(PollAvatarStatus(avatarId));
                        }
                    },
                    onError: (error) =>
                    {
                        Debug.LogError($"Erro ao verificar status: {error}");
                    }
                );
            }
        }

        /// <summary>
        /// Carrega modelo .glb customizado (requer GLTFUtility ou similar).
        /// </summary>
        private void LoadCustomAvatar(string glbUrl)
        {
            Debug.Log($"Carregando avatar de: {glbUrl}");
            // TODO: Implementar carregamento GLB via Addressables ou GLTFUtility
            // Exemplo: Importer.LoadFromUri(glbUrl, new ImportSettings(), OnLoadComplete);
        }

        /// <summary>
        /// Callback quando avatar customizado for carregado.
        /// </summary>
        private void OnLoadComplete(GameObject loadedAvatar, AnimationClip[] animations)
        {
            if (currentAvatar != null)
            {
                Destroy(currentAvatar);
            }

            currentAvatar = Instantiate(loadedAvatar, avatarRoot);
            Debug.Log("Avatar customizado carregado com sucesso!");
        }
    }
}
