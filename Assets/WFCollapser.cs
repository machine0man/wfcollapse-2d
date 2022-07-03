using UnityEngine;
using System;
using System.Collections.Generic;

namespace Nature
{
	public class WFCollapser : MonoBehaviour
	{
		[SerializeField] IntVec2 m_gridDimensionTileCount;

		int TilesCount => GridManager.TilesCount;



		void Start()
		{
			GridManager.CreateGrid(m_gridDimensionTileCount.x, m_gridDimensionTileCount.y);
			TilesDataManager.CreateInitialListData(TilesCount);
		}
		void RunWaveFunctionCollapse()
		{
			GridManager.ClearTiles();
			m_lstSettedTilesData.Clear();

			int l_try = 0;


			IntVec2 l_gridIndex = new IntVec2(0, 0);

			TileData l_tempTileData = GetRandomTileData();
			SetTile(l_gridIndex.x, l_gridIndex.y, l_tempTileData);
			m_lstSettedTilesData.Add(l_tempTileData);



			//first row
			for (l_gridIndex.x = 1; l_gridIndex.x < GridManager.GridCountX; l_gridIndex.x++)
			{
				if (GetFeasibleTileData(l_gridIndex, out TileData l_newTileData, true))
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
					if (GetFeasibleTileData(l_gridIndex, out TileData l_newTileData, false))
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
			}
		}
		void RunWaveFunctionCollapseReversed()
		{
			GridManager.ClearTiles();
			m_lstSettedTilesData.Clear();

			


			IntVec2 l_gridIndex = new IntVec2(GridManager.GridCountX-1, GridManager.GridCountY - 1);

			TileData l_tempTileData = GetRandomTileData();
			SetTile(l_gridIndex.x, l_gridIndex.y, l_tempTileData);
			m_lstSettedTilesData.Add(l_tempTileData);



			//first row
			for (l_gridIndex.x = GridManager.GridCountX - 2; l_gridIndex.x >= 0; l_gridIndex.x--)
			{
				if (GetFeasibleTileDataReversed(l_gridIndex, out TileData l_newTileData, true))
				{
					if (l_newTileData == null)
					{
						break;
					}
					SetTile( (GridManager.GridCountX-1)- l_gridIndex.x, (GridManager.GridCountY - 1) - l_gridIndex.y, l_newTileData);
					m_lstSettedTilesData.Add(l_newTileData);
				}
				else
					return;
			}

			return;

			//rest of the rows
			for (l_gridIndex.y = GridManager.GridCountY - 1; l_gridIndex.y >= 0; l_gridIndex.y--)
			{
				for (l_gridIndex.x = GridManager.GridCountX - 2; l_gridIndex.x >= 0; l_gridIndex.x--)
				{
					if (GetFeasibleTileDataReversed(l_gridIndex, out TileData l_newTileData, false))
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
			}
		}

		List<TileData> m_lstSettedTilesData = new List<TileData>();
		TileData GetTileDataAt(IntVec2 a_gridTileIndex)
		{
			int l_index = a_gridTileIndex.y * GridManager.GridCountX + a_gridTileIndex.x;
			return m_lstSettedTilesData[l_index];
		}
		bool GetFeasibleTileData(IntVec2 a_tileIndex,  out TileData a_newTileData, bool a_isTopRow)
		{
			TileData l_leftTileData = GetTileDataAt(new IntVec2 (a_tileIndex.x-1 , a_tileIndex.y));


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
				TileData l_upperTileData = GetTileDataAt(new IntVec2(a_tileIndex.x, a_tileIndex.y - 1));
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
		bool GetFeasibleTileDataReversed(IntVec2 a_tileIndex,  out TileData a_newTileData, bool a_isBottomRow)
		{
			TileData l_rightTileData = GetTileDataAt(new IntVec2 ((GridManager.GridCountX - 1) - a_tileIndex.x+1  , (GridManager.GridCountY - 1) - a_tileIndex.y ));


			if (a_isBottomRow)
			{
				if (l_rightTileData.LstLeft.Count > 0)
				{
					int l_randomIndex = UnityEngine.Random.Range(0, l_rightTileData.LstLeft.Count);
					a_newTileData = GetTileDataByUID(l_rightTileData.LstRight[l_randomIndex]);
					return true;
				}
			}
			else 
			{
				a_newTileData = null;
				return false;

				//extra check if not first row (top-row)
				TileData l_bottomTileData = GetTileDataAt(new IntVec2(a_tileIndex.x, a_tileIndex.y + 1));
				if (a_tileIndex.x == GridManager.GridCountX-1)
				{
					if (l_bottomTileData.LstTop.Count > 0)
					{
						int l_randomIndex = UnityEngine.Random.Range(0, l_bottomTileData.LstTop.Count);
						a_newTileData = GetTileDataByUID(l_bottomTileData.LstTop[l_randomIndex]);

						return true;
					}
				}
				else 
				{
					for (int l_indexTopLst = 0; l_indexTopLst < l_bottomTileData.LstBottom.Count; l_indexTopLst++)
					{
						int l_topUId = l_bottomTileData.LstBottom[l_indexTopLst];

						foreach (int l_uID in l_rightTileData.LstRight)
						{
							if (l_uID == l_topUId)
							{
								a_newTileData = GetTileDataByUID(l_topUId);
								return true;
							}
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
			//RunWaveFunctionCollapseReversed();
		}
		#endregion

	}


	[Serializable]
	public class IntVec2
	{
		public int x;
		public int y;

		public IntVec2(int a_x, int a_y)
		{
			x = a_x;
			y = a_y;
		}
	}
}   
