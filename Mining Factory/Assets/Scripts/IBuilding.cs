using UnityEngine;

public interface IBuilding
{
    void SetSlots();
    void SetState();
    void SetBuildMode();
    void SetConnect(BuildingInfo buildingInfo, bool input, GameObject otherObject);
    void SetDisconnect(BuildingInfo buildingInfo, bool input, GameObject otherObject);
    void SetBuildingInfo(BuildingInfo buildingInfo);

    string GetID();
    bool GetCanGenerate();
    int GetCurInput();
    BuildingInfo GetBuildingInfo();

    void CheckConnection(BuildingInfo buildingInfo);
}
