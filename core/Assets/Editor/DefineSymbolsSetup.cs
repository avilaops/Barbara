using UnityEditor;
using System.Linq;

namespace Barbara.Core.Editor
{
    /// <summary>
    /// Configura automaticamente o símbolo GLTF_UTILITY ao carregar o editor.
    /// </summary>
    public static class DefineSymbolsSetup
    {
        [InitializeOnLoadMethod]
        private static void AddGltfDefine()
        {
            var buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            var currentDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            var defines = currentDefines.Split(';').ToList();

            const string gltfSymbol = "GLTF_UTILITY";

            if (!defines.Contains(gltfSymbol))
            {
                defines.Add(gltfSymbol);
                var newDefines = string.Join(";", defines.Where(d => !string.IsNullOrWhiteSpace(d)).ToArray());
                PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, newDefines);
                UnityEngine.Debug.Log($"[Barbara] Símbolo '{gltfSymbol}' adicionado automaticamente");
            }
        }
    }
}
