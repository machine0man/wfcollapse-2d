using System;
using System.Collections.Generic;
using UnityEngine;

namespace Nature
{
	public class TilesDataManager : MonoBehaviour
	{
		static TilesDataManager s_Instance;

		[SerializeField] TilesData m_tilesData;

		#region Unity Methods
		void Awake()
		{
			s_Instance = this;
		}
		void Start()
		{
		
		}
		void OnDestroy()
		{
			s_Instance = null;
		}
		#endregion


		#region Static Methods
		public static TileData GetRandomTileData()
		{
			return s_Instance.m_tilesData.GetRandomTileData();
		}
		public static TileData GetTileDataByUID(int a_uID)
		{
			return s_Instance.m_tilesData.GetTileDataByUID(a_uID);
		}
		[SerializeField] List<List<TileData>> m_lstAllGridTileData;

		public static List<List<TileData>> LstAllGridTileData => s_Instance.m_lstAllGridTileData;

		public static void CreateInitialListData(int l_gridTilesCount)
		{
			s_Instance.Internal_CreateInitialListData(l_gridTilesCount);
		}
		void Internal_CreateInitialListData(int l_gridTilesCount)
		{
			m_lstAllGridTileData = new List<List<TileData>>(l_gridTilesCount);
			for (int l_gridindex = 0; l_gridindex < l_gridTilesCount; l_gridindex++)
			{
				m_lstAllGridTileData.Add(new List<TileData>());
				foreach (TileData l_tileData in m_tilesData.LstTilesData)
				{
					m_lstAllGridTileData[l_gridindex].Add(l_tileData);
				}
			}		
		}
		

		#endregion
	}
}   
