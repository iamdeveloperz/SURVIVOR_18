using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;
using static UnityEditor.Progress;

[Serializable]
public class ItemSlot
{
    public ItemData item;
    public int quantity = 0;

    public ItemSlot(ItemData item,int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }

    public ItemSlot()
    {
        this.item = null;
        this.quantity = 0;
    }
}
public class Inventory : MonoBehaviour
{
    private static int _maxCapacity = 24;

    [SerializeField] private GameObject go_SlotsParent;  // Slot���� �θ��� Grid Setting 
    [SerializeField] private Slot[] slots;  // ���Ե� �迭
    public GameObject slotPrefab;

    public List<ItemSlot> items; // Do Delete
    public ItemSlot[] itemSlots;

    public static Inventory Instance;

    public GameObject itemName;  
    public GameObject itemInfo;
    public GameObject itemHp;
    public GameObject itemWater;
    public GameObject itemClear;
    public PlayerStatHandler playerStatHandler;
    public Slot curSlot;

    private void Awake()
    {
        Debug.Log("inventory awake");
        Instance = this;

        slots = go_SlotsParent.GetComponentsInChildren<Slot>();
        for (int i = 0; i < slots.Length; ++i)
        {
            slots[i].index = i;
        }
        itemSlots = new ItemSlot[_maxCapacity];
        for(int i = 0; i < itemSlots.Length; ++i)
        {
            itemSlots[i] = new ItemSlot();
        }
    }
    
    private void OnEnable()
    {
        Debug.Log("inventory OnEnable");
        //Button itemClearBtn = itemClear.GetComponent<Button>();
        //itemClearBtn.onClick.AddListener(ClearBtn);
    }

    void Start()
    {
        Debug.Log("inventory Start");
        gameObject.SetActive(false);
    }

    public void ClearBtn()
    {
        curSlot.ClearSlot();
    }

    private void OnDisable()
    {
        //if (itemName.activeSelf)
        //{
        //    //TextClose();
        //}
    }
    public void TextClose()
    {
        itemName.SetActive(false);
        itemInfo.SetActive(false);
        itemHp.SetActive(false);
        itemWater.SetActive(false);
        itemClear.SetActive(false);
    }
    public void itemText(string itemNameText, string itemInfoText, ItemType type = ItemType.equipItem, float hpText = 0, float waterText = 0)
    {
        itemName.SetActive(true);
        itemInfo.SetActive(true);
        itemClear.SetActive(true);
        itemName.GetComponent<TextMeshProUGUI>().text = itemNameText;
        itemInfo.GetComponent<TextMeshProUGUI>().text = itemInfoText;
        if (type == ItemType.useItem)
        {
            itemHp.SetActive(true);
            itemWater.SetActive(true);
            itemHp.GetComponent<TextMeshProUGUI>().text = $"체력 : + {hpText}";
            itemWater.GetComponent<TextMeshProUGUI>().text = $"수분 : + {waterText}";
        }
    }

    public void AddItem(ItemData item)
    {
        int emptyIndex = FindEmptySlot();
        if (emptyIndex == -1) return;

        if(item.canStack)
        {
            for(int i = 0; i < itemSlots.Length; ++i)
            { 
                if (itemSlots[i].item == item && itemSlots[i].quantity != itemSlots[i].item.maxStackCount)
                {
                    itemSlots[i].quantity += 1;
                    slots[i].SetItemSlot(itemSlots[i]);
                    return;
                }
            }            
        }

        itemSlots[emptyIndex].item = item;
        itemSlots[emptyIndex].quantity = 1;
        slots[emptyIndex].SetItemSlot(itemSlots[emptyIndex]);
    }

    private int FindEmptySlot()
    {
        for(int i = 0; i < itemSlots.Length; ++i)
        {
            if (itemSlots[i].quantity == 0)
                return i;
        }
        return -1;
    }

    public void SwitchingItemSlot(int sourceIndex, int targetIndex)
    {
        var itemSlot = itemSlots[sourceIndex];
        itemSlots[sourceIndex] = itemSlots[targetIndex];
        itemSlots[targetIndex] = itemSlot;

        slots[sourceIndex].SetItemSlot(itemSlots[sourceIndex]);
        slots[targetIndex].SetItemSlot(itemSlots[targetIndex]);
    }

    public void SubtractItem(ItemData item, int num)
    {
        for (int i = 0; i < items.Count; i++)
            if (items[i].item.name == item.name)
                slots[i].Consumeitem(num);
    }
}
