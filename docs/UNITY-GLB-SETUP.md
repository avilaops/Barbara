# 🎮 Configuração do Unity - GLB Loader

## Instalar Siccity.GLTFUtility

### Opção 1: Via Package Manager (Git URL)

1. Abra o Unity Editor
2. Window → Package Manager
3. Clique no "+" no canto superior esquerdo
4. Selecione "Add package from git URL"
5. Cole: `https://github.com/Siccity/GLTFUtility.git`

### Opção 2: Via manifest.json

Edite `core/Packages/manifest.json` e adicione:

```json
{
  "dependencies": {
    "com.siccity.gltfutility": "https://github.com/Siccity/GLTFUtility.git",
    "com.unity.render-pipelines.universal": "14.0.11",
    ...
  }
}
```

Salve e o Unity instalará automaticamente.

### Opção 3: Manual (Asset Package)

1. Baixe o [último release](https://github.com/Siccity/GLTFUtility/releases)
2. Arraste o `.unitypackage` para o Unity
3. Importe todos os arquivos

## Configurar Scripting Define Symbol

### Via Editor UI

1. Edit → Project Settings → Player
2. Na aba "Other Settings"
3. Role até "Script Compilation"
4. Em "Scripting Define Symbols", adicione:

```
GLTF_UTILITY
```

5. Clique em "Apply"

### Via Script Editor (Alternativa)

Crie `core/Assets/Editor/DefineSymbolsSetup.cs`:

```csharp
using UnityEditor;
using System.Linq;

public class DefineSymbolsSetup
{
    [InitializeOnLoadMethod]
    private static void AddGltfDefine()
    {
        var buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
        var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup)
            .Split(';')
            .ToList();

        if (!defines.Contains("GLTF_UTILITY"))
        {
            defines.Add("GLTF_UTILITY");
            PlayerSettings.SetScriptingDefineSymbolsForGroup(
                buildTargetGroup,
                string.Join(";", defines.ToArray())
            );
            UnityEngine.Debug.Log("GLTF_UTILITY define symbol adicionado");
        }
    }
}
```

Salve e o Unity recompilará automaticamente com o símbolo definido.

## Verificar Instalação

### Teste 1: Compilação

1. Abra `core/Assets/Scripts/Utilities/GlbLoader.cs`
2. Verifique que não há erros de compilação
3. O código dentro de `#if GLTF_UTILITY` deve estar ativo (sem cinza)

### Teste 2: Runtime

Crie um script de teste:

```csharp
using UnityEngine;
using Barbara.Core;

public class TestGlbLoader : MonoBehaviour
{
    void Start()
    {
        string testUrl = "https://models.readyplayer.me/64bfa15f0e72c63d7c57c2f1.glb";
        
        GlbLoader.Instance.Load(
            testUrl,
            onSuccess: (model) => {
                Debug.Log("✅ GLB carregado com sucesso!");
                model.transform.position = Vector3.zero;
            },
            onError: (error) => {
                Debug.LogError($"❌ Erro: {error}");
            }
        );
    }
}
```

Anexe ao GameObject na cena e rode Play Mode.

## Troubleshooting

### "The type or namespace 'Siccity' could not be found"

- Verifique que o pacote foi instalado corretamente
- Window → Package Manager → verifique se "GLTFUtility" aparece

### "GLTF_UTILITY is not defined"

- Verifique Project Settings → Player → Scripting Define Symbols
- Reinicie o Unity após adicionar o símbolo

### Build WebGL falha

- Certifique-se de que GLTFUtility é compatível com WebGL
- Teste primeiro em Standalone para isolar problemas

### Modelos não carregam

1. Verifique o Console do Unity para erros
2. Teste com uma URL pública conhecida (Ready Player Me sample)
3. Verifique CORS no servidor que hospeda os GLBs
4. No WebGL, abra o Console do navegador (F12) para ver erros de rede

## Próximos Passos

1. ✅ Instale GLTFUtility via Package Manager
2. ✅ Configure GLTF_UTILITY define symbol
3. ⏳ Teste em Play Mode com um GLB de exemplo
4. ⏳ Configure `ASSETS_BASE_URL` no backend
5. ⏳ Teste o fluxo completo: gerar avatar → aguardar → carregar GLB

## Alternativas ao GLTFUtility

Se encontrar problemas, considere:

- **UnityGLTF** (Microsoft): `https://github.com/KhronosGroup/UnityGLTF.git`
- **GLTFast**: via Package Manager oficial do Unity
- **Trilib**: Asset Store (pago, mas mais robusto)

Ajuste o código em `GlbLoader.cs` conforme a biblioteca escolhida.
