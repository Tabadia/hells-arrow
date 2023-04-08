using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public enum ColorblindTypes
{
    Normal = 0,
    Protanopia,
    Protanomaly,
    Deuteranopia,
    Deuteranomaly,
    Tritanopia,
    Tritanomaly,
    Achromatopsia,
    Achromatomaly,
}

public class Colorblindness : MonoBehaviour
{
    private int currentType;

    Volume[] volumes;
    VolumeComponent lastFilter;

    void SearchVolumes() => volumes = GameObject.FindObjectsOfType<Volume>();

    public static Colorblindness Instance { get; private set; }

    void Start()
    {
        if (PlayerPrefs.HasKey("Accessibility.ColorblindType"))
            currentType = PlayerPrefs.GetInt("Accessibility.ColorblindType");
        else
            PlayerPrefs.SetInt("Accessibility.ColorblindType", 0);

        SearchVolumes();
        StartCoroutine(ApplyFilter(currentType));
    }

    // StartCoroutine(ApplyFilter());
    private IEnumerator ApplyFilter(int _currentType) {
        print("Applying filter");
        PlayerPrefs.SetInt("Accessibility.ColorblindType", _currentType);
        ResourceRequest loadRequest = Resources.LoadAsync<VolumeProfile>($"Colorblind/{(ColorblindTypes)_currentType}");

        do yield return null; while (!loadRequest.isDone);

        var filter = loadRequest.asset as VolumeProfile;

        if (filter == null)
        {
            Debug.LogError("An error has occured! Please, report");
            yield break;
        }

        if (lastFilter != null)
        {
            foreach (var volume in volumes)
            {
                volume.profile.components.Remove(lastFilter);

                foreach (var component in filter.components)
                    volume.profile.components.Add(component);
            }
        }

        lastFilter = filter.components[0];
    }

    public void SetFilter(int currentType) {
        StartCoroutine(ApplyFilter(currentType));
    }
}