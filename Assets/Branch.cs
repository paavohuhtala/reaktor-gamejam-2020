using UnityEngine;

public enum BranchType
{
    Main,
    InnerBranch,
    TerminalBranch
}

public enum Side
{
    Center,
    Left,
    Right
}

public class Branch : MonoBehaviour
{
    public const int MaxDepth = 8;
    public const int InnerMaxDepth = 2;
    public int Depth = 0;
    public int InnerDepth = 0;

    public float StartingLength = 0.0f;
    public float CurrentLength;
    public float TargetLength = 5.0f;

    public Side Side = Side.Center;
    public BranchType Type = BranchType.Main;

    public float StartingThickness = 1.0f;
    public float TargetThickness = 2.0f;

    public float GrowthRate = 1.0f;

    public float BranchAngle = 30.0f;

    public bool FullyGrown;

    public Transform Trunk;
    public Transform Branches;

    public Transform LeftBranchRoot; 
    public Transform RightBranchRoot;

    private GameObject treePrefab;

    void Start()
    {
        treePrefab = Resources.Load<GameObject>("Prefabs/TreeBranch");
        CurrentLength = StartingLength;
        UpdateScale();
    }

    void UpdateScale()
    {
        var growthLevel = CurrentLength / TargetLength;
        var thickness = TargetThickness * growthLevel;
        Trunk.localScale = new Vector3(thickness, CurrentLength, thickness);
    }

    float ComputeBranchAngle(Side side)
    {
        switch (side)
        {
            case Side.Left: return BranchAngle;
            case Side.Right: return -BranchAngle;
            default:
            case Side.Center: return 0;
        }
    }

    Branch CreateBranch(Vector3 root, Side branchSide)
    {
        if (Type == BranchType.InnerBranch && InnerDepth >= InnerMaxDepth)
        {
            return null;
        }

        var xAxis = new Vector3(1.0f, 0.0f, 0.0f);
        var yOffset = new Vector3(0, -0.5f, 0.0f);

        var angle = ComputeBranchAngle(branchSide);

        var branchGameObject = Instantiate(
            treePrefab,
            root + yOffset,
            Quaternion.AngleAxis(angle, xAxis),
            Branches
        );

        var branch = branchGameObject.GetComponent<Branch>();

        branch.Depth = Depth + 1;
        branch.TargetThickness = TargetThickness * 0.75f;
        branch.TargetLength = TargetLength * 0.85f;
        branch.Side = branchSide;

        if (Side == Side.Center || (Type == BranchType.Main && branchSide == Side))
        {
            branch.Type = BranchType.Main;
        }
        else if (Type == BranchType.InnerBranch || Side != branchSide)
        {
            branch.Type = BranchType.InnerBranch;
            branch.InnerDepth = InnerDepth + 1;
        }

        branch.BranchAngle = BranchAngle + 20;

        return branch;
    }

    void Update()
    {
        if (CurrentLength < TargetLength)
        {
            CurrentLength = Mathf.Min(TargetLength, CurrentLength + GrowthRate * Time.deltaTime);
            UpdateScale();

        }

        if (CurrentLength >= TargetLength && !FullyGrown && Depth < MaxDepth)
        {
            FullyGrown = true;

            CreateBranch(LeftBranchRoot.position, Side.Left);
            CreateBranch(RightBranchRoot.position, Side.Right);
        }
    }
}
