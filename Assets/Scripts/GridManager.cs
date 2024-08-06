using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Nature
{
	public class GridManager : MonoBehaviour
	{
		static GridManager s_Instance;

		[SerializeField] GridLayoutGroup m_gridLayoutGrpTiles;
		[SerializeField] GameObject m_prefTile;
		[SerializeField] RectTransform m_transTilesHolder;
		[SerializeField] Vector2 m_tileSizeToSet;
		[SerializeField] Vector2 m_tileSpacing;

		List<Image> m_lstTileImages = new List<Image>();
		int m_gridCountX;
		int m_gridCountY;

		public static int GridCountX  => s_Instance.m_gridCountX;
		public static int GridCountY  => s_Instance.m_gridCountY;
		public static int TilesCount => s_Instance.m_lstTileImages.Count;

		#region Unity Methods
		void Awake()
		{
			s_Instance = this;
		}
		void OnDestroy()
		{
			s_Instance = null;
		}
		#endregion

		#region Static Methods
		public static void CreateGrid(int a_countX, int a_countY)
		{
			s_Instance.m_gridCountX = a_countX;
			s_Instance.m_gridCountY = a_countY;
			s_Instance.Internal_CreateGrid(a_countX, a_countY);
		}
		public static void SetTile(int a_indexX, int a_indexY, TileData a_tileData)
		{
			s_Instance.Internal_SetTileData(a_indexX, a_indexY, a_tileData);
		}
		#endregion


		void Internal_CreateGrid(int a_width, int a_height)
		{
			m_gridLayoutGrpTiles.cellSize = m_tileSizeToSet;
			m_gridLayoutGrpTiles.spacing = m_tileSpacing;

			m_gridLayoutGrpTiles.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
			m_gridLayoutGrpTiles.constraintCount = a_width;

			Vector2 l_gridDimensions;

			l_gridDimensions.x = a_width * m_tileSizeToSet.x + ((a_width - 1) * m_tileSpacing.x);
			l_gridDimensions.y = a_height * m_tileSizeToSet.y + ((a_height - 1) * m_tileSpacing.y);

			m_transTilesHolder.sizeDelta = l_gridDimensions;

			m_lstTileImages.Clear();
			for (int l_indexTile = 0; l_indexTile < a_width * a_height; l_indexTile++)
			{
				Image l_tileImage = Instantiate(m_prefTile, m_transTilesHolder).GetComponent<Image>();
				m_lstTileImages.Add(l_tileImage);
			}
		}

		void Internal_SetTileData(int a_indexX, int a_indexY, TileData a_tileData)
		{
			Image l_tileImage = m_lstTileImages[a_indexY * m_gridCountX + a_indexX].transform.GetChild(0).GetComponent<Image>();

			ShowTile(a_indexX , a_indexY,true);
			l_tileImage.sprite = a_tileData.SpriteTile;
		}
		void ShowTile(int a_indexX, int a_indexY , bool a_showState)
		{
			Image l_tileImage = m_lstTileImages[a_indexY * m_gridCountX + a_indexX].transform.GetChild(0).GetComponent<Image>();
			l_tileImage.enabled = a_showState;
		}
		public static void ClearTiles()
		{
			s_Instance.Internal_ClearTiles();
		}
		void Internal_ClearTiles()
		{
			foreach (Image l_tileImg in m_lstTileImages)
			{
				l_tileImg.transform.GetChild(0).GetComponent<Image>().enabled = false;
			}
		}
	}
}
