using System.Collections;
using UnityEngine;

public class GhostShadowEffect : MonoBehaviour
{
    [Tooltip("Ghost Shadow duration")]
    public float liveTime = 2f;
    public float refreshRate = 0.05f;
    public float meshDestorytime = 3f;
    public Transform shadowPosition;
    [SerializeField] KeyCode shortCutKey = KeyCode.LeftShift;

    [Header("Shader Setting")]
    [SerializeField] Shader ghostShader;
    [SerializeField] Material ghostMaterial;
    [SerializeField] string EmissionIntensityRef = "_EimissionIntensity";
    [SerializeField] float emissionIntensity = 2f;
    [SerializeField] string fadeOutRef = "_Alpha";
    [SerializeField] float fadeRate = 0.1f;
    [SerializeField] float fadeRefreshRate = 0.05f;
    [Space(10)]
    public bool isActive = false;

    [SerializeField] SkinnedMeshRenderer[] skinnedMeshRenderers;

    private void Awake()
    {
        if (skinnedMeshRenderers == null)
        {
            skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        }
    }
    void Update()
    {
        ActiveGhostShadow();
    }

    void ActiveGhostShadow()
    {
        if (Input.GetKeyDown(shortCutKey) && !isActive)
        {
            isActive = true;
            StartCoroutine(ActiveShadow(liveTime));
        }
    }
    IEnumerator ActiveShadow(float liveTime)
    {
        while (liveTime > 0)
        {
            liveTime -= refreshRate;
            foreach (var skinMesh in skinnedMeshRenderers)
            {
                GameObject obj = new GameObject("Ghost");
                obj.transform.SetPositionAndRotation(shadowPosition.position, shadowPosition.rotation);
                MeshRenderer mr = obj.AddComponent<MeshRenderer>();
                Material mat = new Material(ghostMaterial);
                MeshFilter mf = obj.AddComponent<MeshFilter>();
                Mesh mesh = new Mesh();
                skinMesh.BakeMesh(mesh);
                Material[] materials = skinMesh.materials;
                mat.enableInstancing = true;
                mat.SetFloat(EmissionIntensityRef, emissionIntensity);
                mat.SetFloat("_ColorTime", Remap(liveTime, 0, this.liveTime, 0, 1));
                mf.mesh = mesh;
                for (int i = 0; i < skinMesh.materials.Length; i++)
                {
                    materials[i] = mat;
                }
                mr.materials = materials;
                StartCoroutine(GhostFadeOut(mr.materials, 0, fadeRate, fadeRefreshRate));
                Destroy(obj, meshDestorytime);
            }
            yield return new WaitForSeconds(refreshRate);
        }
        isActive = false;
    }

    IEnumerator GhostFadeOut(Material[] mats, float goal, float rate, float refreshRate)
    {
        float fadeValue = mats[0].GetFloat(fadeOutRef);
        while (fadeValue > goal)
        {
            fadeValue -= rate;
            foreach (var mat in mats)
            {
                mat.SetFloat(fadeOutRef, fadeValue);
            }
            yield return new WaitForSeconds(refreshRate);
        }
    }
    float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
