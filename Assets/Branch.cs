using UnityEngine;

public class Branch : MonoBehaviour
{
    public float StartingLength = 0.0f;
    public float CurrentLength;
    public float TargetLength = 5.0f;

    public float StartingThickness = 1.0f;
    public float TargetThickness = 2.0f;

    public float GrowthRate = 1.0f;

    public bool FullyGrown;

    public GameObject BranchPrefab;

    public Transform LeftBranchRoot; 
    public Transform RightBranchRoot;

    public Branch LeftBranch;
    public Branch RightBranch;

    void Start()
    {
        CurrentLength = StartingLength;
    }

    void Update()
    {
        if (CurrentLength < TargetLength)
        {
            CurrentLength = Mathf.Min(TargetLength, CurrentLength + GrowthRate * Time.deltaTime);

            var growthLevel = CurrentLength / TargetLength;
            var thickness = TargetThickness * growthLevel;
            transform.localScale = new Vector3(thickness, CurrentLength, thickness);
        }
        else if (CurrentLength >= TargetLength && !FullyGrown)
        {
            FullyGrown = true;

            var xAxis = new Vector3(1.0f, 0.0f, 0.0f);
            var yOffset = new Vector3(0, -0.5f, 0.0f);

            var leftBranch = Instantiate(BranchPrefab, LeftBranchRoot.transform.position + yOffset, Quaternion.AngleAxis(30f, xAxis));
            var rightBranch = Instantiate(BranchPrefab, RightBranchRoot.transform.position + yOffset, Quaternion.AngleAxis(-30f, xAxis));
        }
    }
}
