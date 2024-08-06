using UnityEngine;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nature
{
	public class WFCollapser : MonoBehaviour
	{
		[SerializeField] Vector2Int m_gridDimensionTileCount;

		int TilesCount => GridManager.TilesCount;



		void Start()
		{
			GridManager.CreateGrid(m_gridDimensionTileCount.x, m_gridDimensionTileCount.y);
			TilesDataManager.CreateInitialListData(TilesCount);
		}
		async void RunWaveFunctionCollapse()
		{
			GridManager.ClearTiles();
			m_lstSettedTilesData.Clear();


			//fill first cell of grid
			Vector2Int l_gridIndex = new Vector2Int(0, 0);

			TileData l_tempTileData = GetRandomTileData();
			SetTile(l_gridIndex.x, l_gridIndex.y, l_tempTileData);
			m_lstSettedTilesData.Add(l_tempTileData);



			//first row
			for (l_gridIndex.x = 1; l_gridIndex.x < GridManager.GridCountX; l_gridIndex.x++)
			{
				if (TryGetFeasibleTileData(l_gridIndex, out TileData l_newTileData, true))
				{
					if (l_newTileData == null)
					{
						break;
					}
					SetTile(l_gridIndex.x, l_gridIndex.y, l_newTileData);
					m_lstSettedTilesData.Add(l_newTileData);
				}
				else
					return;
			}

			

			//rest of the rows
			for (l_gridIndex.y = 1; l_gridIndex.y < GridManager.GridCountY; l_gridIndex.y++)
			{
				for (l_gridIndex.x = 0; l_gridIndex.x < GridManager.GridCountX; l_gridIndex.x++)
				{
					if (!await TrySetTileData(l_gridIndex))
						return;
				}
			}

			async Task<bool> TrySetTileData(Vector2Int a_gridIndex)
			{
				await Task.Delay(10);
				if (TryGetFeasibleTileData(a_gridIndex, out TileData l_newTileData, false))
				{
					if (l_newTileData == null)
					{
						return false;
					}
					SetTile(l_gridIndex.x, l_gridIndex.y, l_newTileData);
					m_lstSettedTilesData.Add(l_newTileData);
				}
				return true;
			}
		}

		List<TileData> m_lstSettedTilesData = new List<TileData>();
		TileData GetTileDataAt(Vector2Int a_gridTileIndex)
		{
			int l_index = a_gridTileIndex.y * GridManager.GridCountX + a_gridTileIndex.x;
			return m_lstSettedTilesData[l_index];
		}
		bool TryGetFeasibleTileData(Vector2Int a_tileIndex,  out TileData a_newTileData, bool a_isTopRow)
		{
			TileData l_leftTileData = GetTileDataAt(new Vector2Int (a_tileIndex.x-1 , a_tileIndex.y));

			if (a_isTopRow)
			{
				if (l_leftTileData.LstRight.Count > 0)
				{
					int l_randomIndex = UnityEngine.Random.Range(0, l_leftTileData.LstRight.Count);
					a_newTileData = GetTileDataByUID(l_leftTileData.LstRight[l_randomIndex]);
					return true;
				}
			}
			else 
			{
				//extra check if not first row (top-row)
				TileData l_upperTileData = GetTileDataAt(new Vector2Int(a_tileIndex.x, a_tileIndex.y - 1));
				if (a_tileIndex.x == 0)
				{
					if (l_upperTileData.LstBottom.Count > 0)
					{
						int l_randomIndex = UnityEngine.Random.Range(0, l_upperTileData.LstBottom.Count);
						a_newTileData = GetTileDataByUID(l_upperTileData.LstBottom[l_randomIndex]);

						return true;
					}
				}
				else 
				{
					for (int l_indexBottomLst = 0; l_indexBottomLst < l_upperTileData.LstBottom.Count; l_indexBottomLst++)
					{
						int l_bottommUId = l_upperTileData.LstBottom[l_indexBottomLst];

						List<int> l_lstCommonUID = new List<int>();

						foreach (int l_uID in l_leftTileData.LstRight)
						{
							if (l_uID == l_bottommUId)
							{
								l_lstCommonUID.Add(l_bottommUId);
							}
						}

						if (l_lstCommonUID.Count > 0)
						{
							int l_random = UnityEngine.Random.Range(0 , l_lstCommonUID.Count);
							a_newTileData = GetTileDataByUID(l_lstCommonUID[l_random]);
							return true;
						}
					}
				}
			}

			a_newTileData = null;
			return false;
		}
		


		void SetTile(int a_gridIndexX , int a_gridIndexY , TileData a_tileData)
		{
			GridManager.SetTile(a_gridIndexX,a_gridIndexY , a_tileData); 
		}
		TileData GetRandomTileData()
		{
			return TilesDataManager.GetRandomTileData();
		}
		TileData GetTileDataByUID(int a_uID)
		{
			return TilesDataManager.GetTileDataByUID(a_uID);
		}

		#region Public Methods
		public void Run()
		{
			RunWaveFunctionCollapse();
		}
		#endregion

	}
	
}   
