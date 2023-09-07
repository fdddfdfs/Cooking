using System;
using UnityEngine;
using DG.Tweening;
using System.Collections;

public class Plant : ITargettable
{
    public const float PlantLittleScale = 0.6f;
    public const float AnimationTime = 0.6f;

    private IPlantObjects _plantObjects;
    private LastStagePlant _lastStagePlant;

    public string Name => CurrentPlant.PlantSample.Name;
    
    public GameObject Attached { get; private set; }
    
    public int CurrentGrowthStage { get; private set; }

    public SeedShopItemData CurrentPlant { get; private set; }

    public Vector3 Position => Attached != null ? 
        Attached.transform.position : throw new System.Exception("plant not initialized");

    public bool IsFullGrown => CurrentGrowthStage == CurrentPlant.PlantSample.GrowthStageAmount - 1;

    public Plant(IPlantObjects plantObjects)
    {
        _plantObjects = plantObjects;
    }
    
    public Plant(SeedShopItemData seedShopItemData, int growthStage, float grownTime, IPlantObjects plantObjects)
    {
        _plantObjects = plantObjects;
        CurrentGrowthStage = growthStage;
        CurrentPlant = seedShopItemData;
        UpdateGameObject();
        StartNewGrow(grownTime);
    }

    public void SetNewPlant(SeedShopItemData seedShopItemData, int growthStage, float grownTime, Vector3 position)
    {
        if (Attached)
        {
            ClearPlant();
        }
        
        CurrentGrowthStage = growthStage;
        CurrentPlant = seedShopItemData;
        UpdateGameObject();
        ChangeAttachedPosition(position);
        StartNewGrow(grownTime);
    }
    
    public void ClearPlant()
    {
        CurrentPlant = null;
        Attached.SetActive(false);
    }

    public void HighlightLastStagePlant(Material highlightMaterial)
    {
        _lastStagePlant.ChangeMaterial(highlightMaterial);
    }

    public void DeHighlightLastStagePlant()
    {
        _lastStagePlant.ChangeMaterial(null);
    }

    private void ChangeAttachedPosition(Vector3 position)
    {
        if (Attached != null)
            Attached.transform.position = position;
    }

    private void Grow()
    {
        CurrentGrowthStage++;
        UpdateGameObject();
        StartGrowAnimation();
        StartNewGrow(0);
    }

    private void UpdateGameObject()
    {
        GameObject grownPlant = _plantObjects.TakeItem(this);

        if (Attached != null)
        {
            Attached.SetActive(false);
            grownPlant.transform.position = Attached.transform.position;
        }
            
        grownPlant.SetActive(true);
        Attached = grownPlant;
    }

    private void StartGrowAnimation()
    {
        if (Attached != null )
        {
            Attached.transform.localScale = PlantLittleScale * Vector3.one;
            Attached.transform.DOScale(Vector3.one, AnimationTime).SetEase(Ease.OutBounce);
        }   
    }

    private void StartNewGrow(float timeGrown)
    {
        if (CurrentGrowthStage < CurrentPlant.PlantSample.GrowthStageAmount - 1)
            Coroutines.StartRoutine(WaitForGrow(CurrentPlant.PlantSample.GrowTime - timeGrown));
        else
        {
            _lastStagePlant = Attached.GetComponent<LastStagePlant>();

            if (!_lastStagePlant)
            {
                throw new Exception("Last stage plant dont have LastStagePlant component");
            }
        }
    }

    private IEnumerator WaitForGrow(float time)
    {
        yield return new WaitForSeconds(time);
        Grow();
    }

    public void TakeDamage(int damage)
    {
        //Debug.Log(Name + " taking damage");
    }
}