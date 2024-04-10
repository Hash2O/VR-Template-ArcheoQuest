using UnityEngine;

public class ItemInteractionsSystem : MonoBehaviour
{
    [Header("Scripts References")]

    [SerializeField]
    private Equipment equipment;

    //Gestion de l'affichage du panel des actions en fonction de l'item s�lectionn�
    [Header("Action Panel References")]

    public GameObject actionPanel;

    [SerializeField]
    private Transform dropPoint;

    [SerializeField]
    private GameObject useItemButton;

    [SerializeField]
    private GameObject equipItemButton;

    [SerializeField]
    private GameObject dropItemButton;

    [SerializeField]
    private GameObject destroyItemButton;

    //Variable pour donner au bouton les infos sur l'item actuellement s�lectionn�
    //dans l'inventaire (HideInInspector car dynamique et ne devant pas �tre utilis� dans l'inspecteur)
    [HideInInspector]
    public ItemData itemCurrentlySelected; 

    //Affichage dynamique des boutons de l'action panel en fonction du type d'item
    public void OpenActionPanel(ItemData item, Vector3 slotPosition)
    {
        //Initialisation de la variable pour d�terminer l'item s�lectionn�
        itemCurrentlySelected = item;

        //S�curit� en cas de slot vide
        if (item == null)
        {
            actionPanel.SetActive(false);
            return;
        }

        switch (item.itemType)
        {
            case ItemType.Ressource:
                useItemButton.SetActive(false);
                equipItemButton.SetActive(false);
                break;
            case ItemType.Equipment:
                useItemButton.SetActive(false);
                equipItemButton.SetActive(true);
                break;
            case ItemType.Consumable:
                useItemButton.SetActive(true);
                equipItemButton.SetActive(false);
                break;

        }
        //Positionnement de 'action panel avant ouverture
        actionPanel.transform.position = slotPosition;
        actionPanel.SetActive(true);
    }

    //D�sactiver l'action panel
    public void CloseActionPanel()
    {
        actionPanel.SetActive(false);
        itemCurrentlySelected = null;
    }

    //Gestion des actions li�es aux boutons de l'action panel
    public void UseActionButton()
    {
        //Fermeture de l'action panel apr�s le clic sur le bouton
        CloseActionPanel();

    }

    public void EquipActionButton()
    {
        equipment.EquipAction();
    }

    public void DropActionButton()
    {
        //Instantiation de l'objet � dropper, en fct des donn�es de l'item s�lectionn�
        GameObject instantiatedItem = Instantiate(itemCurrentlySelected.prefab);
        //Positionnement du GO instanci� sur le drop point choisi
        instantiatedItem.transform.position = dropPoint.position;
        //Item retir� de la liste
        Inventory.instance.RemoveItem(itemCurrentlySelected);
        //Rafraichissement 
        Inventory.instance.RefreshContent();
        //Fermeture de l'action panel apr�s le clic sur le bouton
        CloseActionPanel();
    }

    public void DestroyActionButton()
    {
        //Item supprim� de la liste
        Inventory.instance.RemoveItem(itemCurrentlySelected);
        //Rafraichissement 
        Inventory.instance.RefreshContent();
        //Fermeture de l'action panel apr�s le clic sur le bouton
        CloseActionPanel();
    }

}
