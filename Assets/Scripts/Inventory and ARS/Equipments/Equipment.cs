using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Equipment : MonoBehaviour
{
    [Header("Scripts References")]

    [SerializeField]
    private ItemInteractionsSystem itemInteractionsSystem;

    [Header("Equipment Panel References")]

    [SerializeField]
    private EquipmentLibrary equipmentLibrary;

    [SerializeField]
    private Image headSlotImage;

    [SerializeField]
    private Image chestSlotImage;

    [SerializeField]
    private Image handsSlotImage;

    [SerializeField]
    private Image legsSlotImage;

    [SerializeField]
    private Image feetSlotImage;

    //Garder une trace des �quipements actuels
    [HideInInspector]
    public ItemData equipedHeadItem;
    [HideInInspector]
    public ItemData equipedChestItem;
    [HideInInspector]
    public ItemData equipedHandsItem;
    [HideInInspector]
    public ItemData equipedLegsItem;
    [HideInInspector]
    public ItemData equipedFeetItem;

    //R�f�rences aux boutons pour enlever l'�quipement port�
    [SerializeField]
    private Button headSlotRemoveEquipmentButton;

    [SerializeField]
    private Button chestSlotRemoveEquipmentButton;

    [SerializeField]
    private Button handsSlotRemoveEquipmentButton;

    [SerializeField]
    private Button legsSlotRemoveEquipmentButton;

    [SerializeField]
    private Button feetSlotRemoveEquipmentButton;

    //G�rer le remplacement d'un �quipement port� par un autre de m�me type
    private void DisablePreviousEquipedEquipment(ItemData itemToDisable)
    {
        //S�curit� : si rien, on continue
        if (itemToDisable == null)
        {
            return;
        }

        EquipmentLibraryItem equipmentLibraryItem = equipmentLibrary.content.Where(elem => elem.itemData == itemToDisable).First();

        if (equipmentLibraryItem != null)
        {
            equipmentLibraryItem.itemPrefab.SetActive(false);
        }

        //Rajout de l'�quipement enlev� � l'inventaire
        Inventory.instance.AddItem(itemToDisable);
    }

    public void RemoveEquipment(EquipmentType equipmentType)
    {
        if (Inventory.instance.IsFull())
        {
            Debug.Log("Inventory is full. Make space !");
            return;
        }

        ItemData currentItem = null;

        switch (equipmentType)
        {
            case EquipmentType.Head:
                currentItem = equipedHeadItem;
                equipedHeadItem = null;
                headSlotImage.sprite = Inventory.instance.emptySlotVisual;
                break;

            case EquipmentType.Chest:
                currentItem = equipedChestItem;
                equipedChestItem = null;
                chestSlotImage.sprite = Inventory.instance.emptySlotVisual;
                break;

            case EquipmentType.Hands:
                currentItem = equipedHandsItem;
                equipedHandsItem = null;
                handsSlotImage.sprite = Inventory.instance.emptySlotVisual;
                break;

            case EquipmentType.Legs:
                currentItem = equipedLegsItem;
                equipedLegsItem = null;
                legsSlotImage.sprite = Inventory.instance.emptySlotVisual;
                break;

            case EquipmentType.Feet:
                currentItem = equipedFeetItem;
                equipedFeetItem = null;
                feetSlotImage.sprite = Inventory.instance.emptySlotVisual;
                break;
        }

        EquipmentLibraryItem equipmentLibraryItem = equipmentLibrary.content.Where(elem => elem.itemData == currentItem).FirstOrDefault();

        if (equipmentLibraryItem != null)
        {
            equipmentLibraryItem.itemPrefab.SetActive(false);
        }

        //Rajout de l'�quipement enlev� � l'inventaire
        if (currentItem)
        {
            Inventory.instance.AddItem(currentItem);
        }

        //Rafraichissement
        Inventory.instance.RefreshContent();
    }

    //Afficher/Effacer les boutons pour enlever l'�quipement actuellement port�
    public void UpdateRemoveEquipmentButtons()
    {
        //1 - On enl�ve tous les listeners d�j� existants pour �viter qu'ils ne se stackent et cr�ent des erreurs

        //2 - Le listener et onClick sont malgr� tout n�cessaires car Unity ne prend pas les params de type EquipmentType dans l'inspecteur

        //3 - Ici, Unity interpr�te l'existence d'un item port� comme "true", ce qui
        // rend la condition valide et permet d'afficher le bouton

        headSlotRemoveEquipmentButton.onClick.RemoveAllListeners();
        headSlotRemoveEquipmentButton.onClick.AddListener(delegate { RemoveEquipment(EquipmentType.Head); });
        headSlotRemoveEquipmentButton.gameObject.SetActive(equipedHeadItem);

        chestSlotRemoveEquipmentButton.onClick.RemoveAllListeners();
        chestSlotRemoveEquipmentButton.onClick.AddListener(delegate { RemoveEquipment(EquipmentType.Chest); });
        chestSlotRemoveEquipmentButton.gameObject.SetActive(equipedChestItem);

        handsSlotRemoveEquipmentButton.onClick.RemoveAllListeners();
        handsSlotRemoveEquipmentButton.onClick.AddListener(delegate { RemoveEquipment(EquipmentType.Hands); });
        handsSlotRemoveEquipmentButton.gameObject.SetActive(equipedHandsItem);

        legsSlotRemoveEquipmentButton.onClick.RemoveAllListeners();
        legsSlotRemoveEquipmentButton.onClick.AddListener(delegate { RemoveEquipment(EquipmentType.Legs); });
        legsSlotRemoveEquipmentButton.gameObject.SetActive(equipedLegsItem);

        feetSlotRemoveEquipmentButton.onClick.RemoveAllListeners();
        feetSlotRemoveEquipmentButton.onClick.AddListener(delegate { RemoveEquipment(EquipmentType.Feet); });
        feetSlotRemoveEquipmentButton.gameObject.SetActive(equipedFeetItem);
    }

    public void EquipAction(ItemData equipment = null)
    {
        //Permettre soit de passer un item particulier, en cas de chargement d'une sauvegarde,
        //soit de poursuivre sur la gestion de l'�quipement actuellement port� en jeu
        ItemData itemToEquip = equipment ? equipment : itemInteractionsSystem.itemCurrentlySelected;

        //Requete pour associer l'�l�ment visuel � celui actuellement s�lectionn�
        EquipmentLibraryItem equipmentLibraryItem = equipmentLibrary.content.Where(elem => elem.itemData == itemToEquip).First();

        if (equipmentLibraryItem != null)
        {
            switch (itemToEquip.equipmentType)
            {
                case EquipmentType.Head:
                    DisablePreviousEquipedEquipment(equipedHeadItem);
                    headSlotImage.sprite = itemToEquip.visual;
                    equipedHeadItem = itemToEquip;
                    break;

                case EquipmentType.Chest:
                    DisablePreviousEquipedEquipment(equipedChestItem);
                    chestSlotImage.sprite = itemToEquip.visual;
                    equipedChestItem = itemToEquip;
                    break;

                case EquipmentType.Hands:
                    DisablePreviousEquipedEquipment(equipedHandsItem);
                    handsSlotImage.sprite = itemToEquip.visual;
                    equipedHandsItem = itemToEquip;
                    break;

                case EquipmentType.Legs:
                    DisablePreviousEquipedEquipment(equipedLegsItem);
                    legsSlotImage.sprite = itemToEquip.visual;
                    equipedLegsItem = itemToEquip;
                    break;

                case EquipmentType.Feet:
                    DisablePreviousEquipedEquipment(equipedFeetItem);
                    feetSlotImage.sprite = itemToEquip.visual;
                    equipedFeetItem = itemToEquip;
                    break;
            }

            equipmentLibraryItem.itemPrefab.SetActive(true);

            //Retirer l'�quipement s�lectionn� de l'inventaire et rafraichissement
            Inventory.instance.RemoveItem(itemToEquip);

        }
        else
        {
            Debug.LogError("Equipment : " + itemToEquip.name + " doesn't exist in EquipmentLibrary");
        }

        //Fermeture de l'action panel apr�s le clic sur le bouton
        itemInteractionsSystem.CloseActionPanel();
    }

    public void LoadEquipment(ItemData[] savedEquipment)
    {
        //Effacer le contenu de l'inventaire
        Inventory.instance.ClearInventoryContent();

        //D�s�quiper tout ce qui est pr�sent actuellement
        foreach (EquipmentType type in System.Enum.GetValues(typeof(EquipmentType)))
        {
            RemoveEquipment(type);
        }

        //Chargement des equipements
        foreach(ItemData item in savedEquipment)
        {
            if(item)
            {
                EquipAction(item);
            }  
        }
    }
}
