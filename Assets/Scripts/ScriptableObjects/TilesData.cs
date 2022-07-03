using System.Collections.Generic;
using UnityEngine;

namespace Nature
{
	[CreateAssetMenu(fileName = "TilesData", menuName = "ScriptableObjects/TilesData", order = 1)]
	public class TilesData : ScriptableObject
	{
		[SerializeField] List<TileData> m_lstTilesData;


		public IReadOnlyList<TileData> LstTilesData => m_lstTilesData.AsReadOnly();
		public TileData GetRandomTileData()
		{
			int l_randInt = Random.Range(0, m_lstTilesData.Count);
			return m_lstTilesData[l_randInt];
		}
		public TileData GetTileDataByUID(int a_uID)
		{
			return m_lstTilesData.Find(l_tileData => l_tileData.UId == a_uID);
		}
	}
}