using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class DataHandler : MonoBehaviour
{
    private GameObject optionItems;
    [SerializeField] private ButtonManager _buttonPrefab;
    [SerializeField] private GameObject buttonContainer;
    [SerializeField] private List<Item> items;
    [SerializeField] private string label;

    private int current_id = 0;

    private static DataHandler instance;

    public static DataHandler Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<DataHandler>();
            }
            return instance;
        }
    }

    private async void Start() {
        items = new List<Item>();
        //LoadItems();
        await Get(label);
        CreateButtons();
    }

    //void LoadItems() {
    //    var items_obj = Resources.LoadAll("Items", typeof(Item));
     //   foreach (var item in items_obj) {
       //     items.Add(item as Item);
        //}
   // }

    void CreateButtons() {
        foreach (Item i in items) {
            ButtonManager b = Instantiate(_buttonPrefab, buttonContainer.transform);
            b.ItemId = current_id;
            b.ButtonTexture = i.itemImage;
            current_id++;
        }
    }

    public void SetOptionItems(int id) {
        optionItems = items[id].itemPrefab;
    }

    public GameObject GetOptionItems() {
        return optionItems;
    }

    public async Task Get(string label) {
        var locations = await Addressables.LoadResourceLocationsAsync(label).Task;
        foreach (var location in locations) {
            var obj = await Addressables.LoadAssetAsync<Item>(location).Task;
            items.Add(obj);
        }
    }
}
