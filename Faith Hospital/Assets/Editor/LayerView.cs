using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

public class LayerView : EditorWindow
{
    private Toggle _layer1Toggle;
    private Toggle _layer2Toggle;
    private Toggle _layer3Toggle;
    
    private GameObject[] layer1Objects;
    private GameObject[] layer2Objects;
    private GameObject[] layer3Objects;
    
    [MenuItem("Tools/Layer View")]
    public static void ShowWindow()
    {
        GetWindow<LayerView>("Layer View");
    }

    private void CreateGUI()
    {
        layer1Objects = GameObject.FindGameObjectsWithTag("Layer1");
        layer2Objects = GameObject.FindGameObjectsWithTag("Layer2");
        layer3Objects = GameObject.FindGameObjectsWithTag("Layer3");
        
        VisualElement root = rootVisualElement;
        Label label = new Label("Choose which layer to view");
        root.Add(label);

        _layer1Toggle = new Toggle("Layer 1");
        _layer1Toggle.value = true; 
        _layer1Toggle.RegisterValueChangedCallback(OnLayer1Toggled);
        root.Add(_layer1Toggle);
        
        _layer2Toggle = new Toggle("Layer 2");
        _layer2Toggle.value = true; 
        _layer2Toggle.RegisterValueChangedCallback(OnLayer2Toggled);
        root.Add(_layer2Toggle);
        
        _layer3Toggle = new Toggle("Layer 3");
        _layer3Toggle.value = true; 
        _layer3Toggle.RegisterValueChangedCallback(OnLayer3Toggled);
        root.Add(_layer3Toggle);
    }

    private void OnLayer1Toggled(ChangeEvent<bool> evt)
    {
        foreach (GameObject obj in layer1Objects)
        {
            obj.SetActive(evt.newValue);
        }
    }
    
    private void OnLayer2Toggled(ChangeEvent<bool> evt)
    {
        foreach (GameObject obj in layer1Objects)
        {
            obj.SetActive(evt.newValue);
        }
    }
    
    private void OnLayer3Toggled(ChangeEvent<bool> evt)
    {
        foreach (GameObject obj in layer1Objects)
        {
            obj.SetActive(evt.newValue);
        }
    }
}