using UnityEngine;

public interface IBuilding
{
    void SetSlots();
    void SetState(int id);
    void SetBuildMode();
    void SetConnect(GameObject gameObject, bool input);
    void SetDisconnect(GameObject gameObject, bool input);

    int GetID();
    bool GetCanGenerate();
    float GetCurInput();

    void CheckConnection(GameObject gameObject);
}
