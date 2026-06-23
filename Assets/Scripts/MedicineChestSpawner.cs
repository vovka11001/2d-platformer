using UnityEngine;

public class MedicineChestSpawner : Spawner<MedicineChest>
{
    [SerializeField] private MedicineChestDetector _detector;
    
    private void OnEnable()
    {
        _detector.TriggerEntered += DestroyMedicineChest;
    }

    private void OnDisable()
    {
        _detector.TriggerEntered -= DestroyMedicineChest;
    }

    private void DestroyMedicineChest(MedicineChest medicineChest)
    {
        Destroy(medicineChest.gameObject);
    }
}
