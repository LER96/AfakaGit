using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [Header("Item Variables")]
    [SerializeField] private Image _itemIcon;
    [SerializeField] private TextMeshProUGUI _itemName;
    [SerializeField] private TextMeshProUGUI _stackText;
    [SerializeField] private GameObject _stackObject;

    [Header("Slot Variables")]
    [SerializeField] private Sprite _fullFrame;
   // [SerializeField] private Sprite _emptyFrame;
   // [SerializeField] private Sprite _emptyIcon;
    [SerializeField] private Image _slotFrame;

    public void SetItem(InventoryItem item)
    {
        _slotFrame = GetComponentInParent<Image>();

       

        _slotFrame.sprite = _fullFrame;
        _itemIcon.sprite = item.data.icon;

        _itemName.text = item.data.name;
        _stackText.text = item.stackSize.ToString();
    }
}
