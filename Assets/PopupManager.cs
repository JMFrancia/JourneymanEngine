using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    [SerializeField] PopupController _poiPopupController;

    List<PopupController> _popupControllers;

    private void Awake()
    {
        _popupControllers = new List<PopupController> {
            _poiPopupController
        };

        HideAll();

        EventManager.StartListeningClass(Constants.Events.DESTINATION_REACHED, OnDestinationReached);
    }

    public void HideAll() {
        for (int n = 0; n < _popupControllers.Count; n++) {
            _popupControllers[n].Hide();
        }
    }

    void OnDestinationReached(GameObject POIObj) {
        _poiPopupController.Show(POIObj.GetComponent<POIManager>().Data);
    }

    
}
