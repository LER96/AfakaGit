using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDestruction : MonoBehaviour
{
    [SerializeField] DestructableItems destructableItem;
    [SerializeField] Item _itemDrop;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Sword")
        {
            DestroyItem();
        }
    }

    private void DestroyItem()
    {
        GameObject go = Instantiate(destructableItem._destroyedVersion, transform.position, transform.rotation);
        Destroy(this.gameObject);
        ItemDrop();
        foreach (Transform child in go.transform)
        {
            GameObject kid = child.gameObject;
            StartCoroutine(FadeAlphaToZero(kid.GetComponent<MeshRenderer>().material, 3f));
        }
        Destroy(go, 3f);
    }

    IEnumerator FadeAlphaToZero(Material mat, float duration)
    {
        Color startColor = mat.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0);

        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            mat.color = Color.Lerp(startColor, endColor, time / duration);
            yield return null;
        }
    }

    void ItemDrop()
    {
        if (Random.Range(0, 100) <= _itemDrop.dropChance)
            Instantiate(_itemDrop.prefab, transform.position, transform.rotation);
    }
}