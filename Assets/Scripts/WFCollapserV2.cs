using UnityEngine;

namespace Nature
{
    public class WFCollapserV2 : MonoBehaviour
    {
        [SerializeField] Vector2Int m_gridDimensionTileCount;
        int TilesCount => GridManager.TilesCount;
        void Start()
        {
            GridManager.CreateGrid(m_gridDimensionTileCount.x, m_gridDimensionTileCount.y);
            TilesDataManager.CreateInitialListData(TilesCount);
        }

    }
}   
